namespace BasicPlatform.Domain.Models.Tenants.Events;

/// <summary>
/// 租户创建成功事件
/// </summary>
public class TenantUpdatedEvent : EventBase
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 唯一编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 联系人姓名
    /// </summary>
    public string ContactName { get; set; }

    /// <summary>
    /// 联系人手机号
    /// </summary>
    public string ContactPhoneNumber { get; set; }

    /// <summary>
    /// 联系人电子邮箱
    /// </summary>
    public string? ContactEmail { get; set; }

    /// <summary>
    /// 数据库链接字符串
    /// <remarks>主应用的</remarks>
    /// </summary>
    public string ConnectionString { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remarks { get; set; }

    /// <summary>
    /// 订阅生效日期
    /// </summary>
    public DateTime EffectiveTime { get; set; }

    /// <summary>
    /// 订阅过期时间
    /// <remarks>为空时永久有效</remarks>
    /// </summary>
    public DateTime? ExpiredTime { get; set; }

    /// <summary>
    /// 更新人
    /// </summary>
    public string? LastUpdatedUserId { get; set; }

    /// <summary>
    /// 租户应用
    /// </summary>
    public List<TenantApplication> Applications { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="code"></param>
    /// <param name="contactName"></param>
    /// <param name="contactPhoneNumber"></param>
    /// <param name="contactEmail"></param>
    /// <param name="connectionString"></param>
    /// <param name="remarks"></param>
    /// <param name="effectiveTime"></param>
    /// <param name="expiredTime"></param>
    /// <param name="lastUpdatedUserId"></param>
    /// <param name="applications"></param>
    public TenantUpdatedEvent(string name, string code, string contactName, string contactPhoneNumber,
        string? contactEmail, string connectionString, string? remarks, DateTime effectiveTime, DateTime? expiredTime,
        string? lastUpdatedUserId, List<TenantApplication> applications)
    {
        Name = name;
        Code = code;
        ContactName = contactName;
        ContactPhoneNumber = contactPhoneNumber;
        ContactEmail = contactEmail;
        ConnectionString = connectionString;
        Remarks = remarks;
        EffectiveTime = effectiveTime;
        ExpiredTime = expiredTime;
        LastUpdatedUserId = lastUpdatedUserId;
        Applications = applications;
    }
}