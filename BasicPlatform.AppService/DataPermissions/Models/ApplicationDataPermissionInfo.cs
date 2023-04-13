namespace BasicPlatform.AppService.DataPermissions.Models;

/// <summary>
/// 应用数据权限
/// </summary>
public class ApplicationDataPermissionInfo
{
    /// <summary>
    /// 应用名称
    /// </summary>
    public string ApplicationName { get; set; } = null!;

    /// <summary>
    /// 应用ID
    /// </summary>
    public string ApplicationId { get; set; } = null!;

    /// <summary>
    /// 数据权限组列表
    /// </summary>
    public IList<DataPermissionGroup> DataPermissionGroups { get; set; } = new List<DataPermissionGroup>();
}