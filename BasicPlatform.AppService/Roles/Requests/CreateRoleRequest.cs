using BasicPlatform.AppService.Roles.Models;

namespace BasicPlatform.AppService.Roles.Requests;

/// <summary>
/// 创建角色请求类
/// </summary>
public class CreateRoleRequest : RoleModel, ITxRequest<string>
{
    /// <summary>
    /// 资源列表
    /// </summary>
    public IList<ResourceModel>? Resources { get; set; }
}