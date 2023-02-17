namespace BasicPlatform.AppService.Organizations.Requests;

/// <summary>
/// 
/// </summary>
public class GetOrganizationPagesRequest : GetPageRequestBase
{
    /// <summary>
    /// 状态
    /// </summary>
    public IList<string>? Status { get; set; }
}