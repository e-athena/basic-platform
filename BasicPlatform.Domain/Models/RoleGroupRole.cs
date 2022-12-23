namespace BasicPlatform.Domain.Models;

/// <summary>
/// 角色组与角色关联
/// </summary>
[Table("authority_role_group_roles")]
public class RoleGroupRole : ValueObject
{
    /// <summary>
    /// 角色组ID
    /// </summary>
    /// <value></value>
    [MaxLength(36)]
    public string RoleGroupId { get; set; } = null!;

    /// <summary>
    /// 角色组
    /// </summary>
    /// <value></value>
    public virtual RoleGroup RoleGroup { get; set; } = null!;

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
}