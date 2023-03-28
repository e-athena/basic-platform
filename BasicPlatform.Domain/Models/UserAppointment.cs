namespace BasicPlatform.Domain.Models;

/// <summary>
/// 用户任职表
/// </summary>
[Table("authority_user_appointments")]
public class UserAppointment : ValueObject
{
    /// <summary>
    /// 组织架构ID
    /// </summary>
    [MaxLength(36)]
    public string OrganizationId { get; set; } = null!;

    /// <summary>
    /// 组织架构
    /// </summary>
    public virtual Organization Organization { get; set; } = null!;

    /// <summary>
    /// 所属职位ID
    /// </summary>
    [MaxLength(36)]
    public string PositionId { get; set; } = null!;

    /// <summary>
    /// 所属职位
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

    public UserAppointment()
    {
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="organizationId">组织/部门</param>
    /// <param name="positionId">职位</param>
    /// <param name="userId">员工</param>
    public UserAppointment(string organizationId, string positionId, string userId)
    {
        OrganizationId = organizationId;
        PositionId = positionId;
        UserId = userId;
    }
}