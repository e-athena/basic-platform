using BasicPlatform.Infrastructure.Tables;

namespace BasicPlatform.AppService;

/// <summary>
/// 通用服务接口
/// </summary>
public interface ICommonService
{
    /// <summary>
    /// 读取表格列信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    IList<TableColumnInfo> GetColumns<T>() where T : class;
}