using BasicPlatform.AppService.Organizations.Models;

namespace BasicPlatform.AppService.Organizations.Requests;

/// <summary>
/// 更新组织架构请求类
/// </summary>
public class UpdateOrganizationRequest : ITxRequest<string>
{
    /// <summary>
    /// Id
    /// </summary>
    public string Id { get; set; } = null!;

    /// <summary>
    /// 父级Id
    /// </summary>
    public string? ParentId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 部门负责人Id
    /// </summary>
    public string? LeaderId { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remarks { get; set; }

    /// <summary>
    /// 角色Ids
    /// </summary>
    public IList<string> RoleIds { get; set; } = new List<string>();
}