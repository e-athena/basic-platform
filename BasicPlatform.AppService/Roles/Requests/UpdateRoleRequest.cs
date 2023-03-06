using BasicPlatform.AppService.Roles.Models;

namespace BasicPlatform.AppService.Roles.Requests;

/// <summary>
/// 更新角色请求类
/// </summary>
public class UpdateRoleRequest : RoleModel, ITxRequest<string>
{
    /// <summary>
    /// 资源列表
    /// </summary>
    public IList<ResourceModel> Resources { get; set; } = new List<ResourceModel>();
}