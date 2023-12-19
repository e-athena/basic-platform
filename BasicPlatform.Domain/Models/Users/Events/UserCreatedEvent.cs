namespace BasicPlatform.Domain.Models.Users.Events;

/// <summary>
/// 用户创建成功
/// </summary>
public class UserCreatedEvent : EventBase
{
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 真实姓名
    /// </summary>
    [MaxLength(16)]
    public string RealName { get; set; } = null!;

    /// <summary>
    /// 性别
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string? NickName { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// 电子邮箱
    /// </summary>
    /// <value></value>
    public string? Email { get; set; }

    /// <summary>
    /// 是否为租户管理员
    /// </summary>
    public bool IsTenantAdmin { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public string? CreatedUserId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="realName"></param>
    /// <param name="gender"></param>
    /// <param name="nickName"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="email"></param>
    /// <param name="isTenantAdmin"></param>
    /// <param name="createdUserId"></param>
    public UserCreatedEvent(string userName, string realName, Gender gender, string? nickName,
        string? phoneNumber, string? email, bool isTenantAdmin, string? createdUserId)
    {
        UserName = userName;
        RealName = realName;
        Gender = gender;
        NickName = nickName;
        PhoneNumber = phoneNumber;
        Email = email;
        IsTenantAdmin = isTenantAdmin;
        CreatedUserId = createdUserId;
    }
}