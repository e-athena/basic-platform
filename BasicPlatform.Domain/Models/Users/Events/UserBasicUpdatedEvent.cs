namespace BasicPlatform.Domain.Models.Users.Events;

/// <summary>
/// 用户基本信息更新事件
/// </summary>
public class UserBasicUpdatedEvent : EventBase
{
    /// <summary>
    /// 真实姓名
    /// </summary>
    public string RealName { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="realName"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="email"></param>
    public UserBasicUpdatedEvent(string realName, string? phoneNumber, string? email)
    {
        RealName = realName;
        PhoneNumber = phoneNumber;
        Email = email;
    }
}