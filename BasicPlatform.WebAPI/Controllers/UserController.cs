using Athena.Infrastructure.Jwt;
using BasicPlatform.AppService.Users;
using BasicPlatform.AppService.Users.Requests;
using BasicPlatform.AppService.Users.Responses;

namespace BasicPlatform.WebAPI.Controllers;

/// <summary>
/// 用户管理
/// </summary>
[Menu("用户管理",
    ModuleCode = "system",
    ModuleName = "系统模块",
    ModuleIcon = "PicRightOutlined",
    ModuleRoutePath = "/system",
    RoutePath = "/system/user"
    // RoutePath = "/admin"
)]
public class UserController : CustomControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserQueryService _queryService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="queryService"></param>
    public UserController(IMediator mediator, IUserQueryService queryService)
    {
        _mediator = mediator;
        _queryService = queryService;
    }

    #region 基础接口

    /// <summary>
    /// 读取列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<Paging<GetUserPagingResponse>> GetPagingAsync([FromBody] GetUserPagingRequest request)
    {
        return _queryService.GetPagingAsync(request);
    }

    /// <summary>
    /// 根据ID读取
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission(ApiPermissionConstant.UserDetail, DisplayName = "详情")]
    public Task<GetUserByIdResponse> GetAsync([FromQuery] string id)
    {
        return _queryService.GetAsync(id);
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<string> PostAsync([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        return _mediator.SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut]
    [ApiPermission(AdditionalRules = new[]
    {
        ApiPermissionConstant.OrgTreeSelectListForSelf,
        ApiPermissionConstant.RoleSelectList,
        ApiPermissionConstant.UserDetail
    })]
    public Task<string> PutAsync([FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        return _mediator.SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// 状态变更
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut]
    public Task<string> StatusChangeAsync([FromBody] UserStatusChangeRequest request,
        CancellationToken cancellationToken)
    {
        return _mediator.SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// 分配资源
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut]
    public Task<string> AssignResourcesAsync([FromBody] AssignUserResourcesRequest request,
        CancellationToken cancellationToken)
    {
        return _mediator.SendAsync(request, cancellationToken);
    }

    #endregion

    #region 扩展接口

    /// <summary>
    /// 读取用户
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<GetUserResponse> GetUserAsync()
    {
        return _queryService.GetUserAsync(null);
    }

    /// <summary>
    /// 下拉列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<List<SelectViewModel>> GetSelectDataAsync()
    {
        return _queryService.GetSelectDataAsync();
    }


    /// <summary>
    /// 读取组织架构用户树形列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<List<CascaderViewModel>> GetOrganizationUserTreeSelectListAsync()
    {
        return _queryService.GetOrganizationUserTreeSelectListAsync();
    }
    // 分配用户资源
    // Allocate additional resources

    /// <summary>
    /// 读取组织架构和用户树形列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<List<CascaderViewModel>> GetOrganizationAndUserTreeSelectListAsync()
    {
        return _queryService.GetOrganizationAndUserTreeSelectListAsync();
    }

    /// <summary>
    /// 读取资源
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IList<MenuTreeInfo>> GetResourcesAsync(
        [FromServices] IApiPermissionService service,
        [FromServices] ISecurityContextAccessor accessor
    )
    {
        var assembly = Assembly.GetExecutingAssembly();
        if (accessor.IsRoot)
        {
            return service.GetMenuResources(assembly);
        }

        var codes = await _queryService.GetUserResourceAsync(null);
        var result = service.GetPermissionMenuResources(assembly, codes);
        return await Task.FromResult(result);
    }

    /// <summary>
    /// 读取拥有的资源代码
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission(ApiPermissionConstant.UserResourceCodeInfo, IsVisible = false)]
    public Task<GetUserResourceCodeInfoResponse> GetResourceCodeInfoAsync([FromQuery] string id)
    {
        return _queryService.GetResourceCodeInfoAsync(id);
    }

    #endregion
}