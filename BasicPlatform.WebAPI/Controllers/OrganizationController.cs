using BasicPlatform.AppService.Organizations;
using BasicPlatform.AppService.Organizations.Requests;
using BasicPlatform.AppService.Organizations.Responses;

namespace BasicPlatform.WebAPI.Controllers;

/// <summary>
/// 组织架构管理
/// </summary>
[Menu("组织架构",
    ModuleCode = "system",
    ModuleName = "系统模块",
    ModuleRoutePath = "/system",
    // RoutePath = "/system/organization",
    RoutePath = "/list"
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
    /// 读取分页列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
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
    [ApiPermission(DisplayName = "详情")]
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
    [ApiPermission(DisplayName = "编辑")]
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
    [ApiPermission(DisplayName = "切换状态")]
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
    [ApiPermission(IsVisible = false, AdditionalRules = new[] {"org:tree"})]
    public Task<List<TreeViewModel>> GetTreeDataAsync()
    {
        return _queryService.GetTreeDataAsync();
    }

    /// <summary>
    /// 读取树形下拉数据列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission(IsVisible = false, AdditionalRules = new[] {"org:treeSelect"})]
    public Task<List<TreeSelectViewModel>> GetTreeSelectDataAsync()
    {
        return _queryService.GetTreeSelectDataAsync();
    }

    /// <summary>
    /// 读取树形选择框数据列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission(IsVisible = false, AdditionalRules = new[] {"org:treeSelectForSelf"})]
    public Task<List<TreeSelectViewModel>> GetTreeSelectDataForSelfAsync()
    {
        return _queryService.GetTreeSelectDataForSelfAsync();
    }

    /// <summary>
    /// 读取树形列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<List<GetOrganizationTreeDataResponse>> GetTreeDataAsync(
        [FromBody] GetOrganizationTreeDataRequest request)
    {
        return _queryService.GetTreeDataAsync(request);
    }

    /// <summary>
    /// 获取组织架构级联人员信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission(IsVisible = false, AdditionalRules = new[] {"org:cascader"})]
    public Task<List<CascaderViewModel>> GetCascaderDataAsync()
    {
        return _queryService.GetCascaderDataAsync();
    }

    #endregion
}