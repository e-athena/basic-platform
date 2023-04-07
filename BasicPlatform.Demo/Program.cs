using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
var environment = builder.Environment;
var host = builder.Host;
services.AddAthena(new AthenaServiceOptions(configuration, environment, host)
{
    ServiceComponentAssemblies = new List<Assembly> {Assembly.GetExecutingAssembly()}
});

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseCors();
// 启用验证
app.UseAuthentication();
app.UseAuthorization();
app.UseAthena(environment);
app.Run();