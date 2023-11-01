namespace BasicPlatform.AppService;

/// <summary>
/// Dto基类
/// </summary>
public class ModelBase
{
    /// <summary>
    /// ID
    /// </summary>
    [MaxLength(36,ErrorMessage = "ID不合法")]
    public string? Id { get; set; }
}