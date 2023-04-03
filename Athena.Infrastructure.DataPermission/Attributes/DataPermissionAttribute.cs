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
    public DataPermissionAttribute(Type baseType)
    {
        BaseType = baseType;
    }

    /// <summary>
    /// 数据权限属性
    /// </summary>
    /// <param name="baseType">基础类型，用于控制查询</param>
    /// <param name="displayName">显示名</param>
    public DataPermissionAttribute(Type baseType, string displayName)
    {
        DisplayName = displayName;
        BaseType = baseType;
    }

    /// <summary>
    /// 权限名称
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// KEY
    /// </summary>
    public string? Key { get; set; }

    /// <summary>
    /// 基础类型
    /// </summary>
    public Type BaseType { get; set; }

    /// <summary>
    /// 分组
    /// </summary>
    public string? Group { get; set; }
}