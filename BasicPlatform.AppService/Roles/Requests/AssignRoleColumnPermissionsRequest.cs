using BasicPlatform.AppService.Roles.Models;

namespace BasicPlatform.AppService.Roles.Requests;

/// <summary>
/// 分配角色列权限请求类
/// </summary>
public class AssignRoleColumnPermissionsRequest : ITxRequest<string>
{
    /// <summary>
    /// 角色ID
    /// </summary>
    public string Id { get; set; } = null!;

    /// <summary>
    /// 权限列表
    /// </summary>
    public IList<RoleColumnPermissionModel> Permissions { get; set; } = new List<RoleColumnPermissionModel>();
}