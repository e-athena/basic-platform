namespace BasicPlatform.AppService.Tenants.Requests;

/// <summary>
/// 分配租户资源请求类
/// </summary>
public class AssignTenantResourcesRequest : ITxRequest<string>
{
    /// <summary>
    /// 租户ID
    /// </summary>
    public string Id { get; set; } = null!;

    /// <summary>
    /// 资源列表
    /// </summary>
    public IList<ResourceModel> Resources { get; set; } = new List<ResourceModel>();
}