using BasicPlatform.AppService.Organizations;
using BasicPlatform.AppService.Organizations.Requests;
using BasicPlatform.AppService.Organizations.Responses;

namespace BasicPlatform.WebAPI.Controllers.System;

/// <summary>
/// 组织架构管理
/// </summary>
[Menu("组织管理",
    ModuleCode = "system",
    ModuleName = "系统管理",
    ModuleRoutePath = "/system",
    ModuleSort = 1,
    RoutePath = "/system/organization",
    Sort = 2,
    Description = "组织机构,部门，多级树状结构"
)]
public class OrganizationController : CustomControllerBase
{
    private readonly IOrganizationQueryService _queryService;
    private readonly IMediator _mediator;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="queryService"></param>
    /// <param name="mediator"></param>
    public OrganizationController(IOrganizationQueryService queryService, IMediator mediator)
    {
        _queryService = queryService;
        _mediator = mediator;
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
        return commonService.GetColumnsAsync<GetOrganizationPagingResponse>();
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
    public Task<Paging<GetOrganizationPagingResponse>> GetPagingAsync([FromBody] GetOrganizationPagingRequest request)
    {
        return _queryService.GetPagingAsync(request);
    }

    /// <summary>
    /// 根据ID读取
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission(ApiPermissionConstant.OrgDetail, DisplayName = "详情")]
    public Task<GetOrganizationByIdResponse?> GetAsync([FromQuery] string id)
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
        ApiPermissionConstant.UserSelectList,
        ApiPermissionConstant.OrgCascaderList
    })]
    public Task<string> PostAsync([FromBody] CreateOrganizationRequest request, CancellationToken cancellationToken)
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
    [ApiPermission(DisplayName = "编辑", AdditionalRules = new[]
    {
        ApiPermissionConstant.UserSelectList,
        ApiPermissionConstant.OrgCascaderList,
        ApiPermissionConstant.OrgDetail
    })]
    public Task<string> PutAsync([FromBody] UpdateOrganizationRequest request, CancellationToken cancellationToken)
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
    [ApiPermission(DisplayName = "状态变更")]
    public Task<string> StatusChangeAsync([FromBody] OrganizationStatusChangeRequest request,
        CancellationToken cancellationToken)
    {
        return _mediator.SendAsync(request, cancellationToken);
    }

    #endregion

    #region 扩展接口

    /// <summary>
    /// 读取树形数据列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission(ApiPermissionConstant.OrgTreeList, IsVisible = false)]
    public Task<List<TreeViewModel>> GetTreeListAsync()
    {
        return _queryService.GetTreeListAsync();
    }

    /// <summary>
    /// 读取树形下拉数据列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission(ApiPermissionConstant.OrgTreeSelectList, IsVisible = false)]
    public Task<List<TreeSelectViewModel>> GetTreeSelectListAsync()
    {
        return _queryService.GetTreeSelectListAsync();
    }

    /// <summary>
    /// 获取组织架构级联信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission(ApiPermissionConstant.OrgCascaderList, IsVisible = false)]
    public Task<List<CascaderViewModel>> GetCascaderListAsync()
    {
        return _queryService.GetCascaderListAsync();
    }

    /// <summary>
    /// 下拉列表
    /// </summary>
    /// <param name="parentId">上级组织Id</param>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission(ApiPermissionConstant.OrgSelectList, IsVisible = false)]
    public Task<List<SelectViewModel>> GetSelectListAsync(string? parentId = null)
    {
        return _queryService.GetSelectListAsync(parentId);
    }

    #endregion
}