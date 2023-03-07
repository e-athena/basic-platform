using BasicPlatform.WebAPI.Logging;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 
/// </summary>
public static class Extensions
{
    /// <summary>
    /// 添加日志
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    // ReSharper disable once IdentifierTypo
    public static IServiceCollection AddCustomLogging(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var isEnableLogging = configuration.GetSection("EnableLogging").Get<bool>();
        if (!isEnableLogging)
        {
            return services;
        }

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        return services;
    }
}