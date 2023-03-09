namespace BasicPlatform.AppService.Positions.Models;

/// <summary>
/// 职位模型
/// </summary>
public class PositionModel
{
    /// <summary>
    /// ID
    /// </summary>
    public string Id { get; set; } = null!;

    /// <summary>
    /// 组织架构ID
    /// </summary>
    [MaxLength(36)]
    [Required]
    public string OrganizationId { get; set; } = null!;

    /// <summary>
    /// 名称
    /// </summary>
    [Required]
    [MaxLength(32)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(1024)]
    public string? Remarks { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public Status Status { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}