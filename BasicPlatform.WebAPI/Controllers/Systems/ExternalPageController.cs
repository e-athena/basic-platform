using BasicPlatform.AppService.ExternalPages;
using BasicPlatform.AppService.ExternalPages.Requests;
using BasicPlatform.AppService.ExternalPages.Responses;

namespace BasicPlatform.WebAPI.Controllers.Systems;

/// <summary>
/// 外部页面管理
/// </summary>
[Menu("扩展页面",
    ModuleCode = "system",
    ModuleName = "系统模块",
    ModuleRoutePath = "/system",
    ModuleSort = 3,
    
    RoutePath = "/system/external-page",
    Sort = 3,
    Description = "可用于添加外部页面，如：https://www.baidu.com"
)]
public class ExternalPageController : CustomControllerBase
{
    private readonly IExternalPageQueryService _queryService;
    private readonly IMediator _mediator;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="queryService"></param>
    /// <param name="mediator"></param>
    public ExternalPageController(IExternalPageQueryService queryService, IMediator mediator)
    {
        _queryService = queryService;
        _mediator = mediator;
    }

    #region 基础接口

    /// <summary>
    /// 读取列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiPermission(AdditionalRules = new[]
    {
        ApiPermissionConstant.ExternalPageTreeList
    })]
    public Task<Paging<GetExternalPagePagingResponse>> GetPagingAsync([FromBody] GetExternalPagePagingRequest request)
    {
        return _queryService.GetPagingAsync(request);
    }

    /// <summary>
    /// 根据ID读取
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission(ApiPermissionConstant.ExternalPageDetail, DisplayName = "详情")]
    public Task<GetExternalPageByIdResponse?> GetAsync([FromQuery] string id)
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
        ApiPermissionConstant.ExternalPageSelectList
    })]
    public Task<string> PostAsync([FromBody] CreateExternalPageRequest request, CancellationToken cancellationToken)
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
        ApiPermissionConstant.ExternalPageSelectList,
        ApiPermissionConstant.ExternalPageDetail
    })]
    public Task<string> PutAsync([FromBody] UpdateExternalPageRequest request, CancellationToken cancellationToken)
    {
        return _mediator.SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete]
    public Task<string> DeleteAsync([FromBody] DeleteExternalPageRequest request, CancellationToken cancellationToken)
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
    [ApiPermission(ApiPermissionConstant.ExternalPageSelectList, IsVisible = false)]
    public Task<List<SelectViewModel>> GetSelectListAsync()
    {
        return _queryService.GetSelectListAsync();
    }
    
    /// <summary>
    /// 读取树形数据列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission(ApiPermissionConstant.ExternalPageTreeList, IsVisible = false)]
    public Task<List<TreeViewModel>> GetTreeListAsync()
    {
        return _queryService.GetTreeListAsync();
    }
    #endregion
}