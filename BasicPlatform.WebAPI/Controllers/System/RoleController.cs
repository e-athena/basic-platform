using BasicPlatform.AppService.Roles;
using BasicPlatform.AppService.Roles.Requests;
using BasicPlatform.AppService.Roles.Responses;

namespace BasicPlatform.WebAPI.Controllers.System;

/// <summary>
/// 角色管理
/// </summary>
[Menu("角色管理",
    ModuleCode = "system",
    ModuleName = "系统管理",
    ModuleRoutePath = "/system",
    ModuleSort = 1,
    RoutePath = "/system/role",
    Sort = 1,
    Description = "系统基于角色授权，每个角色对不同的功能模块具备添删改查以及自定义权限等多种权限设定"
)]
public class RoleController : CustomControllerBase
{
    private readonly IMediator _mediator;
    private readonly IRoleQueryService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="service"></param>
    public RoleController(IMediator mediator, IRoleQueryService service)
    {
        _mediator = mediator;
        _service = service;
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
        return commonService.GetColumnsAsync<GetRolePagingResponse>();
    }

    /// <summary>
    /// 读取列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<Paging<GetRolePagingResponse>> GetPagingAsync([FromBody] GetRolePagingRequest request)
    {
        return _service.GetPagingAsync(request);
    }

    /// <summary>
    /// 根据ID读取
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission(ApiPermissionConstant.RoleDetail, DisplayName = "详情")]
    public Task<GetRoleByIdResponse> GetAsync([FromQuery] string id)
    {
        return _service.GetAsync(id);
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<string> PostAsync([FromBody] CreateRoleRequest request, CancellationToken cancellationToken)
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
    [ApiPermission(AdditionalRules = new[] {ApiPermissionConstant.RoleDetail})]
    public Task<string> PutAsync([FromBody] UpdateRoleRequest request, CancellationToken cancellationToken)
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
    public Task<string> StatusChangeAsync([FromBody] RoleStatusChangeRequest request,
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
    [ApiPermission(AdditionalRules = new[] {ApiPermissionConstant.RoleDetail})]
    public Task<string> AssignResourcesAsync([FromBody] AssignRoleResourcesRequest request,
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
        ApiPermissionConstant.RoleDataPermissions
    })]
    public Task<string> AssignDataPermissionsAsync([FromBody] AssignRoleDataPermissionsRequest request,
        CancellationToken cancellationToken)
    {
        return _mediator.SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// 分配用户
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut]
    [ApiPermission(AdditionalRules = new[]
    {
        ApiPermissionConstant.RoleDetail
    })]
    public Task<string> AssignUsersAsync([FromBody] AssignRoleUsersRequest request,
        CancellationToken cancellationToken)
    {
        return _mediator.SendAsync(request, cancellationToken);
    }

    #endregion

    #region 扩展接口

    /// <summary>
    /// 读取下拉列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission(ApiPermissionConstant.RoleSelectList, IsVisible = false)]
    public Task<List<SelectViewModel>> GetSelectListAsync()
    {
        return _service.GetSelectListAsync();
    }

    /// <summary>
    /// 读取数据权限
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission(ApiPermissionConstant.RoleDataPermissions, IsVisible = false)]
    public async Task<IList<DataPermissionGroup>> GetDataPermissionsAsync([FromQuery] string id)
    {
        var result = await _service.GetDataPermissionsAsync(id);
        var assembly = Assembly.Load("BasicPlatform.AppService");
        return DataPermissionHelper.GetGroupList(
            assembly,
            result.Select(p => new DataPermission
            {
                ResourceKey = p.ResourceKey,
                DataScopeCustom = p.DataScopeCustom,
                DataScope = p.DataScope,
                Enabled = p.Enabled,
                QueryFilterGroups = p.QueryFilterGroups?.ToList() ?? new List<QueryFilterGroup>()
            }).ToList());
    }

    #endregion
}