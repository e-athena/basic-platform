SelfLog.Enable(Console.Error);

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;
var host = builder.Host;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
// Add services to the container.
services.AddAthenaProvider();
services.AddCustomOpenTelemetry<Program>(configuration);
services.AddCustomMediatR(
    Assembly.Load("BasicPlatform.AppService.FreeSql")
);
services.AddCustomServiceComponent(
    Assembly.GetExecutingAssembly(),
    Assembly.Load("BasicPlatform.AppService.FreeSql"),
    Assembly.Load("BasicPlatform.Infrastructure")
);
services.AddCustomSwaggerGen(configuration);
// 添加ORM
services.AddCustomFreeSql(configuration, builder.Environment.IsDevelopment());
// 添加集成事件支持
services.AddCustomIntegrationEvent(configuration, capOptions =>
{
    // Dashboard
    capOptions.UseDashboard();
}, new[]
{
    Assembly.Load("BasicPlatform.ProcessManager"),
});

services.AddCustomCsRedisCache(configuration);
services.AddCustomApiPermission();
services.AddCustomDataPermission(configuration);
services.AddCustomBasicAuth(configuration);
services.AddCustomJwtAuthWithSignalR(configuration);
services.AddCustomSignalRWithRedis(configuration);
services.AddCustomCors(configuration);
services.AddCustomStorageLogger(configuration);
services.AddCustomEventTracking(configuration);
services.AddCustomController().AddNewtonsoftJson();

host.ConfigureLogging((_, loggingBuilder) => loggingBuilder.ClearProviders())
    .UseSerilog((ctx, cfg) =>
        cfg.ReadFrom.Configuration(ctx.Configuration)
    )
    .UseDefaultServiceProvider(options => { options.ValidateScopes = false; });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCustomSwagger();
}

app.UseAthenaProvider();
app.UseCustomStaticFiles();
app.UseCors();
//启用验证
app.UseAuthentication();
app.UseAuthorization();
app.UseCustomAuditLog();
app.UseCustomFreeSqlMultiTenancy();
app.MapControllers();
app.MapCustomSignalR();
app.MapSpaFront<Program>();
app.MapHealth();
// run
app.Run();