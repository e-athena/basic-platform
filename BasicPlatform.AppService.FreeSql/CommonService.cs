using BasicPlatform.AppService.Users;
using BasicPlatform.Infrastructure.Tables;

namespace BasicPlatform.AppService.FreeSql;

/// <summary>
/// 通用服务接口实现类
/// </summary>
[Component]
public class CommonService : ICommonService
{
    private readonly IUserQueryService _userQueryService;

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
    public async Task<IList<TableColumnInfo>> GetColumnsAsync<T>() where T : class
    {
        var sources = TableColumnReader.GetTableColumns(typeof(T));
        // 读取用户保存的数据
        var userCustoms = await _userQueryService.GetCurrentUserCustomColumnsAsync(typeof(T).Name);
        if (userCustoms.Count == 0)
        {
            return sources;
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
            source.Show = item.Show;
            source.Fixed = item.Fixed;
            source.Sort = item.Sort;
        }

        return sources.OrderBy(p => p.Sort).ToList();
    }
}