using BasicPlatform.Domain.Models.Roles;

namespace BasicPlatform.Domain.Models.Users;

/// <summary>
/// 用户组与角色关联
/// </summary>
[Table("authority_user_group_roles")]
public class UserGroupRole : ValueObject
{
    /// <summary>
    /// 用户组ID
    /// </summary>
    /// <value></value>
    [MaxLength(36)]
    public string UserGroupId { get; set; } = null!;

    /// <summary>
    /// 用户组
    /// </summary>
    /// <value></value>
    public virtual UserGroup UserGroup { get; set; } = null!;

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