using BasicPlatform.AppService.Tenants.Requests;
using BasicPlatform.Domain.Models.Users.Events;

namespace BasicPlatform.ProcessManager;

/// <summary>
/// 租户流程管理器
/// </summary>
public class TenantProcessManager :
    IMessageHandler<UserCreatedEvent>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mediator"></param>
    public TenantProcessManager(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// 将租户设置为已初始化完成
    /// </summary>
    /// <param name="payload"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [EventTracking]
    [IntegratedEventSubscribe(nameof(UserCreatedEvent), Group = nameof(TenantProcessManager))]
    public async Task HandleAsync(UserCreatedEvent payload, CancellationToken cancellationToken)
    {
        // 不是创建租户超级管理员，不处理
        if (!payload.IsTenantAdmin || string.IsNullOrEmpty(payload.TenantId))
        {
            return;
        }

        await _mediator.SendAsync(new InitTenantRequest
        {
            Code = payload.TenantId,
            UserId = payload.CreatedUserId,
            RootTraceId = payload.RootTraceId
        }, cancellationToken: cancellationToken);
    }
}