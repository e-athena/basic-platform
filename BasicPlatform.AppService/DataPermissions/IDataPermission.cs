namespace BasicPlatform.AppService.DataPermissions;

/// <summary>
/// 数据权限接口
/// </summary>
public interface IDataPermission
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
    /// <remarks>Sql语句需要返回Id,MapKey字段，返回其他可能会出异常或达不到查询预期</remarks>
    /// <example>【SELECT Id,MapKey FROM table1 WHERE UserId=@UserId】</example>
    /// <example>假设Key和Value值分别为：ProjectId、{ProjectId}，则对应的SQL语句应为：SELECT a.`Id`,'ProjectId,{ProjectId}' `MapKey` FROM `projects` a WHERE a.`UserId`='640557fc46a0d9b4453abb80'</example>
    /// <returns></returns>
    string GetSqlString();
}