using Athena.Infrastructure.Mvc.Attributes;
using Athena.Infrastructure.Permission.Helpers;

namespace BasicPlatform.WebAPI.Controllers;

/// <summary>
/// 工具控制器
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")]
public class UtilController : ControllerBase
{
    /// <summary>
    /// 读取权限资源
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [IgnoreApiResultFilter]
    public async Task<IActionResult> GetPermissionResourcesAsync()
    {
        var result = PermissionHelper.GetPermissionResources(Assembly.GetAssembly(typeof(UtilController))!);
        return await Task.FromResult(new JsonResult(result));
    }

    /// <summary>
    /// 读取权限资源
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [IgnoreApiResultFilter]
    public async Task<IActionResult> GetPermissionResourcesForJavaScriptAsync()
    {
        var result =
            PermissionHelper.GetPermissionResourcesForJavaScript(Assembly.GetAssembly(typeof(UtilController))!);
        return await Task.FromResult(new JsonResult(result));
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
}