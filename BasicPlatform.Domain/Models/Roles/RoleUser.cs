using BasicPlatform.Domain.Models.Users;

namespace BasicPlatform.Domain.Models.Roles;

/// <summary>
/// 角色用户
/// </summary>
[Table("authority_role_users")]
public class RoleUser : ValueObject
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

    /// <summary>
    /// 
    /// </summary>
    public RoleUser()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="userId"></param>
    public RoleUser(string roleId, string userId)
    {
        RoleId = roleId;
        UserId = userId;
    }
}