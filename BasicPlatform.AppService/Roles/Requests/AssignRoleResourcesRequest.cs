namespace BasicPlatform.AppService.Roles.Requests;

/// <summary>
/// 分配用户资源请求类
/// </summary>
public class AssignRoleResourcesRequest : ITxRequest<string>
{
    /// <summary>
    /// 角色ID
    /// </summary>
    public string Id { get; set; } = null!;

    /// <summary>
    /// 资源列表
    /// </summary>
    public IList<ResourceModel> Resources { get; set; } = new List<ResourceModel>();
}