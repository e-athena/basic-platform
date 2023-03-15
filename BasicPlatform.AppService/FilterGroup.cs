namespace BasicPlatform.AppService;

/// <summary>
/// 
/// </summary>
public class FilterGroup
{
    /// <summary>
    /// or or and
    /// </summary>
    public string XOR { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public IList<Filter> Filters { get; set; }=new List<Filter>();
}