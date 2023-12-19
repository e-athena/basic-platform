using BasicPlatform.Domain.Models.Users;
using BasicPlatform.Domain.Models.Users.Events;

namespace BasicPlatform.Domain.EventHandler;

/// <summary>
/// 用户通知处理器
/// </summary>
public class UserEventHandler : ServiceBase<User>,
    IDomainEventHandler<UserResourceAssignedEvent>,
    IDomainEventHandler<UserDataPermissionAssignedEvent>,
    IDomainEventHandler<UserColumnPermissionAssignedEvent>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="unitOfWorkManager"></param>
    /// <param name="accessor"></param>
    public UserEventHandler(UnitOfWorkManager unitOfWorkManager, ISecurityContextAccessor accessor)
        : base(unitOfWorkManager, accessor)
    {
    }

    /// <summary>
    /// 更新用户资源权限
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [EventTracking]
    public async Task Handle(UserResourceAssignedEvent notification, CancellationToken cancellationToken)
    {
        // 删除旧数据
        await RegisterDeleteValueObjectAsync<UserResource>(
            p => p.UserId == notification.AggregateRootId, cancellationToken
        );
        if (notification.Resources.Count <= 0)
        {
            return;
        }
        await RegisterNewRangeValueObjectAsync(notification.Resources, cancellationToken);
    }

    /// <summary>
    /// 更新用户数据权限
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [EventTracking]
    public async Task Handle(UserDataPermissionAssignedEvent notification, CancellationToken cancellationToken)
    {
        // 删除旧数据
        await RegisterDeleteValueObjectAsync<UserDataPermission>(
            p => p.UserId == notification.AggregateRootId, cancellationToken
        );
        if (notification.Permissions.Count <= 0)
        {
            return;
        }

        // 新增新数据
        await RegisterNewRangeValueObjectAsync(notification.Permissions, cancellationToken);
    }

    /// <summary>
    /// 更新用户列权限
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [EventTracking]
    public async Task Handle(UserColumnPermissionAssignedEvent notification, CancellationToken cancellationToken)
    {
        // 删除旧数据
        await RegisterDeleteValueObjectAsync<UserColumnPermission>(
            p => p.UserId == notification.AggregateRootId, cancellationToken
        );
        if (notification.Permissions.Count <= 0)
        {
            return;
        }

        // 新增新数据
        await RegisterNewRangeValueObjectAsync(notification.Permissions, cancellationToken);
    }
}