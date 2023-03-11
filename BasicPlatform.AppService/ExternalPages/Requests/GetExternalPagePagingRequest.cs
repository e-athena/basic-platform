namespace BasicPlatform.AppService.ExternalPages.Requests;

/// <summary>
/// 
/// </summary>
public class GetExternalPagePagingRequest : GetPagingRequestBase
{
    /// <summary>
    /// 上级ID
    /// </summary>
    public string? ParentId { get; set; }
}