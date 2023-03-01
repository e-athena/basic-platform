namespace BasicPlatform.WebAPI.Controllers;

/// <summary>
/// 工具控制器
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")]
public class UtilController : ControllerBase
{
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