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
    public async Task<string> GetAuthTokenAsync(
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

        // 缓存授权码取token时最长的有效时间
        var expireTime = await _cacheManager.GetAsync<DateTime>(string.Format(SsoCacheKey.SessionExpiry, sessionCode));
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
        var authToken = _securityContextAccessor.CreateToken(new JwtConfig
        {
            AppId = clientId,
            Audience = clientId,
            Issuer = "basic-platform-sso-center",
            Expires = expireSeconds,
            SecurityKey = app.ClientSecret
        }, claims);
        return authToken;
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
}