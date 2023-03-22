namespace BasicPlatform.Infrastructure.Tables;

/// <summary>
/// 表格列特性
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class TableColumnAttribute : Attribute
{
    /// <summary>
    /// 列名
    /// </summary>
    public string? DataIndex { get; set; }

    /// <summary>
    /// 列标题
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// 列宽度
    /// </summary>
    public int Width { get; set; } = -1;

    /// <summary>
    /// 是否显示
    /// </summary>
    public bool Show { get; set; } = true;

    /// <summary>
    /// 是否必须
    /// <remarks>为true时用户不能关掉</remarks>
    /// </summary>
    public bool Required { get; set; }

    /// <summary>
    /// 固定到左侧或者右侧
    /// <remarks>可选值为 'left' 'right' 'undefined</remarks>
    /// </summary>
    public string? Fixed { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; } = 99;

    /// <summary>
    /// 超出宽度显示省略号
    /// </summary>
    public bool Ellipsis { get; set; } = true;

    /// <summary>
    /// 文字对齐方式
    /// </summary>
    public string Align { get; set; } = "left";

    /// <summary>
    /// 是否可排序
    /// </summary>
    public bool Sorter { get; set; }

    /// <summary>
    /// 是否可筛选
    /// </summary>
    public bool Filters { get; set; }

    /// <summary>
    /// 数据类型
    /// </summary>
    public string? ValueType { get; set; }

    /// <summary>
    /// 是否忽略
    /// <remarks>为true时将不会读取</remarks>
    /// </summary>
    public bool Ignore { get; set; }
}