namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 
/// </summary>
public class AthenaServiceOptions
{
    /// <summary>
    /// 
    /// </summary>
    public List<Assembly>? MediatRAssemblies { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<Assembly>? ServiceComponentAssemblies { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// 
    /// </summary>
    public IHostEnvironment Environment { get; }

    /// <summary>
    /// 
    /// </summary>
    public ConfigureHostBuilder Host { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="environment"></param>
    /// <param name="host"></param>
    public AthenaServiceOptions(IConfiguration configuration, IHostEnvironment environment, ConfigureHostBuilder host)
    {
        Configuration = configuration;
        Environment = environment;
        Host = host;
    }
}

/// <summary>
/// 
/// </summary>
public static class AthenaServiceExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="serviceOptions"></param>
    /// <returns></returns>
    public static IServiceCollection AddAthena(
        this IServiceCollection services,
        AthenaServiceOptions serviceOptions)
    {
        var configuration = serviceOptions.Configuration;
        var environment = serviceOptions.Environment;
        var host = serviceOptions.Host;
        host.ConfigureLogging((_, loggingBuilder) => loggingBuilder.ClearProviders())
            .UseSerilog((ctx, cfg) =>
                cfg.ReadFrom.Configuration(ctx.Configuration)
            )
            .UseDefaultServiceProvider(options => { options.ValidateScopes = false; });
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        // Add services to the container.
        services.AddAthenaProvider();
        services.AddCustomOpenTelemetry<Program>(configuration);

        #region 添加MediatR服务

        if (serviceOptions.MediatRAssemblies != null)
        {
            serviceOptions.MediatRAssemblies.Add(Assembly.Load("BasicPlatform.AppService.FreeSql"));
        }
        else
        {
            serviceOptions.MediatRAssemblies = new List<Assembly> {Assembly.Load("BasicPlatform.AppService.FreeSql")};
        }

        // 添加MediatR服务
        services.AddCustomMediatR(serviceOptions.MediatRAssemblies.ToArray());

        #endregion

        #region 添加服务组件

        if (serviceOptions.ServiceComponentAssemblies != null)
        {
            serviceOptions.ServiceComponentAssemblies.Add(Assembly.Load("BasicPlatform.AppService.FreeSql"));
            serviceOptions.ServiceComponentAssemblies.Add(Assembly.Load("BasicPlatform.Infrastructure"));
        }
        else
        {
            serviceOptions.ServiceComponentAssemblies = new List<Assembly>
            {
                Assembly.Load("BasicPlatform.AppService.FreeSql"),
                Assembly.Load("BasicPlatform.Infrastructure")
            };
        }

        // 添加服务组件
        services.AddCustomServiceComponent(serviceOptions.ServiceComponentAssemblies.ToArray());

        #endregion

        services.AddCustomSwaggerGen(configuration);
        services.AddCustomFreeSql(configuration, environment.IsDevelopment());
        // 添加集成事件支持
        services.AddCustomIntegrationEvent(configuration, capOptions =>
        {
            // Dashboard
            capOptions.UseDashboard();
        }, new[] {Assembly.Load("BasicPlatform.IntegratedEventHandler")});

        services.AddCustomCsRedisCache(configuration);
        services.AddCustomApiPermission();
        services.AddCustomJwtAuthWithSignalR(configuration);
        services.AddCustomSignalRWithRedis(configuration);
        services.AddCustomCors(configuration);
        services.AddCustomStorageLogger(configuration);
        services.AddCustomController().AddNewtonsoftJson();
        return services;
    }
}