// Run an Athena application.
var builder = WebApplication.CreateBuilder(args);
builder.AddAthena(services =>
{
    // OTel
    services.AddCustomOpenTelemetry<Program>(builder.Configuration);
});
var app = builder.Build();
app.UseAthena<Program>(mapActions: application =>
{
    // 添加SignalR Hubs
    application.MapCustomSignalR();
});
app.Run();