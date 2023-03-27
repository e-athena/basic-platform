using BasicPlatform.AppService.Roles.Models;

namespace BasicPlatform.AppService.Roles.Requests;

/// <summary>
/// 分配角色数据权限请求类
/// </summary>
public class AssignRoleDataPermissionsRequest : ITxRequest<string>
{
    /// <summary>
    /// 角色ID
    /// </summary>
    public string Id { get; set; } = null!;

    /// <summary>
    /// 权限列表
    /// </summary>
    public IList<RoleDataPermissionModel> Permissions { get; set; } = new List<RoleDataPermissionModel>();
}