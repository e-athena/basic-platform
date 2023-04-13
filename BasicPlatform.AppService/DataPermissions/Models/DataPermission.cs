namespace BasicPlatform.AppService.DataPermissions.Models;

/// <summary>
/// 数据权限
/// </summary>
public class DataPermission
{
    /// <summary>
    /// 应用ID
    /// </summary>
    public string AppId { get; set; } = null!;

    /// <summary>
    /// 禁用选中
    /// </summary>
    public bool DisableChecked { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// 权限名称
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// 资源Key
    /// </summary>
    public string? ResourceKey { get; set; }

    /// <summary>
    /// 数据访问范围
    /// </summary>
    public RoleDataScope DataScope { get; set; } = RoleDataScope.All;

    /// <summary>
    /// 自定义数据访问范围
    /// </summary>
    public string? DataScopeCustom { get; set; }

    /// <summary>
    /// 自定义数据访问范围
    /// </summary>
    public IList<string> DataScopeCustoms => string.IsNullOrEmpty(DataScopeCustom)
        ? new List<string>()
        : DataScopeCustom.Split(',').ToList();

    /// <summary>
    /// 属性列表
    /// </summary>
    public List<DataPermissionProperty>? Properties { get; set; }

    /// <summary>
    /// 策略资源Key
    /// </summary>
    public string? PolicyResourceKey { get; set; }

    /// <summary>
    /// 默认的查询过滤器
    /// </summary>
    public List<QueryFilterGroup> QueryFilterGroups { get; set; } = new();

    /// <summary>
    /// 
    /// </summary>
    public DataPermission()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="appId"></param>
    /// <param name="displayName"></param>
    /// <param name="resourceKey"></param>
    public DataPermission(string appId, string displayName, string resourceKey)
    {
        AppId = appId;
        DisplayName = displayName;
        ResourceKey = resourceKey;
    }
}