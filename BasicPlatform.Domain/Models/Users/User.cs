using BasicPlatform.Domain.Models.Users.Events;

namespace BasicPlatform.Domain.Models.Users;

/// <summary>
/// 用户
/// </summary>
[Table("authority_users")]
public class User : EntityCore, ICreator, IUpdater
{
    /// <summary>
    /// 用户名
    /// </summary>
    [Required]
    [MaxLength(32)]
    public string UserName { get; set; } = null!;

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; } = null!;

    /// <summary>
    /// 头像
    /// </summary>
    [MaxLength(-1)]
    public string? Avatar { get; set; }

    /// <summary>
    /// 真实姓名
    /// </summary>
    [MaxLength(16)]
    public string RealName { get; set; } = null!;

    /// <summary>
    /// 性别
    /// </summary>
    public Gender Gender { get; set; } = Gender.Unknown;

    /// <summary>
    /// 昵称
    /// </summary>
    [MaxLength(16)]
    public string? NickName { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [MaxLength(11)]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    /// <value></value>
    [MaxLength(256)]
    [EmailAddress]
    public string? Email { get; set; }

    /// <summary>
    /// 所属组织ID
    /// </summary>
    [MaxLength(36)]
    public string OrganizationId { get; set; } = null!;

    /// <summary>
    /// 所属组织
    /// </summary>
    public virtual Organization? Organization { get; set; }

    /// <summary>
    /// 所属职位ID
    /// </summary>
    [MaxLength(36)]
    public string? PositionId { get; set; }

    /// <summary>
    /// 所属职位
    /// </summary>
    public virtual Position? Position { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    /// <value></value>
    public Status Status { get; set; } = Status.Enabled;

    /// <summary>
    /// 创建人Id
    /// </summary>
    [MaxLength(36)]
    public string? CreatedUserId { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public virtual User? CreatedUser { get; set; }

    /// <summary>
    /// 最后更新人Id
    /// </summary>
    [MaxLength(36)]
    public string? LastUpdatedUserId { get; set; }

    /// <summary>
    /// 更新人
    /// </summary>
    public virtual User? LastUpdatedUser { get; set; }

    /// <summary>
    /// 最后登录时间
    /// </summary>
    public DateTime? LastLoginTime { get; set; }

    /// <summary>
    /// 最后登录Ip
    /// </summary>
    public string? LastLoginIp { get; set; }

    /// <summary>
    /// 最后登录地址
    /// </summary>
    public string? LastLoginAddress { get; set; }

    /// <summary>
    /// 登录次数
    /// </summary>
    public int LoginCount { get; set; }

    /// <summary>
    /// 是否初始密码
    /// </summary>
    public bool IsInitPassword { get; set; } = true;

    /// <summary>
    /// 是否为租户管理员
    /// </summary>
    public bool IsTenantAdmin { get; set; }

    public User()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <param name="password">密码</param>
    /// <param name="avatar">头像</param>
    /// <param name="realName">姓名</param>
    /// <param name="gender">性别</param>
    /// <param name="nickName">昵称</param>
    /// <param name="phoneNumber">手机呈</param>
    /// <param name="email">邮箱</param>
    /// <param name="organizationId">所属组织ID</param>
    /// <param name="positionId">所属职位ID</param>
    /// <param name="createdUserId">创建人</param>
    /// <param name="isTenantAdmin">是否为租户管理员</param>
    public User(string userName, string? password, string? avatar, string realName, Gender gender, string? nickName,
        string? phoneNumber, string? email, string organizationId, string? positionId, string? createdUserId,
        bool isTenantAdmin)
    {
        password ??= "123456";
        UserName = userName;
        Password = PasswordHash.CreateHash(password);
        Avatar = avatar;
        RealName = realName;
        Gender = gender;
        NickName = nickName;
        PhoneNumber = phoneNumber;
        Email = email;
        OrganizationId = organizationId;
        PositionId = positionId;
        CreatedUserId = createdUserId;
        IsTenantAdmin = isTenantAdmin;

        // 添加事件
        ApplyEvent(new UserCreatedEvent(
            userName,
            realName,
            gender,
            nickName,
            phoneNumber,
            email,
            isTenantAdmin,
            createdUserId
        ));
    }

    /// <summary>
    /// 状态变更
    /// </summary>
    /// <param name="updatedUserId"></param>
    public void StatusChange(string? updatedUserId)
    {
        Status = Status == Status.Disabled ? Status.Enabled : Status.Disabled;
        LastUpdatedUserId = updatedUserId;
        UpdatedOn = DateTime.Now;

        // 状态为启用时
        if (Status == Status.Enabled)
        {
            return;
        }

        // 不能禁用开发者帐号
        if (UserName == "root")
        {
            throw FriendlyException.Of("不能禁用开发者帐号");
        }
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="avatar"></param>
    /// <param name="realName"></param>
    /// <param name="nickName"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="email"></param>
    /// <param name="positionId"></param>
    /// <param name="updatedUserId"></param>
    /// <param name="gender"></param>
    /// <param name="organizationId"></param>
    public void Update(string userName, string? avatar, string realName, Gender gender,
        string? nickName,
        string? phoneNumber, string? email, string organizationId, string? positionId, string? updatedUserId)
    {
        UserName = userName;
        Avatar = avatar;
        RealName = realName;
        Gender = gender;
        NickName = nickName;
        PhoneNumber = phoneNumber;
        Email = email;
        OrganizationId = organizationId;
        PositionId = positionId ?? throw FriendlyException.Of("职位不能为空");
        LastUpdatedUserId = updatedUserId;
        UpdatedOn = DateTime.Now;

        // 添加领域事件
        ApplyEvent(new UserUpdatedEvent(Id, userName, avatar, realName, gender,
            nickName, phoneNumber, email, organizationId, positionId)
        );
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="realName"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="email"></param>
    /// <param name="updatedUserId"></param>
    public void UpdateBasic(string realName, string phoneNumber, string email, string? updatedUserId)
    {
        RealName = realName;
        PhoneNumber = phoneNumber;
        Email = email;
        LastUpdatedUserId = updatedUserId;
    }

    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="oldPassword"></param>
    /// <param name="newPassword"></param>
    /// <param name="updatedUserId"></param>
    public void ChangePassword(string oldPassword, string newPassword, string? updatedUserId)
    {
        if (!PasswordHash.ValidatePassword(oldPassword, Password))
        {
            throw FriendlyException.Of("当前密码输入有误");
        }

        Password = PasswordHash.CreateHash(newPassword);
        LastUpdatedUserId = updatedUserId;
    }

    /// <summary>
    /// 更新登录信息
    /// </summary>
    /// <param name="ip"></param>
    public void UpdateLoginInfo(string ip)
    {
        LastLoginTime = DateTime.Now;
        LastLoginIp = ip;
        LoginCount++;
        LastLoginAddress = NewLife.IP.Ip.GetAddress(ip);
    }

    /// <summary>
    /// 重置密码
    /// </summary>
    /// <param name="newPassword"></param>
    /// <param name="updatedUserId"></param>
    public void ResetPassword(string newPassword, string? updatedUserId)
    {
        if (UserName == "root")
        {
            throw FriendlyException.Of("不能重置开发者帐号的密码");
        }

        Password = PasswordHash.CreateHash(newPassword);
        IsInitPassword = true;
        LastUpdatedUserId = updatedUserId;
    }

    /// <summary>
    /// 分配数据权限
    /// </summary>
    /// <param name="permissions"></param>
    /// <param name="updatedUserId"></param>
    public void AssignDataPermissions(List<UserDataPermission> permissions, string? updatedUserId)
    {
        LastUpdatedUserId = updatedUserId;
        ApplyEvent(new UserDataPermissionAssignedEvent(permissions));
    }
}