namespace BasicPlatform.AppService;

/// <summary>
/// Dto基类
/// </summary>
public class QueryModelBase
{
    /// <summary>
    /// ID
    /// </summary>
    [TableColumn(Sort = 0, HideInTable = true, HideInSearch = true)]
    public string? Id { get; set; }

    /// <summary>
    /// 组织架构ID
    /// </summary>
    [TableColumn(Ignore = true)]
    public string? OrganizationalUnitIds { get; set; }

    /// <summary>
    /// 创建人Id
    /// </summary>
    [TableColumn(Sort = 85, HideInTable = true)]
    public string? CreatedUserId { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    [TableColumn(Sort = 86, HideInTable = true, Width = 90)]
    public string? CreatedUserName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [TableColumn(Sort = 87, HideInTable = true)]
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// 更新人Id  
    /// </summary>
    [TableColumn(Sort = 88, HideInTable = true)]
    public string? UpdatedUserId { get; set; }

    /// <summary>
    /// 更新人
    /// </summary>
    [TableColumn(Sort = 89, HideInTable = true, Width = 90)]
    public string? UpdatedUserName { get; set; }

    /// <summary>
    /// 最后修改时间
    /// </summary>
    [TableColumn(Sort = 90, HideInTable = true)]
    public DateTime? UpdatedOn { get; set; }
}