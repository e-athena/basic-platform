namespace Athena.Infrastructure.DataPermission.Extras;

/// <summary>
/// 扩展数据权限接口
/// </summary>
public interface IExtraDataPermission
{
    /// <summary>
    /// LABEL
    /// </summary>
    string Label { get; }

    /// <summary>
    /// KEY
    /// </summary>
    string Key { get; }

    /// <summary>
    /// 占位符
    /// </summary>
    string Value { get; }

    /// <summary>
    /// 获取查询SQL语句
    /// </summary>
    /// <remarks>Sql语句需要返回Id字段，返回其他可能会出异常或达不到查询预期</remarks>
    /// <returns></returns>
    string GetSqlString();
}