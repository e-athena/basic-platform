namespace BasicPlatform.AppService.Tenants.Models;

/// <summary>
/// 租户模型
/// </summary>
public class TenantModel : ModelBase
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 唯一编码
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// 租户类型
    /// </summary>
    public TenantIsolationLevel IsolationLevel { get; set; }

    /// <summary>
    /// 联系人姓名
    /// </summary>
    public string ContactName { get; set; } = null!;

    /// <summary>
    /// 联系人手机号
    /// </summary>
    public string ContactPhoneNumber { get; set; } = null!;

    /// <summary>
    /// 联系人电子邮箱
    /// </summary>
    public string? ContactEmail { get; set; }

    /// <summary>
    /// 数据库链接字符串
    /// <remarks>主应用的</remarks>
    /// </summary>
    public string? ConnectionString { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remarks { get; set; }

    /// <summary>
    /// 订阅生效日期
    /// </summary>
    public DateTime EffectiveTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 订阅过期时间
    /// <remarks>为空时永久有效</remarks>
    /// </summary>
    public DateTime? ExpiredTime { get; set; }

    /// <summary>
    /// 子应用
    /// </summary>
    public IList<TenantApplicationModel> Applications { get; set; } = new List<TenantApplicationModel>();
}