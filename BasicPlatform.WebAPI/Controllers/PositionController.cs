using BasicPlatform.AppService.Positions;
using BasicPlatform.AppService.Positions.Requests;
using BasicPlatform.AppService.Positions.Responses;

namespace BasicPlatform.WebAPI.Controllers;

/// <summary>
/// 职位管理
/// </summary>
[Menu("职位管理",
    ModuleCode = "system",
    ModuleName = "系统模块",
    ModuleRoutePath = "/system",
    // RoutePath = "/system/position"
    RoutePath = "/welcome"
)]
public class PositionController : CustomControllerBase
{
    private readonly IPositionQueryService _service;
    private readonly IMediator _mediator;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mediator"></param>
    public PositionController(IPositionQueryService service, IMediator mediator)
    {
        _mediator = mediator;
        _service = service;
    }

    #region 基础接口

    /// <summary>
    /// 读取列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
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
    [ApiPermission("position:detail", DisplayName = "详情")]
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
    [ApiPermission(AdditionalRules = new[] {"position:detail"})]
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
    /// 读取树形列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<List<TreeViewModel>> GetTreeDataAsync()
    {
        return _service.GetTreeDataAsync();
    }

    /// <summary>
    /// 读取树形下拉列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission("position:treeSelect", IsVisible = false)]
    public Task<List<TreeSelectViewModel>> GetTreeSelectDataAsync()
    {
        return _service.GetTreeSelectDataAsync();
    }

    /// <summary>
    /// 读取树形选择框列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission("position:treeSelectForSelf", IsVisible = false)]
    public Task<List<TreeSelectViewModel>> GetTreeSelectDataForSelfAsync()
    {
        return _service.GetTreeSelectDataForSelfAsync();
    }

    /// <summary>
    /// 根据角色Id读取职位Id列表
    /// </summary>
    /// <param name="roleId">角色ID</param>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission("position:getIdsByRoleId", IsVisible = false)]
    public Task<List<string>> GetIdsByRoleIdAsync([FromQuery] string roleId)
    {
        return _service.GetIdsByRoleIdAsync(roleId);
    }

    /// <summary>
    /// 为职位分配角色
    /// </summary>
    /// <param name="request">请求主体</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiPermission(DisplayName = "分配角色")]
    public Task<string> AssignRolesAsync([FromBody] AssignRolesForPositionRequest request,
        CancellationToken cancellationToken)
    {
        return _mediator.SendAsync(request, cancellationToken);
    }

    #endregion
}