namespace BasicPlatform.Domain.Models.Users;

/// <summary>
/// 用户组与用户关联
/// </summary>
[Table("authority_user_group_users")]
public class UserGroupUser : ValueObject
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
    /// 用户ID
    /// </summary>
    /// <value></value>
    [MaxLength(36)]
    public string UserId { get; set; } = null!;

    /// <summary>
    /// 用户
    /// </summary>
    /// <value></value>
    public virtual User User { get; set; } = null!;
}