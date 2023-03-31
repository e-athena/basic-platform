namespace BasicPlatform.AppService.DataPermissions.Models;

/// <summary>
/// 数据权限属性
/// </summary>
public class DataPermissionProperty
{
    /// <summary>
    /// 显示名称
    /// </summary>
    public string Label { get; set; } = null!;

    /// <summary>
    /// 值
    /// </summary>
    public string Value { get; set; } = null!;

    /// <summary>
    /// Key
    /// </summary>
    public string Key { get; set; } = null!;

    /// <summary>
    /// 属性类型
    /// </summary>
    public string? PropertyType { get; set; }

    /// <summary>
    /// 枚举选项列表
    /// </summary>
    public List<dynamic>? EnumOptions { get; set; }
}