using BasicPlatform.Infrastructure.Tables;

namespace BasicPlatform.AppService.FreeSql;

/// <summary>
/// 通用服务接口实现类
/// </summary>
[Component]
public class CommonService : ICommonService
{
    /// <summary>
    /// 读取表格列信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IList<TableColumnInfo> GetColumns<T>() where T : class
    {
        return TableColumnReader.GetTableColumns(typeof(T));
    }
}