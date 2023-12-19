using System.Security.Claims;
using Athena.Infrastructure.Mvc.Messaging.Requests;
using Athena.InstantMessaging;
using Athena.InstantMessaging.Models;
using BasicPlatform.AppService.Users;
using BasicPlatform.AppService.Users.Requests;

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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="securityContextAccessor"></param>
    /// <param name="service"></param>
    public AccountController(ISecurityContextAccessor securityContextAccessor, IUserQueryService service)
    {
        _securityContextAccessor = securityContextAccessor;
        _service = service;
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="options"></param>
    /// <param name="mediator"></param>
    /// <param name="cacheManager"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<dynamic> LoginAsync(
        [FromServices] IOptionsMonitor<JwtConfig> options,
        [FromServices] IMediator mediator,
        [FromServices] ICacheManager cacheManager,
        [FromBody] LoginRequest request
    )
    {
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
            throw FriendlyException.Of("帐户不可用,请联系管理员");
        }

        // 更新登录信息
        await mediator.SendAsync(new UpdateUserLoginInfoRequest
        {
            Id = info.Id!
        });

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
            new("RealName", info.RealName),
            new("IsTenantAdmin", info.IsTenantAdmin ? "true" : "false"),
            new("TenantId", _securityContextAccessor.TenantId ?? string.Empty)
        };
        var sessionCode = Guid.NewGuid().ToString("N").ToUpper();
        var cacheTime = TimeSpan.FromSeconds(options.CurrentValue.Expires);
        var key = string.Format(SsoCacheKey.CurrentUserInfo, sessionCode);
        await cacheManager.SetAsync(key, info, cacheTime);
        // 设置会话过期时间
        await cacheManager.SetAsync(
            string.Format(SsoCacheKey.SessionExpiry, sessionCode),
            DateTime.Now.AddSeconds(options.CurrentValue.Expires),
            cacheTime
        );
        var token = _securityContextAccessor.CreateToken(claims);
        return new
        {
            Status = "ok",
            Type = "account",
            CurrentAuthority = token,
            SessionCode = sessionCode
        };
    }

    /// <summary>
    /// 当前用户信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<dynamic> CurrentUserAsync(
        [FromServices] INoticeHubService noticeHubService,
        [FromServices] IApiPermissionCacheService service,
        [FromServices] ISecurityContextAccessor accessor
    )
    {
        var user = await _service.GetCurrentUserAsync();
        var appId = accessor.TenantId;
        var identificationId = user.Id;
        if (user.ResourceCodes.Count == 0)
        {
            // 删除缓存
            await service.RemoveAsync(appId, identificationId!);
        }
        else
        {
            // 设置缓存
            await service.SetAsync(
                appId,
                identificationId!,
                user.ResourceCodes
            );
        }

        // 发送上线通知
        await noticeHubService.SendMessageToAllAsync(new InstantMessaging<string>
        {
            NoticeType = "OnlineNotice",
            Data = $"{user.RealName}上线啦~",
            Type = MessageType.Notice,
        });

        return await Task.FromResult(new
        {
            UserId = user.Id,
            Avatar = user.Avatar ?? "https://gw.alipayobjects.com/zos/antfincdn/XAosXuNZyF/BiazfanxmamNRoxxVxka.png",
            user.Email,
            user.PhoneNumber,
            user.RealName,
            user.UserName,
            Group = user.OrganizationName,
            Title = user.PositionName,
            Country = "中国",
            Signature = "无个性，不签名。",
            Geographic = new
            {
                Province = new
                {
                    Label = "广东省",
                    Value = "440000"
                },
                City = new
                {
                    Label = "广州市",
                    Value = "440100"
                }
            },
            Address = "广东省广州市",
            NotifyCount = 12,
            UnreadCount = 11,
            Tags = new List<dynamic>
            {
                new
                {
                    Label = "设计师",
                    Value = "设计师"
                },
                new
                {
                    Label = "程序员",
                    Value = "程序员"
                }
            }
        });
    }

    /// <summary>
    /// 添加用户访问记录
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<long> AddUserAccessRecordAsync(
        [FromServices] IMediator mediator,
        [FromBody] AddUserAccessRecordRequest request,
        CancellationToken cancellationToken
    )
    {
        return mediator.SendAsync(request, cancellationToken);
    }
}