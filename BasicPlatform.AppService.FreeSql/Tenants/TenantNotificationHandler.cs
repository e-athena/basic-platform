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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="unitOfWorkManager"></param>
    /// <param name="accessor"></param>
    public TenantNotificationHandler(UnitOfWorkManager unitOfWorkManager,
        ISecurityContextAccessor accessor)
        : base(unitOfWorkManager, accessor)
    {
    }

    /// <summary>
    /// 更新租户资源
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [EventTracking]
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
    /// 添加租户子应用
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [EventTracking]
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
    /// 更新租户子应用
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    [EventTracking]
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