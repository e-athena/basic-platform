namespace BasicPlatform.Domain.Models;

/// <summary>
/// 数据访问策略配置
/// </summary>
[Table("AuthorityDataAccessPolicyConfigurations")]
public class DataAccessPolicyConfiguration : EntityCore, ICreator, IUpdater
{
    /// <summary>
    /// 配置组ID
    /// </summary>
    public string GroupId { get; set; } = null!;

    /// <summary>
    /// 配置组
    /// </summary>
    public virtual DataAccessPolicyConfigurationGroup? Group { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 规则
    /// </summary>
    [MaxLength(-1)]
    public string Ruler { get; set; } = null!;

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