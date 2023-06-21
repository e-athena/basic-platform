using BasicPlatform.Domain.Models.Tenants;
using BasicPlatform.Domain.Models.Tenants.Events;

namespace BasicPlatform.AppService.FreeSql.Tenants;

/// <summary>
/// 租户通知处理器
/// </summary>
public class TenantNotificationHandler : AppServiceBase<Tenant>,
    IDomainEventHandler<TenantResourceAssignedEvent>,
    IDomainEventHandler<TenantCreatedEvent>,
    IDomainEventHandler<TenantUpdatedEvent>
{
    public TenantNotificationHandler(UnitOfWorkManager unitOfWorkManager,
        ISecurityContextAccessor accessor)
        : base(unitOfWorkManager, accessor)
    {
    }

    /// <summary>
    /// 租户资源分配事件处理
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task Handle(TenantResourceAssignedEvent notification, CancellationToken cancellationToken)
    {
        // 删除旧数据
        await RegisterDeleteValueObjectAsync<TenantResource>(
            p => p.TenantId == notification.GetId(), cancellationToken
        );
        if (notification.Resources.Count == 0)
        {
            return;
        }

        // 新增新数据
        await RegisterNewRangeValueObjectAsync(notification.Resources, cancellationToken);
    }

    /// <summary>
    /// 租户创建事件处理
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task Handle(TenantCreatedEvent notification, CancellationToken cancellationToken)
    {
        if (notification.Applications.Count == 0)
        {
            return;
        }

        // 新增新数据
        await RegisterNewRangeValueObjectAsync(notification.Applications, cancellationToken);
    }

    /// <summary>
    /// 租户更新事件处理
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    public async Task Handle(TenantUpdatedEvent notification, CancellationToken cancellationToken)
    {
        // 删除旧数据
        await RegisterDeleteValueObjectAsync<TenantApplication>(
            p => p.TenantId == notification.GetId(), cancellationToken
        );
        if (notification.Applications.Count == 0)
        {
            return;
        }

        // 新增新数据
        await RegisterNewRangeValueObjectAsync(notification.Applications, cancellationToken);
    }
}