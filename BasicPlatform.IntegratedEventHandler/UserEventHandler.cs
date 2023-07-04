using Athena.Infrastructure.EventTracking.Aop;
using BasicPlatform.Domain.Models.Users;
using BasicPlatform.Domain.Models.Users.Events;

namespace BasicPlatform.IntegratedEventHandler;

/// <summary>
/// 用户事件处理
/// </summary>
public class UserEventHandler : TenantQueryServiceBase<User>,
    IMessageHandler<UserCreatedEvent>
{
    private readonly ILogger<UserEventHandler> _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="multiTenancy"></param>
    /// <param name="tenantService"></param>
    /// <param name="loggerFactory"></param>
    public UserEventHandler(
        FreeSqlMultiTenancy multiTenancy,
        ITenantService tenantService,
        ILoggerFactory loggerFactory) : base(multiTenancy, tenantService)
    {
        _logger = loggerFactory.CreateLogger<UserEventHandler>();
    }

    /// <summary>
    /// 用户创建成功集成事件处理
    /// </summary>
    /// <param name="payload"></param>
    /// <param name="cancellationToken"></param>
    [EventTracking]
    [IntegratedEventSubscribe(nameof(UserCreatedEvent))]
    public async Task HandleAsync(UserCreatedEvent payload, CancellationToken cancellationToken = default)
    {
        ChangeTenant(payload.TenantId, payload.AppId);
        var res = await QueryableNoTracking.Where(p => p.Id == payload.GetId())
            .FirstAsync(cancellationToken);

        Console.WriteLine(res?.PhoneNumber);

        _logger.LogError("TestAsync");
        // throw new Exception("模拟报错");
    }
}