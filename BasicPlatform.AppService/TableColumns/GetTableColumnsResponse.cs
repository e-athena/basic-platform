namespace BasicPlatform.AppService.TableColumns;

/// <summary>
/// 读取表格列响应类
/// </summary>
public class GetTableColumnsResponse
{
    /// <summary>
    /// 模块名称
    /// </summary>
    public string ModuleName { get; set; } = null!;

    /// <summary>
    /// 列信息
    /// </summary>
    public IList<TableColumnInfo> Columns { get; set; } = null!;
}