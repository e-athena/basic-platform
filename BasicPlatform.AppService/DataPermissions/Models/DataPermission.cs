namespace BasicPlatform.AppService.DataPermissions.Models;

/// <summary>
/// 数据权限
/// </summary>
public class DataPermission
{
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
    public IList<string> DataScopeCustoms => DataScopeCustom?.Split(',').ToList() ?? new List<string>();

    /// <summary>
    /// 
    /// </summary>
    public DataPermission()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="displayName"></param>
    /// <param name="resourceKey"></param>
    public DataPermission(string displayName, string resourceKey)
    {
        DisplayName = displayName;
        ResourceKey = resourceKey;
    }
}