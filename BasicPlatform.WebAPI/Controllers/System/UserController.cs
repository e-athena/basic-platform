using BasicPlatform.AppService.DataPermissions;
using BasicPlatform.AppService.DataPermissions.Models;
using BasicPlatform.AppService.ExternalPages.Models;
using BasicPlatform.AppService.Users;
using BasicPlatform.AppService.Users.Requests;
using BasicPlatform.AppService.Users.Responses;

namespace BasicPlatform.WebAPI.Controllers.System;

/// <summary>
/// 用户管理
/// </summary>
[Menu("用户管理",
    ModuleCode = "system",
    ModuleName = "系统管理",
    ModuleRoutePath = "/system",
    ModuleSort = 1,
    RoutePath = "/system/user",
    Sort = 0,
    Description = "系统基于角色授权，每个角色对不同的功能模块具备添删改查以及自定义权限等多种权限设定"
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
    /// 读取数据列
    /// </summary>
    /// <param name="commonService"></param>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    [ApiPermission(IsVisible = false)]
    public Task<GetTableColumnsResponse> GetColumnsAsync(
        [FromServices] ICommonService commonService)
    {
        return commonService.GetColumnsAsync<GetUserPagingResponse>();
    }

    /// <summary>
    /// 读取列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiPermission(AdditionalRules = new[]
    {
        ApiPermissionConstant.OrgTreeList
    })]
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
    [ApiPermission(AdditionalRules = new[]
    {
        ApiPermissionConstant.PositionSelectList,
        ApiPermissionConstant.OrgCascaderList,
        ApiPermissionConstant.RoleSelectList
    })]
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
        ApiPermissionConstant.PositionSelectList,
        ApiPermissionConstant.OrgCascaderList,
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

    /// <summary>
    /// 分配权限
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut]
    [ApiPermission(AdditionalRules = new[]
    {
        ApiPermissionConstant.UserDataPermissions
    })]
    public Task<string> AssignDataPermissionsAsync([FromBody] AssignUserDataPermissionsRequest request,
        CancellationToken cancellationToken)
    {
        return _mediator.SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// 更新表格列表信息
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiPermission(IsVisible = false)]
    [AllowAnonymous]
    public Task<long> UpdateUserCustomColumnsAsync([FromBody] UpdateUserCustomColumnsRequest request,
        CancellationToken cancellationToken)
    {
        return _mediator.SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// 重置密码
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>新密码</returns>
    [HttpPost]
    [AllowAnonymous]
    public Task<string> ResetPasswordAsync([FromBody] ResetUserPasswordRequest request,
        CancellationToken cancellationToken)
    {
        return _mediator.SendAsync(request, cancellationToken);
    }

    #endregion

    #region 扩展接口

    /// <summary>
    /// 下拉列表
    /// </summary>
    /// <param name="organizationId">组织Id</param>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission(ApiPermissionConstant.UserSelectList, IsVisible = false)]
    public Task<List<SelectViewModel>> GetSelectListAsync(string? organizationId = null)
    {
        return _queryService.GetSelectListAsync(organizationId);
    }

    /// <summary>
    /// 读取资源
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission(IsVisible = false)]
    [AllowAnonymous]
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

        var resources = await _queryService.GetUserResourceAsync(null);
        var keys = resources.Select(p => p.Key).ToList();
        var result = service.GetPermissionMenuResources(assembly, keys);

        return result;
    }

    /// <summary>
    /// 读取外部页面列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission(IsVisible = false)]
    [AllowAnonymous]
    public Task<IList<ExternalPageModel>> GetExternalPagesAsync()
    {
        return _queryService.GetCurrentUserExternalPagesAsync();
    }

    /// <summary>
    /// 读取拥有的资源代码
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    [ApiPermission(ApiPermissionConstant.UserResourceCodeInfo, IsVisible = false)]
    public Task<GetUserResourceCodeInfoResponse> GetResourceCodeInfoAsync([FromQuery] string id)
    {
        return _queryService.GetResourceCodeInfoAsync(id);
    }

    /// <summary>
    /// 读取数据权限
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission(ApiPermissionConstant.UserDataPermissions, IsVisible = false)]
    public async Task<IList<DataPermissionGroup>> GetDataPermissionsAsync([FromQuery] string id)
    {
        var result = await _queryService.GetDataPermissionsAsync(id);
        var assembly = Assembly.Load("BasicPlatform.AppService");
        return DataPermissionHelper.GetGroupList(
            assembly,
            result.Select(p => new DataPermission
            {
                ResourceKey = p.ResourceKey,
                DataScopeCustom = p.DataScopeCustom,
                DataScope = p.DataScope,
                Enabled = p.Enabled,
                DisableChecked = p.IsRolePermission
            }).ToList()
        );
    }

    #endregion
}