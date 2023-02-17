namespace BasicPlatform.AppService.Organizations.Models;

/// <summary>
/// 组织架构Dto
/// </summary>
public class OrganizationViewModel : ViewModelBase
{
    /// <summary>
    /// 父级Id
    /// </summary>
    public string? ParentId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 路径
    /// </summary>
    public string ParentPath { get; set; } = string.Empty;

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remarks { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public Status Status { get; set; }
}