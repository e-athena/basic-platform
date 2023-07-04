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
    /// 是否已初始化数据库
    /// </summary>
    public bool IsInitDatabase { get; set; }

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
    /// <param name="expiredTime"></param>
    /// <param name="createdUserId"></param>
    /// <param name="applications"></param>
    /// <param name="contactName"></param>
    /// <param name="contactPhoneNumber"></param>
    public Tenant(string id, string name, string code, string contactName, string contactPhoneNumber,
        string? contactEmail, string connectionString, string? remarks, DateTime effectiveTime,
        DateTime? expiredTime, string? createdUserId, List<TenantApplication> applications) : base(id)
    {
        Name = name;
        Code = code;
        ContactName = contactName;
        ContactPhoneNumber = contactPhoneNumber;
        ContactEmail = contactEmail;
        ConnectionString = SecurityHelper.Encrypt(connectionString);
        Remarks = remarks;
        EffectiveTime = effectiveTime;
        ExpiredTime = expiredTime;
        CreatedUserId = createdUserId;
        Status = Status.Enabled;

        ApplyEvent(new TenantCreatedEvent(name, code, contactName, contactPhoneNumber, contactEmail,
            connectionString, remarks, effectiveTime, expiredTime, Status, createdUserId, applications));
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
    /// <param name="expiredTime"></param>
    /// <param name="lastUpdatedUserId"></param>
    /// <param name="contactName"></param>
    /// <param name="contactPhoneNumber"></param>
    /// <param name="applications"></param>
    public void Update(string name, string code, string contactName, string contactPhoneNumber,
        string? contactEmail, string connectionString, string? remarks, DateTime effectiveTime,
        DateTime? expiredTime, string? lastUpdatedUserId, List<TenantApplication> applications)
    {
        Name = name;
        Code = code;
        ContactName = contactName;
        ContactPhoneNumber = contactPhoneNumber;
        ContactEmail = contactEmail;
        var connectString = SecurityHelper.Encrypt(connectionString);
        // 如果数据库链接字符串有变更，则重置数据库初始化状态
        if (ConnectionString != connectString)
        {
            IsInitDatabase = false;
        }

        ConnectionString = connectString;
        Remarks = remarks;
        EffectiveTime = effectiveTime;
        ExpiredTime = expiredTime;
        LastUpdatedUserId = lastUpdatedUserId;

        ApplyEvent(new TenantUpdatedEvent(name, code, contactName, contactPhoneNumber, contactEmail,
            connectionString, remarks, effectiveTime, expiredTime, lastUpdatedUserId, applications));
    }

    /// <summary>
    /// 变更状态
    /// </summary>
    /// <param name="lastUpdatedUserId"></param>
    public void ChangeStatus(string? lastUpdatedUserId)
    {
        Status = Status == Status.Disabled ? Status.Enabled : Status.Disabled;
        LastUpdatedUserId = lastUpdatedUserId;

        ApplyEvent(new TenantStatusChangedEvent(Status));
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

    /// <summary>
    /// 初始化
    /// </summary>
    public void InitDatabase(string? lastUpdatedUserId)
    {
        IsInitDatabase = true;
        LastUpdatedUserId = lastUpdatedUserId;

        ApplyEvent(new TenantDatabaseInitializedEvent(lastUpdatedUserId));
    }
}