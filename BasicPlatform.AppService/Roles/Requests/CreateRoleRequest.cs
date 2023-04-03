using BasicPlatform.AppService.Roles.Models;

namespace BasicPlatform.AppService.Roles.Requests;

/// <summary>
/// 创建角色请求类
/// </summary>
public class CreateRoleRequest : RoleModel, ITxRequest<string>
{
}