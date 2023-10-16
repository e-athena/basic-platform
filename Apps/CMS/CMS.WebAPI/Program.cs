// Run an Athena application.
var builder = WebApplication.CreateBuilder(args);
builder.AddAthena(services =>
{
    services.AddCustomOpenTelemetry<Program>(builder.Configuration);
});
var app = builder.Build();
app.RunAthena<Program>();