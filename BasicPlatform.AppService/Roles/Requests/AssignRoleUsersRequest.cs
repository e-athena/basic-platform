namespace BasicPlatform.AppService.Roles.Requests;

/// <summary>
/// 分配角色用户请求类
/// </summary>
public class AssignRoleUsersRequest : ITxRequest<string>
{
    /// <summary>
    /// 角色ID
    /// </summary>
    public string Id { get; set; } = null!;

    /// <summary>
    /// 用户列表
    /// </summary>
    public List<string> UserIds { get; set; } = new List<string>();
}