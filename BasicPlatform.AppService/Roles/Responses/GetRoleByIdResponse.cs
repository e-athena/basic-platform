using BasicPlatform.AppService.Roles.Models;

namespace BasicPlatform.AppService.Roles.Responses;

/// <summary>
/// 读取角色信息
/// </summary>
public class GetRoleByIdResponse : RoleModel
{
    /// <summary>
    /// 状态
    /// </summary>
    /// <value></value>
    public Status Status { get; set; }

    /// <summary>
    /// 资源代码
    /// </summary>
    public List<ResourceModel> Resources { get; set; } = new();
}