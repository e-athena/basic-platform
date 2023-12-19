namespace CMS.WebAPI.Controllers;

/// <summary>
/// 帐户控制器
/// </summary>
[EnableCors]
[Route("api/[controller]/[action]")]
[ApiController]
public class AccountController : ControllerBase
{
    /// <summary>
    /// 当前用户信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    public async Task<dynamic> CurrentUserAsync(
        [FromServices] IApiPermissionCacheService service,
        [FromServices] ISecurityContextAccessor accessor,
        [FromServices] IUserService userService
    )
    {
        var user = await userService.GetUserInfoAsync(accessor.UserId!);
        if (user == null)
        {
            throw FriendlyException.Of("用户不存在");
        }

        var appId = accessor.TenantId;
        var identificationId = user.Id;
        if (user.ResourceCodes.Count == 0)
        {
            // 删除缓存
            await service.RemoveAsync(appId, identificationId);
        }
        else
        {
            // 设置缓存
            await service.SetAsync(
                appId,
                identificationId,
                user.ResourceCodes
            );
        }

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
}