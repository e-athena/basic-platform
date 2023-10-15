using Athena.Infrastructure.EventTracking;
using Athena.Infrastructure.EventTracking.Messaging.Models;
using Athena.Infrastructure.EventTracking.Messaging.Requests;
using Athena.Infrastructure.EventTracking.Messaging.Responses;

namespace BasicPlatform.WebAPI.Controllers.Developer;

/// <summary>
/// 事件追踪管理
/// </summary>
[FrontEndRouting("事件追踪",
    ModuleCode = "developer",
    ModuleName = "开发者中心",
    ModuleIcon = "ControlOutlined",
    ModuleRoutePath = "/developer",
    ModuleSort = 0,
    RoutePath = "/developer/event-tracking",
    Sort = 3,
    Description = "用于监控事件链路执行情况。"
)]
public class EventTrackingController : CustomControllerBase
{
    private readonly ITrackStorageService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public EventTrackingController(ITrackStorageService service)
    {
        _service = service;
    }

    /// <summary>
    /// 读取列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<Paging<GetTrackPagingResponse>> GetPagingAsync([FromBody] GetTrackPagingRequest request)
    {
        return _service.GetPagingAsync(request);
    }
    /// <summary>
    /// 根据ID读取
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<GetTrackInfoResponse?> GetAsync([FromQuery] string id)
    {
        return _service.GetAsync(id);
    }
    /// <summary>
    /// 根据ID读取
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<DecompositionTreeGraphModel?> GetDecompositionTreeGraphAsync([FromQuery] string id)
    {
        return _service.GetDecompositionTreeGraphAsync(id);
    }
}