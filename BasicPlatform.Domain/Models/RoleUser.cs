namespace BasicPlatform.Domain.Models;

/// <summary>
/// 角色用户
/// </summary>
[Table("AuthorityRoleUsers")]
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

    public RoleUser()
    {
    }

    public RoleUser(string roleId, string userId)
    {
        RoleId = roleId;
        UserId = userId;
    }
}