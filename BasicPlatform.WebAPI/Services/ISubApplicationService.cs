namespace BasicPlatform.WebAPI.Services;

/// <summary>
/// 子应用服务
/// </summary>
public interface ISubApplicationService
{
    /// <summary>
    /// 读取事件列表
    /// </summary>
    /// <returns></returns>
    Task<IDictionary<string, List<SelectViewModel>>> GetEventsAsync();

    /// <summary>
    /// 读取事件追踪配置列表
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    Task<IDictionary<string, List<EventTrackingInfo>>> GetEventTrackingListAsync();

    /// <summary>
    /// 读取菜单资源
    /// </summary>
    /// <param name="resourceUrl"></param>
    /// <returns></returns>
    Task<List<MenuTreeInfo>> GetMenuResourcesAsync(string resourceUrl);

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