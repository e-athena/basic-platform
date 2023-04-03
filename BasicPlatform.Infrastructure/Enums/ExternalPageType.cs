namespace BasicPlatform.Infrastructure.Enums;

/// <summary>
/// 跳转类型
/// </summary>
public enum ExternalPageType
{
    /// <summary>
    /// 外部链接
    /// </summary>
    [Description("外部链接")] ExternalLink = 1,

    /// <summary>
    /// 内部链接
    /// </summary>
    [Description("内部链接")] InternalLink = 2
}