using BasicPlatform.AppService.Roles.Models;

namespace BasicPlatform.AppService.Roles.Responses;

/// <summary>
/// 读取角色分页列表响应类
/// </summary>
[DataPermission(typeof(Role), "角色管理模块")]
public class GetRolePagingResponse : RoleQueryModel
{
}