namespace BasicPlatform.AppService.Applications.Requests;

/// <summary>
/// 
/// </summary>
public class GetApplicationPagingRequest : GetPagingRequestBase
{
    /// <summary>
    /// 环境
    /// </summary>
    public string? Environment { get; set; }
}