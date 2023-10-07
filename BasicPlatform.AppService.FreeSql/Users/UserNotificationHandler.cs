using BasicPlatform.Domain.Models.Users;
using BasicPlatform.Domain.Models.Users.Events;

namespace BasicPlatform.AppService.FreeSql.Users;

/// <summary>
/// 用户通知处理器
/// </summary>
public class UserNotificationHandler : ServiceBase<User>,
    IDomainEventHandler<UserDataPermissionAssignedEvent>,
    IDomainEventHandler<UserColumnPermissionAssignedEvent>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="unitOfWorkManager"></param>
    /// <param name="accessor"></param>
    public UserNotificationHandler(UnitOfWorkManager unitOfWorkManager, ISecurityContextAccessor accessor)
        : base(unitOfWorkManager, accessor)
    {
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
            p => p.UserId == notification.GetId(), cancellationToken
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
            p => p.UserId == notification.GetId(), cancellationToken
        );
        if (notification.Permissions.Count <= 0)
        {
            return;
        }

        // 新增新数据
        await RegisterNewRangeValueObjectAsync(notification.Permissions, cancellationToken);
    }
}