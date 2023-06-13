namespace BasicPlatform.Domain.Models.Users;

/// <summary>
/// 用户自定义列
/// </summary>
[Table("basic_user_custom_columns")]
public class UserCustomColumn : ValueObject
{
    /// <summary>
    /// 用户ID
    /// </summary>
    [MaxLength(36)]
    public string UserId { get; set; } = null!;

    /// <summary>
    /// 用户
    /// </summary>
    public virtual User User { get; set; } = null!;

    /// <summary>
    /// 应用ID
    /// </summary>
    [MaxLength(64)]
    public string? AppId { get; set; }

    /// <summary>
    /// 所属模块
    /// </summary>
    [MaxLength(128)]
    public string ModuleName { get; set; } = null!;

    /// <summary>
    /// 列名
    /// </summary>
    [MaxLength(128)]
    public string DataIndex { get; set; } = null!;

    /// <summary>
    /// 宽度
    /// <remarks>为空时自动</remarks>
    /// </summary>
    public int? Width { get; set; }

    /// <summary>
    /// 是否显示
    /// </summary>
    public bool Show { get; set; }

    /// <summary>
    /// 固定列
    /// <remarks>left,right</remarks>
    /// </summary>
    [MaxLength(10)]
    public string? Fixed { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public UserCustomColumn()
    {
    }

    /// <summary>
    /// 添加用户自定义列
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="appId"></param>
    /// <param name="moduleName"></param>
    /// <param name="dataIndex"></param>
    /// <param name="width"></param>
    /// <param name="show"></param>
    /// <param name="fixed"></param>
    /// <param name="sort"></param>
    public UserCustomColumn(string userId, string? appId, string moduleName, string dataIndex, int? width, bool show,
        string? @fixed,
        int sort)
    {
        UserId = userId;
        AppId = appId;
        ModuleName = moduleName;
        DataIndex = dataIndex;
        Width = width;
        Show = show;
        Fixed = @fixed;
        Sort = sort;
    }
}