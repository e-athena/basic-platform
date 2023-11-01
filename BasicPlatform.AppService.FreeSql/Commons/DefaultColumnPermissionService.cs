using Athena.Infrastructure.ColumnPermissions;
using Athena.Infrastructure.ColumnPermissions.Models;
using BasicPlatform.Domain.Models.Roles;
using BasicPlatform.Domain.Models.Users;

namespace BasicPlatform.AppService.FreeSql.Commons;

/// <summary>
/// 
/// </summary>
[Component(LifeStyle.Singleton)]
public class DefaultColumnPermissionService : IColumnPermissionService
{
    private readonly IFreeSql _freeSql;
    private readonly ICacheManager? _cacheManager;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="freeSql"></param>
    public DefaultColumnPermissionService(IFreeSql freeSql)
    {
        _freeSql = freeSql;
        _cacheManager = AthenaProvider.Provider?.GetService(typeof(ICacheManager)) as ICacheManager;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<IList<ColumnPermission>?> GetAsync(string userId, Type type)
    {
        var typeName = type.Name;

        if (_cacheManager == null)
        {
            return await QueryFunc();
        }

        // Key
        var key = string.Format(CacheConstant.UserColumnPermissionKey, userId, typeName);
        // 过期时间
        var expireTime = TimeSpan.FromMinutes(30);

        return await _cacheManager.GetOrCreateAsync(key, QueryFunc, expireTime) ?? new List<ColumnPermission>();

        // 查询
        async Task<List<ColumnPermission>> QueryFunc()
        {
            // 读取用户的角色列权限
            var list = await _freeSql.Queryable<RoleColumnPermission>()
                .NoTracking()
                .Where(p => p.ColumnType == typeName)
                // 读取用户的角色
                .Where(p => _freeSql.Queryable<RoleUser>()
                    .NoTracking()
                    .As("c")
                    .Where(c => c.UserId == userId)
                    .Any(c => c.RoleId == p.RoleId)
                )
                .ToListAsync(p => new ColumnPermission
                {
                    Enabled = p.Enabled,
                    ColumnKey = p.ColumnKey,
                    IsEnableDataMask = p.IsEnableDataMask,
                    MaskLength = p.MaskLength,
                    MaskPosition = p.MaskPosition,
                    MaskChar = p.MaskChar
                });
            // 读取用户的数据权限
            var userPermissionList = await _freeSql.Queryable<UserColumnPermission>()
                .NoTracking()
                .Where(p => p.ColumnType == typeName)
                .Where(p => p.UserId == userId)
                // 读取未过期的
                .Where(p => p.ExpireAt == null || p.ExpireAt > DateTime.Now)
                .ToListAsync(p => new ColumnPermission
                {
                    Enabled = p.Enabled,
                    ColumnKey = p.ColumnKey,
                    IsEnableDataMask = p.IsEnableDataMask,
                    MaskLength = p.MaskLength,
                    MaskPosition = p.MaskPosition,
                    MaskChar = p.MaskChar
                });

            // 以用户的为准，因为可对用户进行个性化设置
            foreach (var item in userPermissionList)
            {
                // 查询
                var single = list
                    .FirstOrDefault(p => p.ColumnKey == item.ColumnKey);
                if (single == null)
                {
                    list.Add(item);
                    continue;
                }

                single.Enabled = item.Enabled;
                single.IsEnableDataMask = item.IsEnableDataMask;
                single.MaskLength = item.MaskLength;
                single.MaskPosition = item.MaskPosition;
                single.MaskChar = item.MaskChar;
            }

            // 去重
            list = list
                .GroupBy(p => p.ColumnKey)
                .Select(p => p.First())
                .ToList();

            return list;
        }
    }
}