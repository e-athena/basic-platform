using CRM.Domain.Customers.Events;

namespace CRM.ProcessManagers;

public class TestProcessManager :
    IMessageHandler<CustomerCreatedEvent>
{
    private const string TopicGroup = "test.process.manager.group";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="payload"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [IntegratedEventSubscribe(nameof(CustomerCreatedEvent), Group = TopicGroup)]
    public Task HandleAsync(CustomerCreatedEvent payload, CancellationToken cancellationToken)
    {
        Console.WriteLine(JsonConvert.SerializeObject(payload));
        return Task.CompletedTask;
    }
}