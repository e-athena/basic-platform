namespace BasicPlatform.WebAPI.Services;

/// <summary>
/// 子应用服务
/// </summary>
public interface ISubApplicationService
{
    /// <summary>
    /// 读取菜单资源
    /// </summary>
    /// <param name="resources"></param>
    /// <returns></returns>
    Task<List<ApplicationResourceInfo>> GetMenuResourcesAsync(IList<ResourceModel>? resources);

    /// <summary>
    /// 读取数据权限资源
    /// </summary>
    /// <returns></returns>
    Task<List<ApplicationDataPermissionInfo>> GetDataPermissionResourcesAsync();

    /// <summary>
    /// 同步数据库
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <returns></returns>
    Task SyncDatabaseAsync(string tenantCode);
}