namespace BasicPlatform.AppService.Users.Models;

/// <summary>
/// 添加用户任职
/// </summary>
public class UserAppointmentModel
{
    /// <summary>
    /// 组织架构ID
    /// </summary>
    [MaxLength(36)]
    public string OrganizationId { get; set; } = null!;

    /// <summary>
    /// 所属职位ID
    /// </summary>
    [MaxLength(36)]
    public string PositionId { get; set; } = null!;

}