namespace BasicPlatform.AppService.Organizations.Requests;

/// <summary>
/// 变更组织架构状态请求
/// </summary>
public class OrganizationStatusChangeRequest : ITxRequest<string>
{
    /// <summary>
    /// 组织架构ID
    /// </summary>
    public string Id { get; set; } = null!;
}