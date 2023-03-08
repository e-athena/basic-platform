namespace BasicPlatform.AppService.Organizations.Requests;

/// <summary>
/// 
/// </summary>
public class GetOrganizationPagingRequest : GetPagingRequestBase
{
    /// <summary>
    /// 上级ID
    /// </summary>
    public string? ParentId { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public IList<Status>? Status { get; set; }
}