using Athena.Infrastructure.Status;

namespace BasicPlatform.WebAPI.Controllers.Systems;

/// <summary>
/// 服务器信息
/// </summary>
[Menu("服务器信息",
    ModuleCode = "system",
    ModuleName = "系统模块",
    ModuleRoutePath = "/system",
    ModuleSort = 3,
    
    RoutePath = "/system/server-info",
    Sort = 6
)]
public class ServerController : CustomControllerBase
{
    /// <summary>
    /// 读取服务器信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public StatusModel Get([FromServices] IConfiguration configuration)
    {
        return configuration.BuildAppStatusModel();
    }
}