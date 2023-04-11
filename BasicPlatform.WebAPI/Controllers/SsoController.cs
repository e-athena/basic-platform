using System.Security.Claims;
using BasicPlatform.AppService.Applications;
using BasicPlatform.AppService.Users.Models;

namespace BasicPlatform.WebAPI.Controllers;

/// <summary>
/// SSO控制器
/// </summary>
[EnableCors]
[Route("api/[controller]/[action]")]
[ApiController]
public class SsoController : ControllerBase
{
    private readonly IApplicationQueryService _applicationQueryService;
    private readonly ISecurityContextAccessor _securityContextAccessor;
    private readonly ICacheManager _cacheManager;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cacheManager"></param>
    /// <param name="applicationQueryService"></param>
    /// <param name="securityContextAccessor"></param>
    public SsoController(ICacheManager cacheManager,
        IApplicationQueryService applicationQueryService,
        ISecurityContextAccessor securityContextAccessor)
    {
        _cacheManager = cacheManager;
        _applicationQueryService = applicationQueryService;
        _securityContextAccessor = securityContextAccessor;
    }

    /// <summary>
    /// 读取授权码
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="sessionCode"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    public async Task<string> GetAuthCodeAsync(
        [FromQuery] string clientId,
        [FromQuery] string sessionCode
    )
    {
        // 检查应用是否存在
        var app = await _applicationQueryService.GetByClientIdAsync(clientId);
        if (app == null)
        {
            throw FriendlyException.Of("应用不存在");
        }

        // 检查用户是否已登录
        var currentUserInfo =
            await _cacheManager.GetAsync<UserQueryModel>(string.Format(SsoCacheKey.CurrentUserInfo, sessionCode));
        // 如果找不到用户信息，说明会话已过期
        if (currentUserInfo == null)
        {
            throw FriendlyException.Of("会话不存在或已过期");
        }

        // 生成授权码
        var authCode = Guid.NewGuid().ToString("N").ToUpper();
        // 缓存授权码
        await _cacheManager.SetAsync(
            string.Format(SsoCacheKey.AuthCode, authCode),
            currentUserInfo,
            TimeSpan.FromMinutes(10));
        // 缓存授权码是哪个应用的
        await _cacheManager.SetStringAsync(
            string.Format(SsoCacheKey.AuthCodeClientId, authCode),
            clientId,
            TimeSpan.FromMinutes(10)
        );
        // 缓存授权码取token时最长的有效时间
        var expireTime = await _cacheManager.GetAsync<DateTime>(string.Format(SsoCacheKey.SessionExpiry, sessionCode));
        await _cacheManager.SetAsync(
            string.Format(SsoCacheKey.AuthCodeSessionTime, authCode),
            expireTime,
            expireTime - DateTime.Now
        );
        return authCode;
    }

    /// <summary>
    /// 读取Token
    /// </summary>
    /// <param name="authCode"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<string> GetTokenAsync([FromQuery] string authCode)
    {
        var authCodeKey = string.Format(SsoCacheKey.AuthCode, authCode);
        var authCodeClientIdKey = string.Format(SsoCacheKey.AuthCodeClientId, authCode);
        var authCodeSessionTimeKey = string.Format(SsoCacheKey.AuthCodeSessionTime, authCode);

        var currentUserInfo = await _cacheManager.GetAsync<UserQueryModel>(authCodeKey);
        // 如果找不到用户信息，说明会话已过期
        if (currentUserInfo == null)
        {
            throw FriendlyException.Of("授权码不存在或已过期");
        }

        // 清除授权码，一个授权码只能用一次
        await _cacheManager.RemoveAsync(authCodeKey);
        // 读取应用ID
        var clientId = await _cacheManager.GetStringAsync(authCodeClientIdKey);
        if (clientId == null)
        {
            throw FriendlyException.Of("授权码不存在或已过期");
        }

        // 读取应用信息
        var app = await _applicationQueryService.GetByClientIdAsync(clientId);
        if (app == null)
        {
            throw FriendlyException.Of("应用不存在");
        }

        // 读取授权码对应的会话过期时间
        var sessionTime = await _cacheManager.GetAsync<DateTime>(authCodeSessionTimeKey);
        // Token过期时间
        var expireTime = sessionTime > DateTime.Now.AddMinutes(30) ? DateTime.Now.AddMinutes(30) : sessionTime;
        // 根据当前时间和授权码对应的会话过期时间，计算出Token的过期时间/秒
        var expireSeconds = (int) (expireTime - DateTime.Now).TotalSeconds;
        // Claims
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, currentUserInfo.Id!),
            new(ClaimTypes.Name, currentUserInfo.UserName),
            new("RealName", currentUserInfo.RealName)
        };
        // 生成Token
        var token = _securityContextAccessor.CreateToken(new JwtConfig
        {
            AppId = clientId,
            Audience = clientId,
            Issuer = "basic-platform-sso-center",
            Expires = expireSeconds,
            SecurityKey = app.ClientSecret
        }, claims);
        return token;
    }
}

/// <summary>
/// SSO缓存键
/// </summary>
public static class SsoCacheKey
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public const string CurrentUserInfo = "user:{0}";

    /// <summary>
    /// 会话过期时间
    /// </summary>
    public const string SessionExpiry = "sso:sessionExpiry:{0}";

    /// <summary>
    /// 授权码
    /// </summary>
    public const string AuthCode = "sso:authCode:{0}";

    /// <summary>
    /// 授权码对应的应用
    /// </summary>
    public const string AuthCodeClientId = "sso:authCodeClientId:{0}";

    /// <summary>
    /// 授权码会话过期时间
    /// </summary>
    public const string AuthCodeSessionTime = "sso:authCodeSessionTime:{0}";
}