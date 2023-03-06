using System.Security.Claims;
using Athena.Infrastructure;
using Athena.Infrastructure.Exceptions;
using Athena.Infrastructure.Jwt;
using Athena.Infrastructure.Mvc.Messaging.Requests;
using BasicPlatform.AppService.Users;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace BasicPlatform.WebAPI.Controllers;

/// <summary>
/// 帐户控制器
/// </summary>
[EnableCors]
[Route("api/[controller]/[action]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly ISecurityContextAccessor _securityContextAccessor;
    private readonly IUserQueryService _service;
    private readonly ILogger<AccountController> _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="securityContextAccessor"></param>
    /// <param name="service"></param>
    /// <param name="loggerFactory"></param>
    public AccountController(ISecurityContextAccessor securityContextAccessor, IUserQueryService service,
        ILoggerFactory loggerFactory)
    {
        _securityContextAccessor = securityContextAccessor;
        _logger = loggerFactory.CreateLogger<AccountController>();
        _service = service;
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<dynamic> LoginAsync(LoginRequest request)
    {
        _logger.LogInformation("有人登录啦，{@Request}", request);
        // var token = _securityContextAccessor.CreateToken(new List<Claim>
        // {
        //     new(ClaimTypes.Name, request.UserName),
        //
        //     new(ClaimTypes.NameIdentifier, "63a4897bbd3497da92a27f5b"),
        //     new(ClaimTypes.Role, ObjectId.GenerateNewStringId()),
        //     new("RoleName", "admin"),
        //     new(ClaimTypes.Name, request.UserName),
        //     new("RealName", request.UserName)
        // });
        //
        // return await Task.FromResult(new
        // {
        //     Status = "ok",
        //     Type = "account",
        //     CurrentAuthority = token,
        // });
        // 读取用户信息
        var info = await _service.GetByUserNameAsync(request.UserName);
        // 验证密码
        if (!info.PasswordEquals(request.Password))
        {
            throw FriendlyException.Of("登录名或密码错误");
        }

        // 验证状态
        if (!info.IsEnabled)
        {
            throw FriendlyException.Of("帐户已被禁用,请联系管理员");
        }

        var role = "";
        var roleName = "";
        if (info.Roles.Count > 0)
        {
            role = info.Roles.Aggregate(role, (current, item) => current + item.Id + ",");
            role = role[..^1];

            roleName = info.Roles.Aggregate(roleName, (current, infoRole) => current + $"{infoRole.Name},");
            roleName = roleName[..^1];
        }

        // Claims
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, info.Id!),
            new(ClaimTypes.Role, role),
            new("RoleName", roleName),
            new(ClaimTypes.Name, info.UserName),
            new("RealName", info.RealName)
        };

        var token = _securityContextAccessor.CreateToken(claims);

        return new
        {
            Status = "ok",
            Type = "account",
            CurrentAuthority = token,
        };
    }

    /// <summary>
    /// 当前用户信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<dynamic> CurrentUserAsync(
        [FromServices] IApiPermissionCacheService apiPermissionCacheService,
        [FromServices] ISecurityContextAccessor accessor
    )
    {
        var user = await _service.GetCurrentUserAsync();
        if (user.ResourceCodes.Count == 0)
        {
            // 删除缓存
            await apiPermissionCacheService
                .RemoveOperationPermissionsAsync(accessor.TenantId, user.Id!);
        }
        else
        {
            // 设置缓存
            await apiPermissionCacheService
                .SetOperationPermissionsAsync(accessor.TenantId, user.Id!, user.ResourceCodes);
        }

        return await Task.FromResult(new
        {
            user.Id,
            Avatar = "https://gw.alipayobjects.com/zos/antfincdn/XAosXuNZyF/BiazfanxmamNRoxxVxka.png",
            user.Email,
            user.PhoneNumber,
            user.RealName,
            user.UserName,
            Roles = "admin",
            Access = "admin"
        });
        // return await Task.FromResult(new
        // {
        //     Id = 1,
        //     Avatar = "https://gw.alipayobjects.com/zos/antfincdn/XAosXuNZyF/BiazfanxmamNRoxxVxka.png",
        //     Email = "5fd2@c1xa4mje.com.cn",
        //     PhoneNumber = "13979442544",
        //     RealName = "锺国贤",
        //     UserName = "admin",
        //     Name = "锺国贤",
        //     Roles = "admin",
        //     Access = "admin"
        // });
    }
}