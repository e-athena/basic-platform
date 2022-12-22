namespace BasicPlatform.Domain.Models;

/// <summary>
/// 职位用户
/// </summary>
[Table("AuthorityPositionUsers")]
public class PositionUser : ValueObject
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
    /// 用户ID
    /// </summary>
    [MaxLength(36)]
    public string UserId { get; set; } = null!;

    /// <summary>
    /// 用户
    /// </summary>
    public virtual User User { get; set; } = null!;

    public PositionUser()
    {
    }

    public PositionUser(string positionId, string userId)
    {
        PositionId = positionId;
        UserId = userId;
    }
}