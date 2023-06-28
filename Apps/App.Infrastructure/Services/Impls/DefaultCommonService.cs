using Athena.Infrastructure.FrontEndTables;

namespace App.Infrastructure.Services.Impls;

/// <summary>
/// 通用服务接口实现类
/// </summary>
[Component]
public class DefaultCommonService : ICommonService
{
    private readonly IUserService _userService;

    public DefaultCommonService(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// 读取表格列信息
    /// </summary>
    /// <param name="appId">应用ID</param>
    /// <param name="userId">用户ID</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<GetTableColumnsResponse> GetColumnsAsync<T>(string appId, string? userId) where T : class
    {
        var moduleName = typeof(T).Name;
        var sources = TableColumnReader.GetTableColumns(typeof(T));
        // 读取用户保存的数据
        var userCustoms = await _userService.GetUserCustomColumnsAsync(
            appId,
            typeof(T).Name,
            userId
        );
        if (userCustoms.Count == 0)
        {
            return new GetTableColumnsResponse
            {
                ModuleName = moduleName,
                Columns = sources.OrderBy(p => p.Sort).ToList()
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
            Columns = sources.OrderBy(p => p.Sort).ToList()
        };
    }
}