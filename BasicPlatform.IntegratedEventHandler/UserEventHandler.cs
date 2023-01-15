using System.Text.Json;
using Athena.Infrastructure.FreeSql.Bases;
using BasicPlatform.Domain.Models;

namespace BasicPlatform.IntegratedEventHandler;

/// <summary>
/// 
/// </summary>
public class UserEventHandler : QueryServiceBase<User>, IIntegratedEventHandler
{
    public UserEventHandler(IFreeSql freeSql) : base(freeSql)
    {
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
    }
}