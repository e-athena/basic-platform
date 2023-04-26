namespace BasicPlatform.Domain.Models;

/// <summary>
/// 角色
/// </summary>
[Table("authority_roles")]
public class Role : EntityCore, ICreator, IUpdater
{
    /// <summary>
    /// 名称
    /// </summary>
    [Required]
    [MaxLength(32)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(1024)]
    public string? Remarks { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    /// <value></value>
    public Status Status { get; set; } = Status.Enabled;

    /// <summary>
    /// 数据访问范围
    /// </summary>
    public RoleDataScope DataScope { get; set; } = RoleDataScope.All;

    /// <summary>
    /// 自定义数据访问范围(组织Id)
    /// <remarks>多个组织使用逗号分割</remarks>
    /// </summary>
    [MaxLength(-1)]
    public string? DataScopeCustom { get; set; }

    /// <summary>
    /// 创建人Id
    /// </summary>
    [MaxLength(36)]
    public string? CreatedUserId { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public User? CreatedUser { get; set; }

    /// <summary>
    /// 最后更新人Id
    /// </summary>
    [MaxLength(36)]
    public string? LastUpdatedUserId { get; set; }

    /// <summary>
    /// 最后更新人
    /// </summary>
    public virtual User? LastUpdatedUser { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Role()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="dataScope"></param>
    /// <param name="dataScopeCustom"></param>
    /// <param name="remarks"></param>
    /// <param name="createdUserId"></param>
    public Role(string name, RoleDataScope dataScope, string? dataScopeCustom, string? remarks,
        string? createdUserId)
    {
        Name = name;
        DataScope = dataScope;
        DataScopeCustom = dataScopeCustom;
        Remarks = remarks;
        CreatedUserId = createdUserId;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="name"></param>
    /// <param name="dataScope"></param>
    /// <param name="dataScopeCustom"></param>
    /// <param name="remarks"></param>
    /// <param name="updatedUserId"></param>
    public void Update(string name, RoleDataScope dataScope, string? dataScopeCustom, string? remarks,
        string? updatedUserId)
    {
        Name = name;
        DataScope = dataScope;
        DataScopeCustom = dataScopeCustom;
        Remarks = remarks;
        LastUpdatedUserId = updatedUserId;
    }

    /// <summary>
    /// 状态变更
    /// </summary>
    /// <param name="updatedUserId"></param>
    public void StatusChange(string? updatedUserId)
    {
        Status = Status == Status.Disabled ? Status.Enabled : Status.Disabled;
        LastUpdatedUserId = updatedUserId;
        UpdatedOn = DateTime.Now;
    }

    /// <summary>
    /// 数据权限授权
    /// </summary>
    /// <param name="dataScope">范围</param>
    /// <param name="customIds">自定义授权的组织Id列表</param>
    /// <param name="updatedUserId"></param>
    public void DataScopeAssign(RoleDataScope dataScope, IList<string>? customIds, string? updatedUserId)
    {
        DataScope = dataScope;
        if (dataScope == RoleDataScope.Custom && customIds != null && customIds.Count > 0)
        {
            DataScopeCustom = string.Join(",", customIds);
        }
        else
        {
            DataScopeCustom = null;
        }

        LastUpdatedUserId = updatedUserId;
    }
}