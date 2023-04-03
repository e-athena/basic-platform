namespace BasicPlatform.AppService.Organizations.Models;

/// <summary>
/// 组织架构Dto
/// </summary>
public class OrganizationQueryModel : QueryModelBase
{
    /// <summary>
    /// 父级Id
    /// </summary>
    [TableColumn(HideInTable = true)]
    public string? ParentId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [TableColumn(Sort = 0, Width = 150, Fixed = "left")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 部门负责人Id
    /// </summary>
    [MaxLength(36)]
    [TableColumn(HideInTable = true)]
    public string? LeaderId { get; set; }

    /// <summary>
    /// 路径
    /// </summary>
    [TableColumn(HideInTable = true)]
    public string ParentPath { get; set; } = string.Empty;

    /// <summary>
    /// 备注
    /// </summary>
    [TableColumn(Sort = 2)]
    public string? Remarks { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [TableColumn(Sort = 4)]
    public Status Status { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [TableColumn(Sort = 3, Width = 70)]
    public int Sort { get; set; }
}