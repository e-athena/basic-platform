using Athena.Infrastructure.DataPermission;
using Athena.Infrastructure.DataPermission.Models;
using SqlSugar;

namespace CRM.WebAPI.Controllers;

/// <summary>
/// 外接接口控制器
/// <remarks>用于提供接口给主应用调用</remarks>
/// </summary>
[ApiController]
[Route("api/external")]
[BasicAuthFilter]
public class ExternalController : ControllerBase
{
    /// <summary>
    /// 读取菜单功能资源
    /// </summary>
    /// <returns></returns>
    [HttpGet("get-menu-resources")]
    public IList<MenuTreeInfo> GetMenuResources([FromServices] IApiPermissionService service)
    {
        var assembly = Assembly.GetExecutingAssembly();
        return service.GetFrontEndRoutingResources(assembly, GlobalConstant.DefaultAppId);
    }

    /// <summary>
    /// 读取数据权限资源
    /// </summary>
    /// <returns></returns>
    [HttpGet("get-data-permission-resources")]
    public IList<DataPermissionGroup> GetDataPermissionResources()
    {
        var assembly = Assembly.Load("CRM.QueryServices");
        return DataPermissionHelper.GetGroupList(assembly, GlobalConstant.DefaultAppId);
    }

    /// <summary>
    /// 同步数据库结构
    /// </summary>
    /// <param name="dbContext"></param>
    [HttpGet("sync-database")]
    public Task<string> SyncDatabaseAsync([FromServices] ISqlSugarClient dbContext)
    {
        dbContext.SyncStructure("CRM.Domain");
        return Task.FromResult("ok");
    }
}