using BasicPlatform.AppService.Positions;
using BasicPlatform.AppService.Positions.Requests;
using BasicPlatform.AppService.Positions.Responses;

namespace BasicPlatform.WebAPI.Controllers.System;

/// <summary>
/// 职位管理
/// </summary>
[FrontEndRouting("职位管理",
    ModuleCode = "system",
    ModuleName = "系统管理",
    ModuleRoutePath = "/system",
    ModuleSort = 1,
    RoutePath = "/system/position",
    Sort = 3,
    Description = "员工职位，如总经理、销售经理、销售员等"
)]
public class PositionController : CustomControllerBase
{
    private readonly IMediator _mediator;
    private readonly IPositionQueryService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="service"></param>
    public PositionController(IMediator mediator, IPositionQueryService service)
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
        return commonService.GetColumnsAsync<GetPositionPagingResponse>();
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
    public Task<Paging<GetPositionPagingResponse>> GetPagingAsync([FromBody] GetPositionPagingRequest request)
    {
        return _service.GetPagingAsync(request);
    }

    /// <summary>
    /// 根据ID读取
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission(ApiPermissionConstant.PositionDetail, DisplayName = "详情")]
    public Task<GetPositionByIdResponse> GetAsync([FromQuery] string id)
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
        ApiPermissionConstant.OrgCascaderList
    })]
    public Task<string> PostAsync([FromBody] CreatePositionRequest request, CancellationToken cancellationToken)
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
        ApiPermissionConstant.OrgCascaderList,
        ApiPermissionConstant.PositionDetail
    })]
    public Task<string> PutAsync([FromBody] UpdatePositionRequest request, CancellationToken cancellationToken)
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
    public Task<string> StatusChangeAsync([FromBody] PositionStatusChangeRequest request,
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
    [ApiPermission(ApiPermissionConstant.PositionSelectList, IsVisible = false)]
    public Task<List<SelectViewModel>> GetSelectListAsync([FromQuery] string? organizationId = null)
    {
        return _service.GetSelectListAsync(organizationId);
    }

    #endregion
}