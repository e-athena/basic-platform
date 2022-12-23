namespace BasicPlatform.Domain.Models;

/// <summary>
/// 职位角色
/// </summary>
[Table("authority_position_roles")]
public class PositionRole : ValueObject
{
    /// <summary>
    /// 职位ID
    /// </summary>
    [MaxLength(36)]
    public string PositionId { get; set; } = null!;

    /// <summary>
    /// 职位
    /// </summary>
    public virtual Position Position { get; set; } = null!;

    /// <summary>
    /// 角色ID
    /// </summary>
    [MaxLength(36)]
    public string RoleId { get; set; } = null!;

    /// <summary>
    /// 角色
    /// </summary>
    public virtual Role Role { get; set; } = null!;

    public PositionRole()
    {
    }

    public PositionRole(string positionId, string roleId)
    {
        PositionId = positionId;
        RoleId = roleId;
    }
}