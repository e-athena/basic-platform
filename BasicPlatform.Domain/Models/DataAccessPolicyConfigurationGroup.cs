namespace BasicPlatform.Domain.Models;

/// <summary>
/// 数据访问策略配置组
/// </summary>
[Table("authority_data_access_policy_configuration_groups")]
public class DataAccessPolicyConfigurationGroup : EntityCore, ICreator, IUpdater
{
    /// <summary>
    /// 名称
    /// </summary>
    /// <value></value>
    [Required]
    [MaxLength(12)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remarks { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
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
    public string? UpdatedUserId { get; set; }

    /// <summary>
    /// 最后更新人
    /// </summary>
    public virtual User? UpdatedUser { get; set; }
}