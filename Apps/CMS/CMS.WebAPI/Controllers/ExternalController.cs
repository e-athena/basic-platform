namespace CMS.WebAPI.Controllers;

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
    public ApplicationDataPermissionInfo GetDataPermissionResources(
        [FromServices] IEnumerable<IDataPermission> services
    )
    {
        var dataPermissionFactory = new DataPermissionFactory(services);
        var assembly = Assembly.Load("CMS.QueryServices");
        var groupList = DataPermissionHelper.GetGroupList(assembly, GlobalConstant.DefaultAppId);
        return new ApplicationDataPermissionInfo
        {
            ApplicationId = GlobalConstant.DefaultAppId,
            ApplicationName = "CMS",
            DataPermissionGroups = groupList,
            ExtraSelectList = dataPermissionFactory.GetSelectList()
        };
    }

    /// <summary>
    /// 同步数据库结构
    /// </summary>
    /// <param name="freeSql"></param>
    [HttpGet("sync-database")]
    public Task<string> SyncDatabaseAsync([FromServices] IFreeSql freeSql)
    {
        freeSql.SyncStructure("CMS.Domain");
        return Task.FromResult("ok");
    }
}