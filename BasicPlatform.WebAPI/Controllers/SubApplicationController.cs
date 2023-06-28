using Athena.Infrastructure.QueryFilters;
using BasicPlatform.AppService.ExternalPages.Models;
using BasicPlatform.AppService.Organizations;
using BasicPlatform.AppService.Positions;
using BasicPlatform.AppService.Roles;
using BasicPlatform.AppService.Tenants;
using BasicPlatform.AppService.Users;
using BasicPlatform.AppService.Users.Models;
using BasicPlatform.AppService.Users.Requests;
using BasicPlatform.AppService.Users.Responses;
using BasicPlatform.WebAPI.Attributes;

namespace BasicPlatform.WebAPI.Controllers;

/// <summary>
/// 
/// </summary>
[EnableCors]
[Route("api/[controller]/[action]")]
[ApiController]
[SubApplicationFilter]
public class SubApplicationController : ControllerBase
{
    private readonly IUserQueryService _queryService;
    private readonly IMediator _mediator;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="queryService"></param>
    /// <param name="mediator"></param>
    public SubApplicationController(IUserQueryService queryService, IMediator mediator)
    {
        _queryService = queryService;
        _mediator = mediator;
    }

    /// <summary>
    /// 读取用户自定义列列表
    /// </summary>
    /// <param name="appId"></param>
    /// <param name="moduleName"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<List<UserCustomColumnModel>> GetUserCustomColumnsAsync(string appId, string moduleName, string userId)
    {
        return _queryService.GetUserCustomColumnsAsync(appId, moduleName, userId);
    }

    #region 用户

    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<string> CreateUserAsync([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        return _mediator.SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// 更新用户
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<string> UpdateUserAsync([FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        return _mediator.SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// 读取用户资源
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="appId"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<List<ResourceModel>> GetUserResourceAsync(
        [FromQuery] string userId,
        [FromQuery] string appId
    )
    {
        return _queryService.GetUserResourceAsync(userId, appId);
    }

    /// <summary>
    /// 读取用户资源代码
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="appId"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<List<string>> GetUserResourceCodesAsync(
        [FromQuery] string userId,
        [FromQuery] string appId
    )
    {
        return _queryService.GetResourceCodesAsync(userId, appId);
    }

    /// <summary>
    /// 读取用户信息
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<GetCurrentUserResponse> GetUserInfoAsync([FromQuery] string userId)
    {
        return _queryService.GetCurrentUserAsync(userId);
    }

    /// <summary>
    /// 读取用户下拉列表
    /// </summary>
    /// <param name="organizationId">组织Id</param>
    /// <returns></returns>
    [HttpGet]
    public Task<List<SelectViewModel>> GetUserSelectListAsync(string? organizationId = null)
    {
        return _queryService.GetSelectListAsync(organizationId);
    }

    /// <summary>
    /// 读取用户ID
    /// </summary>
    /// <param name="userName">登录名</param>
    /// <returns></returns>
    [HttpGet]
    public Task<string> GetUserIdByUserNameAsync([FromQuery] string userName)
    {
        return _queryService.GetIdByUserNameAsync(userName);
    }

    /// <summary>
    /// 读取用户下拉列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<List<SelectViewModel>> GetAllUserSelectListAsync()
    {
        return _queryService.GetAllSelectListAsync();
    }

    #region 数据查询权限相关

    /// <summary>
    /// 读取用户读取查询过滤器
    /// </summary>
    /// <param name="service"></param>
    /// <param name="userId">用户ID</param>
    /// <param name="resourceKey">资源Key</param>
    /// <param name="appId">应用ID</param>
    /// <returns></returns>
    [HttpGet]
    public Task<List<QueryFilterGroup>> GetUserPolicyQueryFilterGroupsAsync(
        [FromServices] IDataPermissionService service,
        [FromQuery] string userId,
        [FromQuery] string resourceKey,
        [FromQuery] string? appId
    )
    {
        return service.GetPolicyQueryFilterGroupsAsync(userId, resourceKey, appId);
    }

    /// <summary>
    /// 读取用户组织架构ID列表
    /// </summary>
    /// <param name="service"></param>
    /// <param name="userId">用户ID</param>
    /// <param name="appId">应用ID</param>
    /// <returns></returns>
    [HttpGet]
    public Task<List<string>> GetUserOrganizationIdsAsync(
        [FromServices] IDataPermissionService service,
        [FromQuery] string userId,
        [FromQuery] string? appId
    )
    {
        return service.GetUserOrganizationIdsAsync(userId, appId);
    }

    /// <summary>
    /// 读取用户组织架构及下级组织架构ID列表
    /// </summary>
    /// <param name="service"></param>
    /// <param name="userId">用户ID</param>
    /// <param name="appId">应用ID</param>
    /// <returns></returns>
    [HttpGet]
    public Task<List<string>> GetUserOrganizationIdsTreeAsync(
        [FromServices] IDataPermissionService service,
        [FromQuery] string userId,
        [FromQuery] string? appId
    )
    {
        return service.GetUserOrganizationIdsTreeAsync(userId, appId);
    }

    /// <summary>
    /// 读取用户角色的数据范围列表
    /// </summary>
    /// <param name="service"></param>
    /// <param name="userId">用户ID</param>
    /// <param name="appId">应用ID</param>
    /// <returns></returns>
    [HttpGet]
    public Task<List<DataPermission>> GetUserDataScopesAsync(
        [FromServices] IDataPermissionService service,
        [FromQuery] string userId,
        [FromQuery] string? appId
    )
    {
        return service.GetUserDataScopesAsync(userId, appId);
    }

    #endregion

    #endregion

    #region 部门

    /// <summary>
    /// 读取部门下拉列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<List<SelectViewModel>> GetPositionSelectListAsync(
        [FromServices] IPositionQueryService service,
        [FromQuery] string? organizationId = null)
    {
        return service.GetSelectListAsync(organizationId);
    }

    #endregion

    #region 组织架构

    /// <summary>
    /// 获取组织架构级联列表
    /// </summary>
    /// <param name="service"></param>
    /// <param name="organizationId"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<List<CascaderViewModel>> GetOrganizationCascaderListAsync(
        [FromServices] IOrganizationQueryService service,
        [FromQuery] string? organizationId = null
    )
    {
        return service.GetCascaderListAsync(organizationId);
    }

    /// <summary>
    /// 获取组织架构下拉列表
    /// </summary>
    /// <param name="service"></param>
    /// <param name="organizationId"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<List<SelectViewModel>> GetOrganizationSelectListAsync(
        [FromServices] IOrganizationQueryService service,
        [FromQuery] string? organizationId = null
    )
    {
        return service.GetSelectListAsync(organizationId);
    }

    /// <summary>
    /// 读取组织架构树形列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<List<TreeViewModel>> GetOrganizationTreeListAsync(
        [FromServices] IOrganizationQueryService service,
        [FromQuery] string? organizationId = null
    )
    {
        return service.GetTreeListAsync(organizationId);
    }

    /// <summary>
    /// 读取组织架构ID
    /// </summary>
    /// <param name="service"></param>
    /// <param name="organizationName"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<string> GetOrganizationIdByNameAsync(
        [FromServices] IOrganizationQueryService service,
        [FromQuery] string organizationName
    )
    {
        return service.GetIdByNameAsync(organizationName);
    }

    #endregion

    #region 角色

    /// <summary>
    /// 读取角色下拉列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<List<SelectViewModel>> GetRoleSelectListAsync(
        [FromServices] IRoleQueryService service,
        [FromQuery] string organizationId)
    {
        return service.GetSelectListAsync(organizationId);
    }

    #endregion

    #region 租户

    /// <summary>
    /// 读取租户连接字符串
    /// </summary>
    /// <param name="service"></param>
    /// <param name="tenantCode"></param>
    /// <param name="appId"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<string> GetTenantConnectionStringAsync(
        [FromServices] ITenantQueryService service,
        [FromQuery] string tenantCode,
        [FromQuery] string appId
    )
    {
        return service.GetConnectionStringAsync(tenantCode, appId);
    }

    #endregion

    #region 其他

    /// <summary>
    /// 更新表格列表信息
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<long> UpdateUserCustomColumnsAsync([FromBody] UpdateUserCustomColumnsRequest request,
        CancellationToken cancellationToken)
    {
        return _mediator.SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// 读取外部页面列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<IList<ExternalPageModel>> GetExternalPagesAsync(string userId)
    {
        return _queryService.GetUserExternalPagesAsync(userId);
    }

    #endregion
}