namespace Athena.Infrastructure.DataPermission;

/// <summary>
/// 数据权限服务接口
/// </summary>
public interface IDataPermissionService
{
    /// <summary>
    /// 获取授权的字段列表
    /// </summary>
    /// <param name="code"></param>
    /// <param name="userId">用户ID</param>
    /// <returns></returns>
    List<string> GetAuthFieldNameList(string code, string userId);

    /// <summary>
    /// 获取用户ID列表
    /// </summary>
    /// <param name="userId">关联的用户ID</param>
    /// <param name="values">Keys</param>
    /// <returns></returns>
    List<string> GetAuthUserIdList(string userId, List<string> values);

    /// <summary>
    /// 获取组织架构ID列表
    /// </summary>
    /// <param name="userId">关联的用户ID</param>
    /// <returns></returns>
    List<string> GetAuthOrganizationIdList(string userId);

    /// <summary>
    /// 获取用户规则列表
    /// </summary>
    /// <param name="userId"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    List<string> GetUserRulerList<T>(string userId);

    /// <summary>
    /// 获取基础数据权限下拉列表
    /// </summary>
    /// <returns></returns>
    IList<dynamic> GetBasicDataPermissionSelectList();

    /// <summary>
    /// 获取组织架构树形数据列表
    /// </summary>
    /// <returns></returns>
    IList<dynamic> GetOrganizationTreeList();
}