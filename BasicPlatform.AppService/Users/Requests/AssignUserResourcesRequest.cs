namespace BasicPlatform.AppService.Users.Requests;

/// <summary>
/// 分配用户资源请求类
/// </summary>
public class AssignUserResourcesRequest : ITxRequest<string>
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public string Id { get; set; } = null!;

    /// <summary>
    /// 资源列表
    /// </summary>
    public IList<ResourceModel> Resources { get; set; } = new List<ResourceModel>();

    /// <summary>
    /// 有效期至
    /// <remarks>为空时永久有效</remarks>
    /// </summary>
    public DateTime? ExpireAt { get; set; }
}