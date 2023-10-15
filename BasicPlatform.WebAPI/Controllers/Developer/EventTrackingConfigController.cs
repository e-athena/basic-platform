using Athena.Infrastructure.EventTracking;
using Athena.Infrastructure.EventTracking.Helpers;
using Athena.Infrastructure.EventTracking.Messaging.Requests;
using Athena.Infrastructure.EventTracking.Messaging.Responses;
using Athena.Infrastructure.Messaging.Requests;

namespace BasicPlatform.WebAPI.Controllers.Developer;

/// <summary>
/// 事件追踪配置管理
/// </summary>
[FrontEndRouting("事件追踪配置",
    ModuleCode = "developer",
    ModuleName = "开发者中心",
    ModuleIcon = "ControlOutlined",
    ModuleRoutePath = "/developer",
    ModuleSort = 0,
    RoutePath = "/developer/event-tracking/config",
    Sort = 4,
    Description = "用于配置事件订阅关系图。",
    IsVisible = false
)]
public class EventTrackingConfigController : CustomControllerBase
{
    private readonly ITrackConfigService _service;
    private const string ConfigSelectList = "event-tracking-select-list";
    private const string EventSelectList = "event-tracking-event-select-list";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public EventTrackingConfigController(ITrackConfigService service)
    {
        _service = service;
    }

    /// <summary>
    /// 读取列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<Paging<GetTrackConfigPagingResponse>> GetPagingAsync([FromBody] GetTrackConfigPagingRequest request)
    {
        return _service.GetPagingAsync(request);
    }

    /// <summary>
    /// 根据ID读取
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<GetTrackConfigInfoResponse?> GetAsync([FromQuery] string id)
    {
        return _service.GetAsync(id);
    }

    /// <summary>
    /// 根据追踪特性列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission(ConfigSelectList)]
    public IList<EventTrackingInfo> GetSelectList()
    {
        return EventTrackingHelper.GetEventTrackingInfos(new List<Assembly>
        {
            Assembly.Load("BasicPlatform.AppService.FreeSql"),
            Assembly.Load("BasicPlatform.ProcessManager")
        });
    }

    /// <summary>
    /// 根据事件下拉列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission(EventSelectList)]
    public IList<SelectViewModel> GetEventSelectList()
    {
        return EventTrackingHelper.GetEventSelectList(new List<Assembly>
        {
            Assembly.Load("BasicPlatform.Domain")
        });
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiPermission(AdditionalRules = new[]
    {
        ConfigSelectList,
        EventSelectList
    })]
    public Task SaveAsync([FromBody] SaveTrackConfigRequest request, CancellationToken cancellationToken)
    {
        return _service.SaveAsync(request, cancellationToken);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete]
    public Task DeleteAsync([FromBody] IdRequest request, CancellationToken cancellationToken)
    {
        return _service.DeleteAsync(request.Id, cancellationToken);
    }
}