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
    /// 读取程序集信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult GetAssemblyInfo()
    {
        // 读取所有程序集
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        // 过滤掉动态程序集
        assemblies = assemblies.Where(x => !x.IsDynamic).ToArray();
        // 只读取直接引用的程序集
        assemblies = assemblies.Where(x => x.Location.Contains(AppDomain.CurrentDomain.BaseDirectory)).ToArray();
        // 按名称排序
        assemblies = assemblies.OrderBy(x => x.FullName).ToArray();
        // 过滤掉重复的
        assemblies = assemblies.Distinct().ToArray();
        // 循环读取程序集信息
        var list = new List<object>();
        foreach (var assembly in assemblies)
        {
            var version = assembly.GetName().Version;
            var fileVersion = FileVersionInfo.GetVersionInfo(assembly.Location);
            var info = new
            {
                assembly.FullName,
                version,
                fileVersion.FileVersion,
                fileVersion.ProductVersion,
                fileVersion.ProductName,
                fileVersion.CompanyName,
                fileVersion.FileDescription,
                fileVersion.Comments,
                fileVersion.OriginalFilename,
                fileVersion.Language,
            };
            list.Add(info);
        }

        return Ok(list);
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