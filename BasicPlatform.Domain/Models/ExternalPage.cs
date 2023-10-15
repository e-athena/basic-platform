using BasicPlatform.Domain.Models.Users;

namespace BasicPlatform.Domain.Models;

/// <summary>
/// 外部页面管理
/// </summary>
[Table("authority_external_pages")]
public class ExternalPage : FullEntityCore
{
    /// <summary>
    /// 上级ID
    /// </summary>
    [MaxLength(36)]
    public string? ParentId { get; set; }

    /// <summary>
    /// 所属人
    /// <remarks>为空时全局可用</remarks>
    /// </summary>
    [MaxLength(36)]
    public string? OwnerId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [MaxLength(32)]
    [Required]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 跳转类型
    /// </summary>
    [Required]
    public ExternalPageType Type { get; set; }

    /// <summary>
    /// 跳转地址
    /// </summary>
    [MaxLength(1000)]
    [Required]
    public string Path { get; set; } = null!;

    /// <summary>
    /// 图标
    /// </summary>
    [MaxLength(32)]
    public string Icon { get; set; } = "LinkOutlined";

    /// <summary>
    /// 布局
    /// <remarks>top side mix</remarks>
    /// </summary>
    public string Layout { get; set; } = "mix";

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remarks { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public virtual User? CreatedUser { get; set; }

    /// <summary>
    /// 最后更新人
    /// </summary>
    public virtual User? LastUpdatedUser { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public ExternalPage()
    {
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="parentId"></param>
    /// <param name="ownerId"></param>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <param name="path"></param>
    /// <param name="icon"></param>
    /// <param name="layout"></param>
    /// <param name="sort"></param>
    /// <param name="remarks"></param>
    /// <param name="createdUserId"></param>
    public ExternalPage(
        string? parentId,
        string? ownerId,
        string name,
        ExternalPageType type,
        string path,
        string icon,
        string layout,
        int sort,
        string? remarks,
        string? createdUserId
    )
    {
        ParentId = parentId;
        OwnerId = ownerId;
        Name = name;
        Type = type;
        Path = path;
        Icon = icon;
        Layout = layout;
        Sort = sort;
        Remarks = remarks;
        CreatedUserId = createdUserId;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="parentId"></param>
    /// <param name="ownerId"></param>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <param name="path"></param>
    /// <param name="icon"></param>
    /// <param name="layout"></param>
    /// <param name="sort"></param>
    /// <param name="remarks"></param>
    /// <param name="updatedUserId"></param>
    public void Update(
        string? parentId,
        string? ownerId,
        string name,
        ExternalPageType type,
        string path,
        string icon,
        string layout,
        int sort,
        string? remarks,
        string? updatedUserId
    )
    {
        ParentId = parentId;
        OwnerId = ownerId;
        Name = name;
        Type = type;
        Path = path;
        Icon = icon;
        Layout = layout;
        Sort = sort;
        Remarks = remarks;
        LastUpdatedUserId = updatedUserId;
    }
}