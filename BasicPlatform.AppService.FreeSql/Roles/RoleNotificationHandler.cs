using BasicPlatform.Domain.Models.Roles;
using BasicPlatform.Domain.Models.Roles.Events;

namespace BasicPlatform.AppService.FreeSql.Roles;

/// <summary>
/// 角色通知处理器
/// </summary>
public class RoleNotificationHandler : AppServiceBase<Role>,
    IDomainEventHandler<RoleResourceAssignedEvent>,
    IDomainEventHandler<RoleDataPermissionAssignedEvent>,
    IDomainEventHandler<RoleUserAssignedEvent>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="unitOfWorkManager"></param>
    /// <param name="accessor"></param>
    public RoleNotificationHandler(UnitOfWorkManager unitOfWorkManager,
        ISecurityContextAccessor accessor)
        : base(unitOfWorkManager, accessor)
    {
    }

    /// <summary>
    /// 角色资源分配事件处理
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [EventTracking]
    public async Task Handle(RoleResourceAssignedEvent notification, CancellationToken cancellationToken)
    {
        // 删除旧数据
        await RegisterDeleteValueObjectAsync<RoleResource>(
            p => p.RoleId == notification.GetId(), cancellationToken
        );
        if (notification.Resources.Count == 0)
        {
            return;
        }

        // 新增新数据
        await RegisterNewRangeValueObjectAsync(notification.Resources, cancellationToken);
    }

    /// <summary>
    /// 角色数据权限分配事件处理
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [EventTracking]
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

    /// <summary>
    /// 角色用户分配事件处理
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [EventTracking]
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