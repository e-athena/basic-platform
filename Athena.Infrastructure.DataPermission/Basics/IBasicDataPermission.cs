namespace Athena.Infrastructure.DataPermission.Basics;

/// <summary>
/// 基础数据权限接口
/// </summary>
public interface IBasicDataPermission
{
    /// <summary>
    /// LABEL
    /// </summary>
    string Label { get; }

    /// <summary>
    /// Value
    /// </summary>
    string Value { get; }

    /// <summary>
    /// 获取查询条件
    /// </summary>
    /// <param name="identityId">标识ID</param>
    /// <remarks>Sql语句需要返回Id字段，返回其他可能会出异常或达不到查询预期</remarks>
    /// <returns></returns>
    string GetSqlString(string identityId);
}