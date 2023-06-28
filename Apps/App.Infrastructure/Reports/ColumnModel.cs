namespace App.Infrastructure.Reports;

/// <summary>
/// 色谱柱模型
/// </summary>
public class ColumnModel
{
    /// <summary>
    /// X轴
    /// </summary>
    public string XField { get; set; } = null!;

    /// <summary>
    /// Y轴(值)
    /// </summary>
    public decimal YField { get; set; }

    /// <summary>
    /// 分组
    /// </summary>
    public string SeriesField { get; set; } = null!;


    /// <summary>
    /// 
    /// </summary>
    public ColumnModel()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="xField"></param>
    /// <param name="yField"></param>
    /// <param name="seriesField"></param>
    public ColumnModel(string xField, decimal yField, string seriesField)
    {
        XField = xField;
        YField = yField;
        SeriesField = seriesField;
    }
}