using BasicPlatform.AppService.Users.Models;

namespace BasicPlatform.AppService.Users.Requests;

/// <summary>
/// 添加用户任职
/// </summary>
public class AddUserAppointmentRequest : ITxRequest<string>
{
    /// <summary>
    /// ID
    /// </summary>
    [MaxLength(36)]
    public string Id { get; set; } = null!;

    /// <summary>
    /// 任职列表
    /// </summary>
    public List<UserAppointmentModel> Appointments { get; set; } = new();
}