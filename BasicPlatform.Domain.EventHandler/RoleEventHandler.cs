using BasicPlatform.Domain.Models.Roles;
using BasicPlatform.Domain.Models.Roles.Events;

namespace BasicPlatform.Domain.EventHandler;

/// <summary>
/// 角色通知处理器
/// </summary>
public class RoleEventHandler : ServiceBase<Role>,
    IDomainEventHandler<RoleResourceAssignedEvent>,
    IDomainEventHandler<RoleDataPermissionAssignedEvent>,
    IDomainEventHandler<RoleColumnPermissionAssignedEvent>,
    IDomainEventHandler<RoleUserAssignedEvent>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="unitOfWorkManager"></param>
    /// <param name="accessor"></param>
    public RoleEventHandler(UnitOfWorkManager unitOfWorkManager,
        ISecurityContextAccessor accessor)
        :
        base(unitOfWorkManager, accessor)
    {
    }

    /// <summary>
    /// 更新角色资源
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
            p => p.RoleId == notification.AggregateRootId, cancellationToken
        );
        if (notification.Resources.Count == 0)
        {
            return;
        }

        // 新增新数据
        await RegisterNewRangeValueObjectAsync(notification.Resources, cancellationToken);
    }

    /// <summary>
    /// 更新角色数据权限
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
            p => p.RoleId == notification.AggregateRootId, cancellationToken
        );
        if (notification.Permissions.Count <= 0)
        {
            return;
        }

        // 新增新数据
        await RegisterNewRangeValueObjectAsync(notification.Permissions, cancellationToken);
    }

    /// <summary>
    /// 更新角色列权限
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [EventTracking]
    public async Task Handle(RoleColumnPermissionAssignedEvent notification, CancellationToken cancellationToken)
    {
        // 删除旧数据
        await RegisterDeleteValueObjectAsync<RoleColumnPermission>(
            p => p.RoleId == notification.AggregateRootId, cancellationToken
        );
        if (notification.Permissions.Count <= 0)
        {
            return;
        }

        // 新增新数据
        await RegisterNewRangeValueObjectAsync(notification.Permissions, cancellationToken);
    }

    /// <summary>
    /// 更新角色用户
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
            p => p.RoleId == notification.AggregateRootId, cancellationToken
        );
        if (notification.UserIds.Count <= 0)
        {
            return;
        }

        // 新增新数据
        var roleResources = notification
            .UserIds
            .Select(userId => new RoleUser(notification.AggregateRootId, userId))
            .ToList();
        // 新增新数据
        await RegisterNewRangeValueObjectAsync(roleResources, cancellationToken);
    }
}