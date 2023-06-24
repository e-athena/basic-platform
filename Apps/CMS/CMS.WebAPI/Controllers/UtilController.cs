namespace CMS.WebAPI.Controllers;

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

    /// <summary>
    /// 读取权限资源
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<ResourceTreeInfo>> GetResourcesAsync()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var result = ApiPermissionHelper.GetResources(assembly);
        return await Task.FromResult(result);
    }

    /// <summary>
    /// 读取权限资源
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [IgnoreApiResultFilter]
    public async Task<string> GetResourcesForJavaScriptAsync()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var result = ApiPermissionHelper.GetResourcesForJavaScript(assembly);
        return await Task.FromResult(result);
    }
}