namespace BasicPlatform.AppService;

/// <summary>
/// Dto基类
/// </summary>
public class ModelBase
{
    /// <summary>
    /// ID
    /// </summary>
    [TableColumn(Sort = 0, Show = false)]
    public string? Id { get; set; }
}