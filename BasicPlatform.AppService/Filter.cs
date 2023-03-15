namespace BasicPlatform.AppService;

/// <summary>
/// 
/// </summary>
public class Filter
{
    /// <summary>
    /// Key
    /// </summary>
    public string Key { get; set; } = null!;

    /// <summary>
    /// Value
    /// </summary>
    public string Value { get; set; } = null!;

    /// <summary>
    /// 运算符
    /// </summary>
    public string Operator { get; set; } = null!;

    /// <summary>
    /// or or and
    /// </summary>
    public string XOR { get; set; } = null!;
}