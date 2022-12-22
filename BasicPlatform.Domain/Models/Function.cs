namespace BasicPlatform.Domain.Models;

/// <summary>
/// 功能
/// </summary>
[Table("AuthorityFunctions")]
public class Function : EntityCore, ICreator, IUpdater
{
    /// <summary>
    /// 所属模块Id
    /// </summary>
    /// <value></value>
    [MaxLength(36)]
    public string ModuleId { get; set; } = null!;

    /// <summary>
    /// 所属菜单
    /// </summary>
    /// <value></value>
    public virtual Module Module { get; set; } = null!;

    /// <summary>
    /// 名称
    /// </summary>
    /// <value></value>
    [Required]
    [MaxLength(12)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 操作编码,分号隔开
    /// </summary>
    /// <value></value>
    [MaxLength(256)]
    public string Code { get; set; } = null!;

    /// <summary>
    /// 排序字段
    /// </summary>
    /// <value></value>
    public int Sort { get; set; }

    /// <summary>
    /// 创建人Id
    /// </summary>
    [MaxLength(36)]
    public string? CreatedUserId { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public virtual User? CreatedUser { get; set; }

    /// <summary>
    /// 最后更新人Id
    /// </summary>
    [MaxLength(36)]
    public string? UpdatedUserId { get; set; }

    /// <summary>
    /// 最后更新人
    /// </summary>
    public virtual User? UpdatedUser { get; set; }

    public Function()
    {
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="moduleId"></param>
    /// <param name="name"></param>
    /// <param name="code"></param>
    /// <param name="sort"></param>
    /// <param name="userId"></param>
    public Function(string moduleId, string name, string code, int sort, string? userId)
    {
        ModuleId = moduleId;
        Name = name;
        Code = code;
        Sort = sort;
        CreatedUserId = userId;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="name"></param>
    /// <param name="code"></param>
    /// <param name="sort"></param>
    /// <param name="userId"></param>
    public void Update(string name, string code, int sort, string? userId)
    {
        Name = name;
        Code = code;
        Sort = sort;
        UpdatedUserId = userId;
        UpdatedOn = DateTime.Now;
    }
}