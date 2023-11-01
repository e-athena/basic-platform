using BasicPlatform.AppService.TableColumns;
using BasicPlatform.AppService.Users;
using BasicPlatform.AppService.Users.Models;

namespace BasicPlatform.AppService.FreeSql.Commons;

/// <summary>
/// 通用服务接口实现类
/// </summary>
[Component]
public class CommonService : ICommonService
{
    private readonly IUserQueryService _userQueryService;
    private readonly ISecurityContextAccessor _accessor;
    private readonly ICacheManager _cacheManager;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userQueryService"></param>
    /// <param name="accessor"></param>
    /// <param name="cacheManager"></param>
    public CommonService(IUserQueryService userQueryService, ISecurityContextAccessor accessor,
        ICacheManager cacheManager)
    {
        _userQueryService = userQueryService;
        _accessor = accessor;
        _cacheManager = cacheManager;
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
    private async Task<List<TableColumnInfo>> ColumnAuthAsync(
        string moduleName,
        IList<TableColumnInfo> sources
    )
    {
        // 添加缓存
        // Key
        var key = string.Format(CacheConstant.UserTableColumnPermissionKey, _accessor.UserId, moduleName);
        // 过期时间
        var expireTime = TimeSpan.FromMinutes(1);
        var columns = await _cacheManager.GetOrCreateAsync(key, QueryFunc, expireTime) ??
                      new List<UserColumnPermissionModel>();

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

        // 查询
        Task<List<UserColumnPermissionModel>> QueryFunc()
        {
            // 读取
            return _userQueryService.GetColumnPermissionsByModuleNameAsync(moduleName, _accessor.UserId);
        }
    }
}