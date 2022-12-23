namespace BasicPlatform.Domain.Models;

/// <summary>
/// 角色资源
/// </summary>
[Table("authority_role_resources")]
public class RoleResource : ValueObject
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
    /// 资源ID
    /// </summary>
    /// <value></value>
    [MaxLength(36)]
    public string ResourceId { get; set; } = null!;

    /// <summary>
    /// 资源
    /// </summary>
    /// <value></value>
    public virtual Resource Resource { get; set; } = null!;

    public RoleResource()
    {
    }

    public RoleResource(string roleId, string resourceId)
    {
        RoleId = roleId ?? throw new ArgumentNullException(nameof(roleId));
        ResourceId = resourceId ?? throw new ArgumentNullException(nameof(resourceId));
    }
}