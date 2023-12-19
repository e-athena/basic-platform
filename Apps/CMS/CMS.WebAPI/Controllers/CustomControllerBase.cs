namespace CMS.WebAPI.Controllers;

/// <summary>
/// 控制器基类
///     所有需要登录后才能操作的接口都需要继承此类
/// </summary>
[EnableCors]
[Route("api/[controller]/[action]")]
[ApiController]
[ApiPermissionAuthorize]
[ApiPermissionAuthorizeFilter]
public class CustomControllerBase : ControllerBase
{
    /// <summary>
    /// 用户ID
    /// </summary>
    protected string UserId => HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
}