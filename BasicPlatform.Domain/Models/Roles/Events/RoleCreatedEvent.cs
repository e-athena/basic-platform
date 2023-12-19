namespace BasicPlatform.Domain.Models.Roles.Events;

/// <summary>
/// 角色创建事件
/// </summary>
public class RoleCreatedEvent : EventBase
{
    /// <summary>
    /// 角色名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remarks { get; set; }

    /// <summary>
    /// 数据访问范围
    /// </summary>
    public RoleDataScope DataScope { get; set; }

    /// <summary>
    /// 自定义数据访问范围(组织Id)
    /// </summary>
    public string? DataScopeCustom { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="name"></param>
    /// <param name="remarks"></param>
    /// <param name="dataScope"></param>
    /// <param name="dataScopeCustom"></param>
    public RoleCreatedEvent(string name, string? remarks, RoleDataScope dataScope,
        string? dataScopeCustom)
    {
        Name = name;
        Remarks = remarks;
        DataScope = dataScope;
        DataScopeCustom = dataScopeCustom;
    }
}