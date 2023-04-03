namespace BasicPlatform.AppService.Roles.Models;

/// <summary>
/// 角色模型
/// </summary>
public class RoleQueryModel : QueryModelBase
{
    /// <summary>
    /// 名称
    /// </summary>
    [Required]
    [MaxLength(32)]
    [TableColumn(Width = 150, Sort = 1)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 数据访问范围
    /// </summary>
    [TableColumn(Width = 145, Sort = 2, Filters = true, Sorter = true)]
    public RoleDataScope DataScope { get; set; }

    /// <summary>
    /// 自定义数据访问范围(组织Id)
    /// <remarks>多个组织使用逗号分割</remarks>
    /// </summary>
    [TableColumn(Title = "自定义数据访问范围", Width = 150, Sort = 1, HideInTable = true, HideInSearch = true)]
    public string? DataScopeCustom { get; set; }

    /// <summary>
    /// 自定义数据访问范围列表
    /// </summary>
    [TableColumn(Ignore = true)]
    public IList<string> DataScopeCustomList => DataScopeCustom?.Split(',').ToList() ?? new List<string>();

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(1024)]
    [TableColumn(Sort = 3)]
    public string? Remarks { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    /// <value></value>
    [TableColumn(Width = 90, Sort = 4)]
    public Status Status { get; set; }
}