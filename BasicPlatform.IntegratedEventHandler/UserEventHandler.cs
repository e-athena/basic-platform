using Athena.Infrastructure.FreeSql.Bases;
using BasicPlatform.Domain.Models;
using Microsoft.Extensions.Logging;

namespace BasicPlatform.IntegratedEventHandler;

/// <summary>
/// 
/// </summary>
public class UserEventHandler : QueryServiceBase<User>, IIntegratedEventHandler
{
    private readonly ILogger<UserEventHandler> _logger;

    public UserEventHandler(IFreeSql freeSql, ILoggerFactory loggerFactory) : base(freeSql)
    {
        _logger = loggerFactory.CreateLogger<UserEventHandler>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [IntegratedEventSubscribe(nameof(UserCreatedEvent))]
    public async Task TestAsync(UserCreatedEvent obj, CancellationToken cancellationToken = default)
    {
        var res = await QueryableNoTracking.Where(p => p.Id == obj.Id)
            .FirstAsync(cancellationToken);

        Console.WriteLine(res.PhoneNumber);

        _logger.LogError("TestAsync");
    }
}