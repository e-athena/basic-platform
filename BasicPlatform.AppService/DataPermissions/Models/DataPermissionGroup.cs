namespace BasicPlatform.AppService.DataPermissions.Models;

/// <summary>
/// 
/// </summary>
public class DataPermissionGroup
{
    /// <summary>
    /// 显示名
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// 子项
    /// </summary>
    public IList<DataPermission> Items { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="displayName"></param>
    /// <param name="items"></param>
    public DataPermissionGroup(string displayName, IList<DataPermission> items)
    {
        DisplayName = displayName;
        Items = items;
    }
}