namespace BasicPlatform.AppService.Users.Responses;

/// <summary>
/// 读取用户资源编码响应类
/// </summary>
public class GetUserResourceCodeInfoResponse
{
    /// <summary>
    /// 角色的资源编码列表
    /// </summary>
    public IList<ResourceModel> RoleResources { get; set; } = new List<ResourceModel>();
    
    /// <summary>
    /// 用户的资源编码列表
    /// </summary>
    public IList<ResourceModel> UserResources { get; set; }= new List<ResourceModel>();
}