using Athena.Infrastructure.FreeSql.Interfaces;
using BasicPlatform.Domain.Events.Users;

namespace BasicPlatform.AppService.FreeSql;

/// <summary>
/// 缓存通知处理器
/// </summary>
public class CacheNotificationHandler :
    IDomainEventHandler<UserUpdatedEvent>
{
    private readonly ICacheManager _cacheManager;

    public CacheNotificationHandler(ICacheManager cacheManager)
    {
        _cacheManager = cacheManager;
    }

    /// <summary>
    /// 用户更新成功事件
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="NotImplementedException"></exception>
    public async Task Handle(UserUpdatedEvent notification, CancellationToken cancellationToken)
    {
        // 匹配用户缓存
        var patternKey = string.Format(CacheConstant.UserCacheKeys, notification.Id);
        // 移除用户所有缓存
        await _cacheManager.RemovePatternAsync(patternKey, cancellationToken);
    }
}