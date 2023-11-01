using BasicPlatform.Domain.Models.Roles;
using BasicPlatform.Domain.Models.Roles.Events;
using BasicPlatform.Domain.Models.Users.Events;

namespace BasicPlatform.Domain.EventHandler;

/// <summary>
/// 缓存处理器
/// </summary>
public class CacheEventHandler :
    IDomainEventHandler<UserUpdatedEvent>,
    IDomainEventHandler<RoleDataPermissionAssignedEvent>,
    IDomainEventHandler<RoleColumnPermissionAssignedEvent>,
    IDomainEventHandler<UserDataPermissionAssignedEvent>,
    IDomainEventHandler<UserColumnPermissionAssignedEvent>
{
    private readonly ICacheManager _cacheManager;
    private readonly IFreeSql _freeSql;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cacheManager"></param>
    /// <param name="freeSql"></param>
    public CacheEventHandler(ICacheManager cacheManager, IFreeSql freeSql)
    {
        _cacheManager = cacheManager;
        _freeSql = freeSql;
    }

    /// <summary>
    /// 清理用户缓存
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="NotImplementedException"></exception>
    [EventTracking]
    public async Task Handle(UserUpdatedEvent notification, CancellationToken cancellationToken)
    {
        // 匹配用户缓存
        var patternKey = string.Format(CacheConstant.UserCacheKeys, notification.AggregateRootId);
        // 移除用户所有缓存
        await _cacheManager.RemovePatternAsync(patternKey, cancellationToken);
    }

    /// <summary>
    /// 清除角色用户数据权限相关的缓存
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [EventTracking]
    public async Task Handle(RoleDataPermissionAssignedEvent notification, CancellationToken cancellationToken)
    {
        // 读取角色用户
        var userIdList = await _freeSql.Queryable<RoleUser>()
            .Where(p => p.RoleId == notification.AggregateRootId)
            .ToListAsync(p => p.UserId, cancellationToken);

        if (userIdList.Count == 0)
        {
            return;
        }

        // 用户ID去重
        userIdList = userIdList.Distinct().ToList();

        // 移除用户数据权限相关的缓存
        foreach (var userId in userIdList)
        {
            var patternKey2 = string.Format(CacheConstant.UserPolicyQueryPatternKey, userId);
            await _cacheManager.RemovePatternAsync(patternKey2, cancellationToken);
            var patternKey = string.Format(CacheConstant.UserPolicyFilterGroupQueryPatternKey, userId);
            await _cacheManager.RemovePatternAsync(patternKey, cancellationToken);
        }
    }

    /// <summary>
    /// 清除角色用户列权限相关的缓存
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [EventTracking]
    public async Task Handle(RoleColumnPermissionAssignedEvent notification, CancellationToken cancellationToken)
    {
        // 读取角色用户
        var userIdList = await _freeSql.Queryable<RoleUser>()
            .Where(p => p.RoleId == notification.AggregateRootId)
            .ToListAsync(p => p.UserId, cancellationToken);

        if (userIdList.Count == 0)
        {
            return;
        }

        // 用户ID去重
        userIdList = userIdList.Distinct().ToList();

        // 移除用户数据权限相关的缓存
        foreach (var userId in userIdList)
        {
            var patternKey = string.Format(CacheConstant.UserColumnPermissionPatternKey, userId);
            await _cacheManager.RemovePatternAsync(patternKey, cancellationToken);
        }
    }

    /// <summary>
    /// 清除用户数据权限相关的缓存
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [EventTracking]
    public async Task Handle(UserDataPermissionAssignedEvent notification, CancellationToken cancellationToken)
    {
        var userId = notification.AggregateRootId;
        var patternKey2 = string.Format(CacheConstant.UserPolicyQueryPatternKey, userId);
        await _cacheManager.RemovePatternAsync(patternKey2, cancellationToken);
        var patternKey = string.Format(CacheConstant.UserPolicyFilterGroupQueryPatternKey, userId);
        await _cacheManager.RemovePatternAsync(patternKey, cancellationToken);
    }

    /// <summary>
    /// 清除用户数据权限相关的缓存
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [EventTracking]
    public async Task Handle(UserColumnPermissionAssignedEvent notification, CancellationToken cancellationToken)
    {
        var userId = notification.AggregateRootId;
        var patternKey = string.Format(CacheConstant.UserColumnPermissionPatternKey, userId);
        await _cacheManager.RemovePatternAsync(patternKey, cancellationToken);
    }
}