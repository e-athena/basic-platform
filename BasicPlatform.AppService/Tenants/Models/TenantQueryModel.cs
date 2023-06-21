namespace BasicPlatform.AppService.Tenants.Models;

/// <summary>
/// 租户查询模型
/// </summary>
public class TenantQueryModel : QueryModelBase
{
    /// <summary>
    /// 名称
    /// </summary>
    [TableColumn(Sort = 0)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 编码
    /// </summary>
    [TableColumn(Sort = 5, Width = 100)]
    public string Code { get; set; } = null!;

    /// <summary>
    /// 联系人姓名
    /// </summary>
    [TableColumn(Sort = 6, Width = 100)]
    public string ContactName { get; set; } = null!;

    /// <summary>
    /// 联系人手机号
    /// </summary>
    [TableColumn(Sort = 7, Width = 100)]
    public string ContactPhoneNumber { get; set; } = null!;

    /// <summary>
    /// 联系人电子邮箱
    /// </summary>
    [TableColumn(Sort = 8, Width = 100)]
    public string? ContactEmail { get; set; } = null!;

    /// <summary>
    /// 连接字符串
    /// </summary>
    [TableColumn(Sort = 10, Ignore = true)]
    public virtual string ConnectionString { get; set; } = null!;

    /// <summary>
    /// 生效日期
    /// </summary>
    [TableColumn(Tooltip = "订阅生效日期", Sort = 15)]
    public DateTime EffectiveTime { get; set; }

    /// <summary>
    /// 过期时间
    /// </summary>
    [TableColumn(Tooltip = "订阅过期时间，为空时永久有效。", Sort = 20)]
    public DateTime? ExpireTime { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [TableColumn(Sort = 25)]
    public string? Remarks { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [TableColumn(Sort = 30, Width = 110)]
    public Status Status { get; set; }
}