namespace BasicPlatform.AppService.Organizations.Requests;

/// <summary>
/// 创建组织架构请求类
/// </summary>
public class CreateOrganizationRequest : ITxRequest<string>
{
    /// <summary>
    /// 父级Id
    /// </summary>
    public string? ParentId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 部门负责人Id
    /// </summary>
    public string? LeaderId { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remarks { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public Status Status { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 角色Ids
    /// </summary>
    public IList<string> RoleIds { get; set; } = new List<string>();
}