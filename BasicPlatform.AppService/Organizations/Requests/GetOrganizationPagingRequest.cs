namespace BasicPlatform.AppService.Organizations.Requests;

/// <summary>
/// 
/// </summary>
public class GetOrganizationPagingRequest : GetPagingRequestBase
{
    /// <summary>
    /// 状态
    /// </summary>
    public IList<string>? Status { get; set; }
}