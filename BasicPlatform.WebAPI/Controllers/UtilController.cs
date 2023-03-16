using System.Diagnostics;
using System.Runtime.InteropServices;
using Athena.Infrastructure.Status;
using BasicPlatform.AppService;
using BasicPlatform.AppService.Users.Responses;
using BasicPlatform.Infrastructure.Tables;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace BasicPlatform.WebAPI.Controllers;

/// <summary>
/// 工具控制器
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")]
public class UtilController : ControllerBase
{
    private readonly ILogger<UtilController> _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="loggerFactory"></param>
    public UtilController(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<UtilController>();
    }

    /// <summary>
    /// 同步数据库结构
    /// </summary>
    /// <param name="freeSql"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult SyncStructure([FromServices] IFreeSql freeSql)
    {
        freeSql.SyncStructure("BasicPlatform.Domain");
        return Ok("ok");
    }

    /// <summary>
    /// 释放内存
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult MemoryFree()
    {
        try
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "释放内存失败");
        }

        return Ok("ok");
    }
}