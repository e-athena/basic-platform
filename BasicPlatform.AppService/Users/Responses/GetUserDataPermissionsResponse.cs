using BasicPlatform.AppService.Users.Models;

namespace BasicPlatform.AppService.Users.Responses;

/// <summary>
/// 读取用户数据权限响应类
/// </summary>
public class GetUserDataPermissionsResponse : UserDataPermissionModel
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