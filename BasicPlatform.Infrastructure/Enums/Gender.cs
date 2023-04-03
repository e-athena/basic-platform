namespace BasicPlatform.Infrastructure.Enums;

/// <summary>
/// 性别
/// </summary>
public enum Gender
{
    /// <summary>
    /// 保密
    /// </summary>
    [Description("保密")] Unknown = 0,
    /// <summary>
    /// 男
    /// </summary>
    [Description("男")] Man = 1,
    /// <summary>
    /// 女
    /// </summary>
    [Description("女")] Woman = 2
}