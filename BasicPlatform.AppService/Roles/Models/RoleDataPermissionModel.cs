namespace BasicPlatform.AppService.Roles.Models;

/// <summary>
/// 数据数据权限模型
/// </summary>
public class RoleDataPermissionModel
{
    /// <summary>
    /// 资源Key
    /// </summary>
    public string ResourceKey { get; set; } = null!;

    /// <summary>
    /// 数据访问范围
    /// </summary>
    public RoleDataScope DataScope { get; set; }

    /// <summary>
    /// 自定义数据访问范围
    /// </summary>
    public string? DataScopeCustom { get; set; }

    /// <summary>
    /// 自定义数据访问范围
    /// </summary>
    public IList<string> DataScopeCustoms => DataScopeCustom?.Split(',').ToList() ?? new List<string>();

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }
}