namespace BasicPlatform.Domain.Models.Tenants.Events;

/// <summary>
/// 租户数据库已初始化事件
/// </summary>
public class TenantDatabaseInitializedEvent : EventBase
{
    /// <summary>
    /// 管理员姓名
    /// </summary>
    public string AdminName { get; set; }

    /// <summary>
    /// 管理员手机号
    /// </summary>
    public string AdminPhoneNumber { get; set; }

    /// <summary>
    /// 管理员电子邮箱
    /// </summary>
    public string? AdminEmail { get; set; }

    /// <summary>
    /// 管理员帐号
    /// </summary>
    public string AdminUserName { get; set; }

    /// <summary>
    /// 管理员密码
    /// </summary>
    public string AdminPassword { get; set; }
    
    public TenantDatabaseInitializedEvent(string adminName, string adminPhoneNumber, string? adminEmail,
        string adminUserName, string adminPassword)
    {
        AdminName = adminName;
        AdminPhoneNumber = adminPhoneNumber;
        AdminEmail = adminEmail;
        AdminUserName = adminUserName;
        AdminPassword = adminPassword;
    }
}