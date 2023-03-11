using BasicPlatform.AppService.ExternalPages.Models;

namespace BasicPlatform.AppService.ExternalPages.Responses;

/// <summary>
/// 
/// </summary>
public class GetExternalPagePagingResponse : ExternalPageModel
{
    /// <summary>
    /// 创建人Id  
    /// </summary>
    public string? CreatedUserId { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public string? CreatedUserName { get; set; }

    /// <summary>
    /// 更新人
    /// </summary>
    public string? UpdatedUserName { get; set; }

    /// <summary>
    /// 是否通用页面
    /// </summary>
    public bool IsPublic => string.IsNullOrWhiteSpace(OwnerId);
}