namespace BasicPlatform.AppService;

/// <summary>
/// Dto基类
/// </summary>
public class QueryModelBase
{
    /// <summary>
    /// ID
    /// </summary>
    [TableColumn(Sort = 0, Show = false)]
    public string? Id { get; set; }

    /// <summary>
    /// 创建人Id
    /// </summary>
    [TableColumn(Sort = 85, Show = false)]
    public string? CreatedUserId { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    [TableColumn(Sort = 86, Show = false, Width = 90)]
    public string? CreatedUserName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [TableColumn(Sort = 87, Show = false)]
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// 更新人Id  
    /// </summary>
    [TableColumn(Sort = 88, Show = false)]
    public string? UpdatedUserId { get; set; }

    /// <summary>
    /// 更新人
    /// </summary>
    [TableColumn(Sort = 89, Show = false, Width = 90)]
    public string? UpdatedUserName { get; set; }

    /// <summary>
    /// 最后修改时间
    /// </summary>
    [TableColumn(Sort = 90, Show = false)]
    public DateTime? UpdatedOn { get; set; }
}