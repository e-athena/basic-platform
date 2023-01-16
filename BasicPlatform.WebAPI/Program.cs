SelfLog.Enable(Console.Error);

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

builder.Host
    .ConfigureLogging((_, loggingBuilder) => loggingBuilder.ClearProviders())
    .UseSerilog((ctx, cfg) =>
        cfg.ReadFrom.Configuration(ctx.Configuration)
    );

// Add services to the container.
services.AddHttpContextAccessor();
services.AddCustomMediatR(
    Assembly.Load("BasicPlatform.AppService.FreeSql")
);
services.AddCustomServiceComponent(
    Assembly.Load("BasicPlatform.AppService.FreeSql"),
    Assembly.Load("BasicPlatform.Infrastructure")
);
services.AddCustomSwaggerGen(configuration);
services.AddCustomFreeSqlWithMySql(configuration, builder.Environment, aop =>
{
    aop.CurdAfter += (_, e) =>
    {
        if (e.ElapsedMilliseconds > 200)
        {
            Console.WriteLine($"执行SQL耗时 {e.ElapsedMilliseconds} ms");
        }
    };
});
services.AddCustomIntegrationEvent(configuration, opts =>
{
    // Dashboard
    opts.UseDashboard();
});
services.AddCustomIntegrationEventHandler(
    Assembly.Load("BasicPlatform.IntegratedEventHandler")
);
services.AddCustomJwtAuth(configuration);
services.AddCustomCors(configuration);
services.AddControllers(options =>
{
    options.AddCustomApiResultFilter();
    options.AddCustomApiExceptionFilter();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCustomSwagger();
}

app.UseCors();
//启用验证
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/health", context => context.Response.WriteAsync("ok"));

app.Run();