using BasicPlatform.Domain.Models.WebSettings.Events;

namespace BasicPlatform.Domain.Models.WebSettings;

/// <summary>
/// 网站设置
/// </summary>
[Table("web_settings")]
public class WebSetting : FullEntityCore
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


    /// <summary>
    ///
    /// </summary>
    public WebSetting()
    {
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="logo"></param>
    /// <param name="ico"></param>
    /// <param name="name"></param>
    /// <param name="shortName"></param>
    /// <param name="description"></param>
    /// <param name="copyRight"></param>
    public WebSetting(string logo, string ico, string name, string shortName, string? description, string? copyRight)
    {
        Logo = logo;
        Ico = ico;
        Name = name;
        ShortName = shortName;
        Description = description;
        CopyRight = copyRight;

        ApplyEvent(new WebSettingCreatedEvent(logo, ico, name, shortName, description, copyRight));
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="logo"></param>
    /// <param name="ico"></param>
    /// <param name="name"></param>
    /// <param name="shortName"></param>
    /// <param name="description"></param>
    /// <param name="copyRight"></param>
    public void Update(string logo, string ico, string name, string shortName, string? description, string? copyRight)
    {
        Logo = logo;
        Ico = ico;
        Name = name;
        ShortName = shortName;
        Description = description;
        CopyRight = copyRight;

        ApplyEvent(new WebSettingUpdatedEvent(logo, ico, name, shortName, description, copyRight));
    }
}