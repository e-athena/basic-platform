using BasicPlatform.Domain.Models.Tenants;
using BasicPlatform.Domain.Models.Users.Events;

namespace BasicPlatform.ProcessManager;

/// <summary>
/// 租户流程管理器
/// </summary>
public class TenantProcessManager : TenantServiceBase<Tenant>,
    IMessageHandler<UserCreatedEvent>
{
    private const string TopicGroup = "tenant.process.manager.group";

    public TenantProcessManager(UnitOfWorkManagerCloud cloud, ITenantService tenantService, ILoggerFactory factory) :
        base(cloud, tenantService, factory)
    {
    }

    /// <summary>
    /// 用户创建成功
    /// </summary>
    /// <param name="payload"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [IntegratedEventSubscribe(nameof(UserCreatedEvent), Group = TopicGroup)]
    public async Task HandleAsync(UserCreatedEvent payload, CancellationToken cancellationToken)
    {
        // 不是创建租户超级管理员，不处理
        if (!payload.IsTenantAdmin || string.IsNullOrEmpty(payload.TenantId))
        {
            return;
        }

        // 事务处理
        await UseTransactionAsync(string.Empty, null, async () =>
        {
            // 读取租户信息
            var entity = await Queryable
                .Where(p => p.Code == payload.TenantId)
                .FirstAsync(cancellationToken);

            if (entity == null)
            {
                throw FriendlyException.Of("找不到租户信息");
            }

            entity.InitDatabase(payload.CreatedUserId);
            await RegisterDirtyAsync(entity, cancellationToken);
        });
    }
}