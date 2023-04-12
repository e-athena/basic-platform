using Athena.Infrastructure.Status;

namespace BasicPlatform.WebAPI.Controllers.System;

/// <summary>
/// 服务器信息
/// </summary>
// [Menu("服务器信息",
//     ModuleCode = "system",
//     ModuleName = "系统模块",
//     ModuleRoutePath = "/system",
//     ModuleSort = 1,
//     RoutePath = "/system/server-info",
//     Sort = 5
// )]
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