namespace BasicPlatform.AppService.WebSettings.Models;

/// <summary>
/// 网站设置模型
/// </summary>
public class WebSettingModel : ModelBase
{
    /// <summary>
    /// Logo
    /// </summary>
    public string Logo { get; set; } = "https://cdn.gzwjz.com/FmzrX15jYA03KMVfbgMJnk-P6WGl.png";

    /// <summary>
    /// ico
    /// </summary>
    public string Ico { get; set; } = "/favicon.ico";

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 简称
    /// </summary>
    public string ShortName { get; set; } = null!;

    /// <summary>
    /// 描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 版权
    /// </summary>
    public string? CopyRight { get; set; }
}