namespace App.Infrastructure.Models;

/// <summary>
/// 用户自定义列
/// </summary>
public class UserCustomColumnModel
{
    /// <summary>
    /// 列名
    /// </summary>
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
    public string? Fixed { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}