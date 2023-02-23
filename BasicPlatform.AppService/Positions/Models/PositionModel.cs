namespace BasicPlatform.AppService.Positions.Models;

/// <summary>
/// [职位]数据传输对象
/// </summary>
public class PositionModel : ViewModelBase
{
    /// <summary>
    /// 父级Id
    /// </summary>
    public string? ParentId { get; set; }

    /// <summary>
    /// 职位名
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remarks { get; set; } = "";

    /// <summary>
    /// 状态
    /// </summary>
    public Status Status { get; set; }
}