using BasicPlatform.AppService.Organizations;
using BasicPlatform.AppService.Organizations.Requests;
using BasicPlatform.AppService.Organizations.Responses;

namespace BasicPlatform.WebAPI.Controllers.Organizations;

/// <summary>
/// 组织架构管理
/// </summary>
[Menu("组织管理",
    ModuleCode = "organization",
    ModuleName = "组织架构",
    ModuleIcon = "ApartmentOutlined",
    ModuleRoutePath = "/organization",
    ModuleSort = 1,
    RoutePath = "/organization/org",
    Code = "org",
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
        ApiPermissionConstant.OrgTreeList
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
        ApiPermissionConstant.OrgTreeList,
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
    /// 读取树形列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<List<GetOrganizationTreeDataResponse>> GetTreeListAsync(
        [FromBody] GetOrganizationTreeDataRequest request)
    {
        return _queryService.GetTreeListAsync(request);
    }

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
    /// 读取树形选择框数据列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission(ApiPermissionConstant.OrgTreeSelectListForSelf, IsVisible = false)]
    public Task<List<TreeSelectViewModel>> GetTreeSelectListForSelfAsync()
    {
        return _queryService.GetTreeSelectListForSelfAsync();
    }


    /// <summary>
    /// 获取组织架构级联人员信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission("org:cascader", IsVisible = false)]
    public Task<List<CascaderViewModel>> GetCascaderListAsync()
    {
        return _queryService.GetCascaderListAsync();
    }

    #endregion
}