using BasicPlatform.AppService.Roles.Models;

namespace BasicPlatform.AppService.Roles.Responses;

/// <summary>
/// 读取角色信息
/// </summary>
public class GetRoleByIdResponse : RoleQueryModel
{
    /// <summary>
    /// 资源代码
    /// </summary>
    public List<ResourceModel> Resources { get; set; } = new();

    /// <summary>
    /// 自定义数据访问范围
    /// </summary>
    [TableColumn(Ignore = true)]
    public IList<SelectViewModel> DataScopeCustomSelectList { get; set; } = new List<SelectViewModel>();
}