using BasicPlatform.AppService.Roles.Models;

namespace BasicPlatform.AppService.Roles.Requests;

/// <summary>
/// 更新角色请求类
/// </summary>
public class UpdateRoleRequest : RoleModel, IRequest<string>, ITransactionRequest
{
}