namespace BasicPlatform.AppService.Positions.Models;

/// <summary>
/// 职位模型
/// </summary>
public class PositionQueryModel : QueryModelBase
{
    /// <summary>
    /// 组织架构ID
    /// </summary>
    [MaxLength(36)]
    [Required]
    [TableColumn(HideInTable = true)]
    public string OrganizationId { get; set; } = null!;

    /// <summary>
    /// 名称
    /// </summary>
    [Required]
    [MaxLength(32)]
    [TableColumn(Sort = 1, Width = 150)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(1024)]
    [TableColumn(Sort = 2)]
    public string? Remarks { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [TableColumn(Sort = 4, Width = 90)]
    public Status Status { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [TableColumn(Sort = 3, Width = 90, Align = "center")]
    public int Sort { get; set; }
}