SelfLog.Enable(Console.Error);

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

builder.Host
    .ConfigureLogging((_, loggingBuilder) => loggingBuilder.ClearProviders())
    .UseSerilog((ctx, cfg) =>
        cfg.ReadFrom.Configuration(ctx.Configuration)
    );

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
// Add services to the container.
services.AddHttpContextAccessor();
services.AddCustomOpenTelemetry<Program>(configuration);
services.AddCustomMediatR(
    Assembly.Load("BasicPlatform.AppService.FreeSql")
);
services.AddCustomServiceComponent(
    Assembly.Load("BasicPlatform.AppService.FreeSql"),
    Assembly.Load("BasicPlatform.Infrastructure")
);
services.AddCustomSwaggerGen(configuration);
services.AddCustomFreeSql(configuration, builder.Environment);

// #region 使用SQLite作为集成事件存储，用于开发环境
//
// var connectionString = configuration.GetConnectionString("Default");
// services.AddCustomIntegrationEvent(options =>
// {
//     options.UseSqlite(connectionString);
//     options.UseRedis();
//     options.UseDashboard();
// }, new[] {Assembly.Load("BasicPlatform.IntegratedEventHandler")});
//
// #endregion

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
services.AddCustomStorageLogger(configuration, FreeSqlMultiTenancyManager.Instance);
services.AddControllers(options =>
{
    options.AddCustomApiResultFilter();
    options.AddCustomApiExceptionFilter();
}).AddNewtonsoftJson();

builder.Host.UseDefaultServiceProvider(options => { options.ValidateScopes = false; });
var app = builder.Build();

app.RegisterCustomServiceInstance();
app.UseCustomFreeSqlMultiTenancy();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCustomSwagger();
}

app.UseStaticFiles();
app.UseCors();
//启用验证
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCustomAuditLog();
app.MapCustomSignalR();
app.MapGet("/", context =>
{
    context.Response.Redirect("/index.html");
    return Task.CompletedTask;
});
app.MapGet("/health", context => context.Response.WriteAsync("ok"));
app.Run();