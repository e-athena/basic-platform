namespace BasicPlatform.Infrastructure.Enums;

/// <summary>
/// 角色数据访问范围
/// </summary>
public enum RoleDataScope
{
    /// <summary>
    /// 全部
    /// </summary>
    [Description("全部")] All = 0,

    /// <summary>
    /// 本人
    /// </summary>
    [Description("本人")] Self = 1,

    /// <summary>
    /// 本部门
    /// </summary>
    [Description("本部门")] Department = 2,

    /// <summary>
    /// 本部门及下属部门
    /// </summary>
    [Description("本部门及下属部门")] DepartmentAndSub = 3,

    /// <summary>
    /// 自定义
    /// </summary>
    [Description("自定义")] Custom = 4
}