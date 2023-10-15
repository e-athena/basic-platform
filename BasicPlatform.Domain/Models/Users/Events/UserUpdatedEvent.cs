namespace BasicPlatform.Domain.Models.Users.Events;

/// <summary>
/// 用户更新成功事件
/// </summary>
public class UserUpdatedEvent : EventBase
{
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string? Avatar { get; set; }

    /// <summary>
    /// 真实姓名
    /// </summary>
    public string RealName { get; set; }

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
    /// Email
    /// </summary>
    /// <value></value>
    public string? Email { get; set; }

    /// <summary>
    /// 所属组织ID
    /// </summary>
    public string OrganizationId { get; set; }

    /// <summary>
    /// 所属职位ID
    /// </summary>
    public string? PositionId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="avatar"></param>
    /// <param name="realName"></param>
    /// <param name="gender"></param>
    /// <param name="nickName"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="email"></param>
    /// <param name="organizationId"></param>
    /// <param name="positionId"></param>
    public UserUpdatedEvent(string userName, string? avatar, string realName, Gender gender,
        string? nickName, string? phoneNumber, string? email, string organizationId, string? positionId)
    {
        UserName = userName;
        Avatar = avatar;
        RealName = realName;
        Gender = gender;
        NickName = nickName;
        PhoneNumber = phoneNumber;
        Email = email;
        OrganizationId = organizationId;
        PositionId = positionId;
    }
}