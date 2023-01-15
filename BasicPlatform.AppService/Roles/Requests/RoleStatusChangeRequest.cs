namespace BasicPlatform.AppService.Roles.Requests;

/// <summary>
/// 角色状态变更请求类
/// </summary>
public class RoleStatusChangeRequest : IdRequest, ITxRequest<string>
{
}