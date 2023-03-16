namespace BasicPlatform.AppService;

/// <summary>
/// 自定义查询过滤器组
/// </summary>
public class FilterGroup
{
    /// <summary>
    /// 与或，or or and
    /// </summary>
    public string XOR { get; set; } = null!;

    /// <summary>
    /// 自定义查询过滤器列表
    /// </summary>
    public IList<Filter> Filters { get; set; }=new List<Filter>();
}