namespace BasicPlatform.Domain.Models;

/// <summary>
/// 角色数据权限
/// </summary>
[Table("authority_role_data_permissions")]
public class RoleDataPermission : ValueObject
{
    /// <summary>
    /// 角色ID
    /// </summary>
    /// <value></value>
    [MaxLength(36)]
    public string RoleId { get; set; } = null!;

    /// <summary>
    /// 角色
    /// </summary>
    /// <value></value>
    public virtual Role Role { get; set; } = null!;

    /// <summary>
    /// 资源Key
    /// </summary>
    public string ResourceKey { get; set; } = null!;

    /// <summary>
    /// 数据访问范围
    /// </summary>
    public RoleDataScope DataScope { get; set; }

    /// <summary>
    /// 自定义数据访问范围(组织Id)
    /// <remarks>多个组织使用逗号分割</remarks>
    /// </summary>
    [MaxLength(-1)]
    public string? DataScopeCustom { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }

    public RoleDataPermission()
    {
    }

    public RoleDataPermission(string roleId, string resourceKey, RoleDataScope dataScope, string? dataScopeCustom,
        bool enabled)
    {
        RoleId = roleId;
        ResourceKey = resourceKey;
        DataScope = dataScope;
        DataScopeCustom = dataScopeCustom;
        Enabled = enabled;
    }
}