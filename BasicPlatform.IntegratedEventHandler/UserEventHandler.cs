using BasicPlatform.Domain.Models.Users;
using BasicPlatform.Domain.Models.Users.Events;

namespace BasicPlatform.IntegratedEventHandler;

/// <summary>
/// 
/// </summary>
public class UserEventHandler : TenantQueryServiceBase<User>,
    IMessageHandler<UserCreatedEvent>
{
    private readonly ILogger<UserEventHandler> _logger;

    public UserEventHandler(
        FreeSqlMultiTenancy multiTenancy,
        ITenantService tenantService,
        ILoggerFactory loggerFactory) : base(multiTenancy, tenantService)
    {
        _logger = loggerFactory.CreateLogger<UserEventHandler>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="payload"></param>
    /// <param name="cancellationToken"></param>
    [IntegratedEventSubscribe(nameof(UserCreatedEvent))]
    public async Task HandleAsync(UserCreatedEvent payload, CancellationToken cancellationToken = default)
    {
        ChangeTenant(payload.TenantId, payload.AppId);
        var res = await QueryableNoTracking.Where(p => p.Id == payload.GetId())
            .FirstAsync(cancellationToken);

        Console.WriteLine(res?.PhoneNumber);

        _logger.LogError("TestAsync");
    }
}