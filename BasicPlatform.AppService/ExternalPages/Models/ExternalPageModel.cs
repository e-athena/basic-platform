namespace BasicPlatform.AppService.ExternalPages.Models;

/// <summary>
/// 外部页面
/// </summary>
public class ExternalPageModel : ViewModelBase
{
    /// <summary>
    /// 上级ID
    /// </summary>
    [MaxLength(36)]
    public string? ParentId { get; set; }

    /// <summary>
    /// 所属人
    /// <remarks>为空时全局可用</remarks>
    /// </summary>
    [MaxLength(36)]
    public string? OwnerId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [MaxLength(32)]
    [Required]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 跳转类型
    /// </summary>
    [Required]
    public ExternalPageType Type { get; set; }

    /// <summary>
    /// 跳转地址
    /// </summary>
    [MaxLength(1000)]
    [Required]
    public string Path { get; set; } = null!;

    /// <summary>
    /// 图标
    /// </summary>
    [MaxLength(32)]
    public string Icon { get; set; } = "LinkOutlined";

    /// <summary>
    /// 布局
    /// <remarks>top side mix</remarks>
    /// </summary>
    public string Layout { get; set; } = "mix";

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remarks { get; set; }
}