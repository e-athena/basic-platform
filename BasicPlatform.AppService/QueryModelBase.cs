namespace BasicPlatform.AppService;

/// <summary>
/// Dto基类
/// </summary>
public class QueryModelBase
{
    /// <summary>
    /// ID
    /// </summary>
    // [TableColumn(Sort = 0, HideInTable = true, HideInSearch = true, HideInDescriptions = true)]
    [TableColumn(Ignore = true)]
    public string? Id { get; set; }

    /// <summary>
    /// 组织架构ID
    /// </summary>
    [TableColumn(Ignore = true)]
    public string? OrganizationalUnitId { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    [TableColumn(Sort = 85, HideInTable = true, Width = 90)]
    public string? CreatedUserId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [TableColumn(Sort = 87, HideInTable = true)]
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// 更新人
    /// </summary>
    [TableColumn(Sort = 88, HideInTable = true, Width = 90)]
    public string? LastUpdatedUserId { get; set; }

    /// <summary>
    /// 最后修改时间
    /// </summary>
    [TableColumn(Sort = 90, HideInTable = true)]
    public DateTime? UpdatedOn { get; set; }
}