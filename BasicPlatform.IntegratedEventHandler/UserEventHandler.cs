namespace BasicPlatform.IntegratedEventHandler;

/// <summary>
/// 
/// </summary>
public class UserEventHandler : IIntegratedEventHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    [IntegratedEventSubscribe(nameof(UserCreatedEvent))]
    public Task TestAsync(UserCreatedEvent obj)
    {
        Console.WriteLine(obj.UserName);
        return Task.CompletedTask;
    }
}