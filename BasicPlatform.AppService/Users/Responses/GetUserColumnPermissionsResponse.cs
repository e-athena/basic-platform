using BasicPlatform.AppService.Users.Models;

namespace BasicPlatform.AppService.Users.Responses;

/// <summary>
/// 读取用户列权限响应类
/// </summary>
public class GetUserColumnPermissionsResponse : UserColumnPermissionModel
{
    /// <summary>
    /// 是否为角色权限
    /// </summary>
    public bool IsRolePermission { get; set; }

    /// <summary>
    /// 禁用选中
    /// </summary>
    public bool DisableChecked => IsRolePermission;
}