namespace BasicPlatform.Infrastructure.Tables;

/// <summary>
/// 表格列项模型
/// </summary>
public class TableColumnInfo
{
    /// <summary>
    /// 列名
    /// </summary>
    public string DataIndex { get; set; } = null!;

    /// <summary>
    /// 列标题
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// 列宽度
    /// </summary>
    public int Width { get; set; } = 200;

    /// <summary>
    /// 是否显示
    /// </summary>
    public bool Show { get; set; } = true;

    /// <summary>
    /// 是否必须
    /// </summary>
    public bool Required { get; set; }

    /// <summary>
    /// 固定到左侧
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
    /// <remarks>left，center,right</remarks>
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
    /// 值类型
    /// </summary>
    public string? ValueType { get; set; }

    /// <summary>
    /// 枚举值
    /// </summary>
    public Dictionary<int, dynamic>? ValueEnum { get; set; }

    /// <summary>
    /// 属性类型
    /// </summary>
    public string? PropertyType { get; set; }

    /// <summary>
    /// 属性名称
    /// </summary>
    public string? PropertyName { get; set; }

    /// <summary>
    /// 枚举选项
    /// </summary>
    public List<dynamic>? EnumOptions { get; set; }
}