using BasicPlatform.AppService.TableColumns;
using BasicPlatform.AppService.Users;

namespace BasicPlatform.AppService.FreeSql;

/// <summary>
/// 通用服务接口实现类
/// </summary>
[Component]
public class CommonService : ICommonService
{
    private readonly IUserQueryService _userQueryService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userQueryService"></param>
    public CommonService(IUserQueryService userQueryService)
    {
        _userQueryService = userQueryService;
    }

    /// <summary>
    /// 读取表格列信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<GetTableColumnsResponse> GetColumnsAsync<T>() where T : class
    {
        var moduleName = typeof(T).Name;
        var sources = TableColumnReader.GetTableColumns(typeof(T));
        // 读取用户保存的数据
        var userCustoms = await _userQueryService.GetCurrentUserCustomColumnsAsync(typeof(T).Name);
        if (userCustoms.Count == 0)
        {
            return new GetTableColumnsResponse
            {
                ModuleName = moduleName,
                Columns = await ColumnAuthAsync(moduleName, sources)
            };
        }

        // 合并数据,以用户的为主
        foreach (var source in sources)
        {
            var item = userCustoms.FirstOrDefault(p => p.DataIndex == source.DataIndex);
            if (item == null)
            {
                continue;
            }

            source.DataIndex = item.DataIndex;
            source.Width = item.Width;
            source.HideInTable = !item.Show;
            source.Fixed = item.Fixed;
            source.Sort = item.Sort;
        }

        return new GetTableColumnsResponse
        {
            ModuleName = moduleName,
            Columns = await ColumnAuthAsync(moduleName, sources)
        };
    }

    /// <summary>
    /// 列权限处理
    /// </summary>
    /// <param name="moduleName"></param>
    /// <param name="sources"></param>
    /// <returns></returns>
    private async Task<List<TableColumnInfo>> ColumnAuthAsync(string moduleName, IEnumerable<TableColumnInfo> sources)
    {
        // 处理列权限
        var columns = await _userQueryService.GetColumnPermissionsByModuleNameAsync(moduleName);
        if (columns.Count == 0)
        {
            return sources.OrderBy(p => p.Sort).ToList();
        }

        var news = new List<TableColumnInfo>();
        foreach (var info in sources)
        {
            var item = columns.FirstOrDefault(p => p.ColumnKey == info.PropertyName);
            // 如果为空或者启用则代表有权限查看
            if (item == null || item.Enabled)
            {
                news.Add(info);
            }
        }

        return news;
    }
}