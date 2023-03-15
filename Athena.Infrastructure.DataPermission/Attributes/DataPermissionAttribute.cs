namespace Athena.Infrastructure.DataPermission.Attributes;

/// <summary>
/// 数据权限属性
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class DataPermissionAttribute : Attribute
{
    /// <summary>
    /// 数据权限属性
    /// </summary>
    public DataPermissionAttribute()
    {
    }

    /// <summary>
    /// 数据权限属性
    /// </summary>
    /// <param name="displayName">显示名</param>
    public DataPermissionAttribute(string displayName)
    {
        DisplayName = displayName;
    }

    /// <summary>
    /// 权限名称
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// KEY
    /// </summary>
    public string? Key { get; set; }
}