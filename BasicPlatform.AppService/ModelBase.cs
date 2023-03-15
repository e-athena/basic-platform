namespace BasicPlatform.AppService;

/// <summary>
/// Dto基类
/// </summary>
public class ModelBase
{
    /// <summary>ID</summary>
    [TableColumn(Sort = 0, Show = false)]
    public string? Id { get; set; }

    /// <summary>创建时间</summary>
    [TableColumn(Sort = 97, Show = false)]
    public DateTime CreatedOn { get; set; }

    /// <summary>最后修改时间</summary>
    [TableColumn(Sort = 98, Show = false)]
    public DateTime? UpdatedOn { get; set; }
}