using BasicPlatform.AppService.ExternalPages.Models;

namespace BasicPlatform.AppService.ExternalPages.Responses;

/// <summary>
/// 
/// </summary>
public class GetExternalPageByIdResponse : ExternalPageModel
{
    /// <summary>
    /// 是否通用页面
    /// </summary>
    public bool IsPublic => string.IsNullOrWhiteSpace(OwnerId);

    /// <summary>
    /// 是否为分组
    /// </summary>
    public bool IsGroup { get; set; }
}