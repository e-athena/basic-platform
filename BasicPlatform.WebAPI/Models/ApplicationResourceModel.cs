namespace BasicPlatform.WebAPI.Models;

/// <summary>
/// 应用资源模型
/// </summary>
public class ApplicationResourceModel
{
    /// <summary>
    /// 应用名称
    /// </summary>
    public string ApplicationName { get; set; } = null!;

    /// <summary>
    /// 应用ID
    /// </summary>
    public string ApplicationId { get; set; } = null!;

    /// <summary>
    /// 资源列表
    /// </summary>
    public IList<MenuTreeInfo> Resources { get; set; } = null!;
}