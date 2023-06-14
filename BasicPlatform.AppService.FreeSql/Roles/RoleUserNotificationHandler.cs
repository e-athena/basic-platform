using BasicPlatform.Domain.Models.Roles;
using BasicPlatform.Domain.Models.Roles.Events;

namespace BasicPlatform.AppService.FreeSql.Roles;

/// <summary>
/// 角色用户通知处理器
/// </summary>
public class RoleUserNotificationHandler : AppServiceBase<Role>,
    IDomainEventHandler<RoleUserAssignedEvent>
{
    public RoleUserNotificationHandler(UnitOfWorkManager unitOfWorkManager, ISecurityContextAccessor accessor)
        : base(unitOfWorkManager, accessor)
    {
    }

    /// <summary>
    /// 角色用户分配事件处理
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task Handle(RoleUserAssignedEvent notification, CancellationToken cancellationToken)
    {
        // 删除旧数据
        await RegisterDeleteValueObjectAsync<RoleUser>(
            p => p.RoleId == notification.GetId(), cancellationToken
        );
        if (notification.UserIds.Count <= 0)
        {
            return;
        }

        // 新增新数据
        var roleResources = notification
            .UserIds
            .Select(userId => new RoleUser(notification.GetId()!, userId))
            .ToList();
        // 新增新数据
        await RegisterNewRangeValueObjectAsync(roleResources, cancellationToken);
    }
}