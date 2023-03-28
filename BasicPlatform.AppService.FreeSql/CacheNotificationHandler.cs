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
        // 移除用户缓存

        #region 数据权限相关

        var key1 = string.Format(CacheConstant.UserDataScopesKey, notification.Id);
        var key2 = string.Format(CacheConstant.UserOrganizationKey, notification.Id);
        var key3 = string.Format(CacheConstant.UserOrganizationsKey, notification.Id);
        await _cacheManager.RemoveAsync(key1, cancellationToken);
        await _cacheManager.RemoveAsync(key2, cancellationToken);
        await _cacheManager.RemoveAsync(key3, cancellationToken);

        #endregion
    }
}