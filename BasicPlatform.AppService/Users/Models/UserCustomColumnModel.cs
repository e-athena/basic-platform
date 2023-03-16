namespace BasicPlatform.AppService.Users.Models;

/// <summary>
/// 用户自定义列
/// </summary>
public class UserCustomColumnModel
{
    /// <summary>
    /// 列名
    /// </summary>
    [MaxLength(128)]
    public string DataIndex { get; set; } = null!;

    /// <summary>
    /// 宽度
    /// <remarks>为空时自动</remarks>
    /// </summary>
    public int? Width { get; set; }

    /// <summary>
    /// 是否显示
    /// </summary>
    public bool Show { get; set; }

    /// <summary>
    /// 固定列
    /// <remarks>left,right</remarks>
    /// </summary>
    [MaxLength(10)]
    public string? Fixed { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}