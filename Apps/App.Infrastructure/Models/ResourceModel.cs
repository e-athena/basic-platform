namespace App.Infrastructure.Models;

/// <summary>
/// 资源模型
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class ResourceModel
{
    /// <summary>
    /// 应用ID
    /// </summary>
    public string? AppId { get; set; }

    /// <summary>
    /// KEY
    /// </summary>
    public string Key { get; set; } = null!;

    /// <summary>
    /// Code
    /// <remarks>多个用逗号分割</remarks>
    /// </summary>
    public string Code { get; set; } = null!;

    /// <summary>
    /// 资源代码列表
    /// </summary>
    public IList<string> Codes =>
        string.IsNullOrEmpty(Code) ? new List<string>() : Code.Split(",");
}