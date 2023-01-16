namespace Microsoft.AspNetCore.Builder;

public static class GatewayHostBuilderExtensions
{
    private const string AppYarpJsonPath = "yarp.json";

    public static IHostBuilder AddYarpJson(
        this IHostBuilder hostBuilder,
        bool optional = true,
        bool reloadOnChange = true,
        string path = AppYarpJsonPath)
    {
        return hostBuilder.ConfigureAppConfiguration((_, builder) =>
        {
            builder.AddJsonFile(
                    path: path,
                    optional: optional,
                    reloadOnChange: reloadOnChange
                )
                .AddEnvironmentVariables();
        });
    }
}