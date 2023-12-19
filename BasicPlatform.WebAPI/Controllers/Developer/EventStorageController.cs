using Athena.Infrastructure.EventStorage;
using Athena.Infrastructure.EventStorage.Messaging.Requests;
using Athena.Infrastructure.EventStorage.Messaging.Responses;

namespace BasicPlatform.WebAPI.Controllers.Developer;

/// <summary>
/// 事件存储管理
/// </summary>
[FrontEndRouting("事件存储",
    ModuleCode = "developer",
    ModuleName = "开发者中心",
    ModuleIcon = "ControlOutlined",
    ModuleRoutePath = "/developer",
    ModuleSort = 0,
    RoutePath = "/developer/event-storage",
    Sort = 4,
    Description = "用于查询存储的事件记录。"
)]
public class EventStorageController : CustomControllerBase
{
    private readonly IEventStreamQueryService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public EventStorageController(IEventStreamQueryService service)
    {
        _service = service;
    }

    /// <summary>
    /// 读取列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<Paging<GetEventStreamPagingResponse>> GetPagingAsync([FromBody] GetEventStreamPagingRequest request)
    {
        return _service.GetPagingAsync(request);
    }

    /// <summary>
    /// 读取内容
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<string> GetAsync([FromQuery] long id)
    {
        return _service.GetEventPayloadAsync(id);
    }
}