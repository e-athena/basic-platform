using Athena.Infrastructure.Logger;
using Athena.Infrastructure.Logger.Messaging.Requests;
using Athena.Infrastructure.Logger.Messaging.Responses;

namespace BasicPlatform.WebAPI.Controllers.Developer;

/// <summary>
/// 日志管理
/// </summary>
[FrontEndRouting("日志查询",
    ModuleCode = "developer",
    ModuleName = "开发者中心",
    ModuleIcon = "ControlOutlined",
    ModuleRoutePath = "/developer",
    ModuleSort = 0,
    RoutePath = "/developer/log",
    Sort = 4,
    Description = "用于查询存储的日志记录。"
)]
public class LogController : CustomControllerBase
{
    private readonly ILoggerStorageService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public LogController(ILoggerStorageService service)
    {
        _service = service;
    }

    /// <summary>
    /// 读取列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiPermission(AdditionalRules = new[] {ApiPermissionConstant.LogServiceSelectList})]
    public Task<Paging<GetLogPagingResponse>> GetPagingAsync([FromBody] GetLogPagingRequest request)
    {
        return _service.GetPagingAsync(request);
    }

    /// <summary>读取日志详情</summary>
    /// <param name="request">ID</param>
    /// <returns></returns>
    [HttpGet]
    public Task<GetByIdResponse> GetByIdAsync([FromQuery] GetByIdRequest request)
    {
        return _service.GetByIdAsync(request);
    }

    /// <summary>读取日志详情</summary>
    /// <param name="request">ID</param>
    /// <returns></returns>
    [HttpGet]
    public Task<GetByTraceIdResponse> GetByTraceIdAsync(GetByTraceIdRequest request)
    {
        return _service.GetByTraceIdAsync(request);
    }

    /// <summary>读取调用次数</summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<long> GetCallCountAsync(GetCallCountRequest request)
    {
        return _service.GetCallCountAsync(request);
    }

    /// <summary>
    /// 读取服务列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission(ApiPermissionConstant.LogServiceSelectList)]
    public Task<List<SelectViewModel>> GetServiceSelectListAsync()
    {
        return _service.GetServiceSelectListAsync();
    }
}