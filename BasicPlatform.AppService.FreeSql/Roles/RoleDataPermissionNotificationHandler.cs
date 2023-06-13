using BasicPlatform.Domain.Models.Roles;
using BasicPlatform.Domain.Models.Roles.Events;

namespace BasicPlatform.AppService.FreeSql.Roles;

/// <summary>
/// 角色数据权限通知处理器
/// </summary>
public class RoleDataPermissionNotificationHandler : AppServiceBase<Role>,
    IDomainEventHandler<RoleDataPermissionAssignedEvent>
{
    public RoleDataPermissionNotificationHandler(UnitOfWorkManager unitOfWorkManager, ISecurityContextAccessor accessor)
        : base(unitOfWorkManager, accessor)
    {
    }

    /// <summary>
    /// 角色数据权限分配事件处理
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task Handle(RoleDataPermissionAssignedEvent notification, CancellationToken cancellationToken)
    {
        // 删除旧数据
        await RegisterDeleteValueObjectAsync<RoleDataPermission>(
            p => p.RoleId == notification.GetId(), cancellationToken
        );
        if (notification.Permissions.Count <= 0)
        {
            return;
        }

        // 新增新数据
        await RegisterNewRangeValueObjectAsync(notification.Permissions, cancellationToken);
    }
}