using BasicPlatform.AppService.Tenants.Requests;
using BasicPlatform.AppService.Tenants.Responses;
using BasicPlatform.AppService.Users;
using BasicPlatform.AppService.Users.Models;
using BasicPlatform.AppService.Users.Requests;
using BasicPlatform.WebAPI.Services;

namespace BasicPlatform.WebAPI.Controllers.System;

/// <summary>
/// 租户管理
/// </summary>
[FrontEndRouting("租户管理",
    ModuleCode = "system",
    ModuleName = "系统管理",
    ModuleRoutePath = "/system",
    ModuleSort = 1,
    RoutePath = "/system/tenant",
    Sort = 0,
    Description = "每个租户都相关于一个平行宇宙，数据完全隔离。"
)]
public class TenantController : CustomControllerBase
{
    private readonly IMediator _mediator;
    private readonly ITenantQueryService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="service"></param>
    public TenantController(IMediator mediator, ITenantQueryService service)
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
    [SkipApiPermissionVerification]
    [ApiPermission(IsVisible = false)]
    public Task<GetTableColumnsResponse> GetColumnsAsync(
        [FromServices] ICommonService commonService)
    {
        return commonService.GetColumnsAsync<GetTenantPagingResponse>();
    }

    /// <summary>
    /// 读取列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<Paging<GetTenantPagingResponse>> GetPagingAsync([FromBody] GetTenantPagingRequest request)
    {
        return _service.GetPagingAsync(request);
    }

    /// <summary>
    /// 根据ID读取
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission(ApiPermissionConstant.TenantDetail, DisplayName = "详情")]
    public Task<GetTenantDetailResponse> GetAsync([FromQuery] string id)
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
    [ApiPermission(AdditionalRules = new[]
    {
        ApiPermissionConstant.ApplicationSelectList
    })]
    public Task<string> PostAsync([FromBody] CreateTenantRequest request, CancellationToken cancellationToken)
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
    [ApiPermission(AdditionalRules = new[] {ApiPermissionConstant.TenantDetail})]
    public Task<string> PutAsync([FromBody] UpdateTenantRequest request, CancellationToken cancellationToken)
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
    public Task<string> StatusChangeAsync([FromBody] ChangeTenantStatusRequest request,
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
    [ApiPermission(AdditionalRules = new[] {ApiPermissionConstant.TenantDetail})]
    public Task<string> AssignResourcesAsync([FromBody] AssignTenantResourcesRequest request,
        CancellationToken cancellationToken)
    {
        return _mediator.SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// 同步数据库
    /// </summary>
    /// <param name="freeSql"></param>
    /// <param name="service"></param>
    /// <param name="code"></param>
    [HttpGet]
    public async Task SyncStructureAsync(
        [FromServices] IFreeSql freeSql,
        [FromServices] ISubApplicationService service,
        [FromQuery] string code
    )
    {
        await service.SyncDatabaseAsync(code);
        freeSql.SyncStructure("BasicPlatform.Domain");
    }

    #endregion

    #region 超级管理员

    /// <summary>
    /// 读取超级管理员
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission(ApiPermissionConstant.TenantAdminDetail, IsVisible = false)]
    public Task<UserModel> GetSuperAdminAsync([FromServices] IUserQueryService service)
    {
        return service.GetTenantSuperAdminAsync();
    }

    /// <summary>
    /// 创建超级管理员
    /// </summary>
    /// <param name="freeSql"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiPermission(AdditionalRules = new[]
    {
        ApiPermissionConstant.TenantAdminDetail
    })]
    public async Task CreateSuperAdminAsync(
        [FromServices] IFreeSql freeSql,
        [FromBody] CreateUserRequest request,
        CancellationToken cancellationToken
    )
    {
        freeSql.SyncStructure("BasicPlatform.Domain");
        request.IsTenantAdmin = true;
        await _mediator.SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// 更新超级管理员
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut]
    [ApiPermission(AdditionalRules = new[]
    {
        ApiPermissionConstant.TenantAdminDetail
    })]
    public async Task UpdateSuperAdminAsync(
        [FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken
    )
    {
        request.IsTenantAdmin = true;
        await _mediator.SendAsync(request, cancellationToken);
    }

    #endregion
}