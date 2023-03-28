namespace BasicPlatform.Domain.Events.Users;

/// <summary>
/// 用户更新成功事件
/// </summary>
public class UserUpdatedEvent : DomainEventBase
{
    /// <summary>
    /// Id
    /// </summary>
    public string Id { get; set; } = null!;

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = null!;

    /// <summary>
    /// 头像
    /// </summary>
    public string? Avatar { get; set; }

    /// <summary>
    /// 真实姓名
    /// </summary>
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
    /// Email
    /// </summary>
    /// <value></value>
    public string? Email { get; set; }

    /// <summary>
    /// 所属组织ID
    /// </summary>
    public string OrganizationId { get; set; } = null!;

    /// <summary>
    /// 所属职位ID
    /// </summary>
    public string PositionId { get; set; } = null!;

    public UserUpdatedEvent(string id, string userName, string? avatar, string realName, Gender gender,
        string? nickName, string? phoneNumber, string? email, string organizationId, string positionId)
    {
        Id = id;
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