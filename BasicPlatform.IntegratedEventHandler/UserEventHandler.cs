using BasicPlatform.Domain.Models.Users;

namespace BasicPlatform.IntegratedEventHandler;

/// <summary>
/// 用户事件处理
/// </summary>
public class UserEventHandler : TenantQueryServiceBase<User>
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
}