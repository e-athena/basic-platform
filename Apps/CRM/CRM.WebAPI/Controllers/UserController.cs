using Athena.Infrastructure.Auths;

namespace CRM.WebAPI.Controllers;

/// <summary>
/// 用户控制器
/// </summary>
public class UserController : CustomControllerBase
{
    private readonly IUserService _userService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userService"></param>
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// 读取资源
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [SkipApiPermissionVerification]
    public async Task<IList<MenuTreeInfo>> GetResourcesAsync(
        [FromServices] IApiPermissionService service,
        [FromServices] ISecurityContextAccessor accessor
    )
    {
        var assembly = Assembly.GetExecutingAssembly();
        if (accessor.IsRoot)
        {
            return service.GetFrontEndRoutingResources(assembly, GlobalConstant.DefaultAppId);
        }

        var resources = await _userService.GetUserResourceAsync(UserId!, GlobalConstant.DefaultAppId);
        var keys = resources
            .Where(p => p.AppId == GlobalConstant.DefaultAppId || string.IsNullOrEmpty(p.AppId))
            .Select(p => p.Key)
            .ToList();

        return service.GetPermissionFrontEndRoutingResources(assembly, keys, GlobalConstant.DefaultAppId);
    }

    /// <summary>
    /// 读取外部页面列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [SkipApiPermissionVerification]
    public Task<List<ExternalPageModel>> GetExternalPagesAsync()
    {
        return _userService.GetExternalPagesAsync(UserId!);
    }

    /// <summary>
    /// 更新表格列表信息
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [SkipApiPermissionVerification]
    public Task<long> UpdateUserCustomColumnsAsync([FromBody] UpdateUserCustomColumnsRequest request,
        CancellationToken cancellationToken)
    {
        request.AppId = GlobalConstant.DefaultAppId;
        request.UserId = UserId!;
        return _userService.UpdateUserCustomColumnsAsync(request, cancellationToken);
    }
}