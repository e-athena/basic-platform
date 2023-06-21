using BasicPlatform.Domain.Models.Users;
using BasicPlatform.Domain.Models.Users.Events;

namespace BasicPlatform.AppService.FreeSql.Users;

/// <summary>
/// 用户通知处理器
/// </summary>
public class UserNotificationHandler : AppServiceBase<User>,
    IDomainEventHandler<UserDataPermissionAssignedEvent>
{
    public UserNotificationHandler(UnitOfWorkManager unitOfWorkManager, ISecurityContextAccessor accessor)
        : base(unitOfWorkManager, accessor)
    {
    }

    /// <summary>
    /// 用户数据权限分配事件处理
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
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
}