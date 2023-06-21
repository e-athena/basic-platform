using BasicPlatform.Domain.Models.Tenants.Events;

namespace BasicPlatform.Domain.Models.Tenants;

/// <summary>
/// 租户
/// </summary>
[Table("tenants")]
public class Tenant : EntityCore, ICreator, IUpdater
{
    /// <summary>
    /// 名称
    /// </summary>
    [MaxLength(32)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 唯一编码
    /// </summary>
    [MaxLength(32)]
    public string Code { get; set; } = null!;

    /// <summary>
    /// 联系人姓名
    /// </summary>
    [MaxLength(32)]
    public string ContactName { get; set; } = null!;

    /// <summary>
    /// 联系人手机号
    /// </summary>
    [MaxLength(11)]
    public string ContactPhoneNumber { get; set; } = null!;

    /// <summary>
    /// 联系人电子邮箱
    /// </summary>
    [MaxLength(64)]
    public string? ContactEmail { get; set; }

    /// <summary>
    /// 数据库链接字符串
    /// <remarks>主应用的</remarks>
    /// </summary>
    public string ConnectionString { get; set; } = null!;

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
    public DateTime? ExpireTime { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public Status Status { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public string? CreatedUserId { get; set; }

    /// <summary>
    /// 更新人
    /// </summary>
    public string? LastUpdatedUserId { get; set; }

    public Tenant()
    {
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="code"></param>
    /// <param name="connectionString"></param>
    /// <param name="contactEmail"></param>
    /// <param name="remarks"></param>
    /// <param name="effectiveTime"></param>
    /// <param name="expireTime"></param>
    /// <param name="createdUserId"></param>
    /// <param name="applications"></param>
    /// <param name="contactName"></param>
    /// <param name="contactPhoneNumber"></param>
    public Tenant(string id, string name, string code, string contactName, string contactPhoneNumber,
        string? contactEmail, string connectionString, string? remarks, DateTime effectiveTime,
        DateTime? expireTime, string? createdUserId, List<TenantApplication> applications) : base(id)
    {
        Name = name;
        Code = code;
        ContactName = contactName;
        ContactPhoneNumber = contactPhoneNumber;
        ContactEmail = contactEmail;
        ConnectionString = SecurityHelper.Encrypt(connectionString);
        Remarks = remarks;
        EffectiveTime = effectiveTime;
        ExpireTime = expireTime;
        CreatedUserId = createdUserId;
        Status = Status.Enabled;

        ApplyEvent(new TenantCreatedEvent(name, code, contactName, contactPhoneNumber, contactEmail,
            connectionString, remarks, effectiveTime, expireTime, Status, createdUserId, applications));
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="name"></param>
    /// <param name="code"></param>
    /// <param name="contactEmail"></param>
    /// <param name="connectionString"></param>
    /// <param name="remarks"></param>
    /// <param name="effectiveTime"></param>
    /// <param name="expireTime"></param>
    /// <param name="lastUpdatedUserId"></param>
    /// <param name="contactName"></param>
    /// <param name="contactPhoneNumber"></param>
    /// <param name="applications"></param>
    public void Update(string name, string code, string contactName, string contactPhoneNumber,
        string? contactEmail, string connectionString, string? remarks, DateTime effectiveTime,
        DateTime? expireTime, string? lastUpdatedUserId, List<TenantApplication> applications)
    {
        Name = name;
        Code = code;
        ContactName = contactName;
        ContactPhoneNumber = contactPhoneNumber;
        ContactEmail = contactEmail;
        ConnectionString = SecurityHelper.Encrypt(connectionString);
        Remarks = remarks;
        EffectiveTime = effectiveTime;
        ExpireTime = expireTime;
        LastUpdatedUserId = lastUpdatedUserId;

        ApplyEvent(new TenantUpdatedEvent(name, code, contactName, contactPhoneNumber, contactEmail,
            connectionString, remarks, effectiveTime, expireTime, lastUpdatedUserId, applications));
    }

    /// <summary>
    /// 变更状态
    /// </summary>
    /// <param name="lastUpdatedUserId"></param>
    public void ChangeStatus(string? lastUpdatedUserId)
    {
        Status = Status == Status.Disabled ? Status.Enabled : Status.Disabled;
        LastUpdatedUserId = lastUpdatedUserId;
    }

    /// <summary>
    /// 分配资源
    /// </summary>
    /// <param name="resources"></param>
    /// <param name="lastUpdatedUserId"></param>
    public void AssignResources(List<TenantResource> resources, string? lastUpdatedUserId)
    {
        LastUpdatedUserId = lastUpdatedUserId;

        ApplyEvent(new TenantResourceAssignedEvent(resources));
    }
}