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
    /// 资源代码
    /// </summary>
    /// <value></value>
    [MaxLength(256)]
    public string ResourceCode { get; set; } = null!;
    
    public RoleResource()
    {
    }

    public RoleResource(string roleId, string resourceCode)
    {
        RoleId = roleId ?? throw new ArgumentNullException(nameof(roleId));
        ResourceCode = resourceCode ?? throw new ArgumentNullException(nameof(resourceCode));
    }
}