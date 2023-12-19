namespace BasicPlatform.Domain.Models.WebSettings.Events;

/// <summary>
/// 网站设置更新成功事件
/// </summary>
public class WebSettingUpdatedEvent : EventBase
{
    /// <summary>
    /// Logo
    /// </summary>
    public string Logo { get; set; }

    /// <summary>
    /// ico
    /// </summary>
    public string Ico { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 简称
    /// </summary>
    public string ShortName { get; set; }

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
    /// <param name="logo"></param>
    /// <param name="ico"></param>
    /// <param name="name"></param>
    /// <param name="shortName"></param>
    /// <param name="description"></param>
    /// <param name="copyRight"></param>
    public WebSettingUpdatedEvent(string logo, string ico, string name, string shortName, string? description,
        string? copyRight)
    {
        Logo = logo;
        Ico = ico;
        Name = name;
        ShortName = shortName;
        Description = description;
        CopyRight = copyRight;
    }
}