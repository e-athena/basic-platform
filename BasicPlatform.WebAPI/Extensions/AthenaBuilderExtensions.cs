using Microsoft.Extensions.FileProviders;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// 
/// </summary>
public static class AthenaBuilderExtensions
{
    private const string EmbeddedFileNamespace = "BasicPlatform.WebAPI.wwwroot";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseAthena(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseAthenaProvider();
        app.UseCustomFreeSqlMultiTenancy();
        if (env.IsDevelopment())
        {
            app.UseCustomSwagger();
        }

        const string pathMatch = "/basic";
        app.UseStaticFiles(new StaticFileOptions()
        {
            RequestPath = pathMatch,
            FileProvider = new EmbeddedFileProvider(typeof(AthenaBuilderExtensions).Assembly, EmbeddedFileNamespace)
        });
        app.UseCustomAuditLog();
        app.UseRouter(configure: endpointRouteBuilder =>
        {
            endpointRouteBuilder.MapGet(pathMatch, httpContext =>
            {
                var path = httpContext.Request.Path.Value;
                var redirectUrl = string.IsNullOrEmpty(path) || path.EndsWith("/")
                    ? "index.html"
                    : $"{path.Split('/').Last()}/index.html";
                httpContext.Response.StatusCode = 301;
                httpContext.Response.Headers["Location"] = redirectUrl;
                return Task.CompletedTask;
            });
            endpointRouteBuilder.MapGet(pathMatch + "/index.html", async httpContext =>
            {
                httpContext.Response.StatusCode = 200;
                httpContext.Response.ContentType = "text/html;charset=utf-8";

                await using var stream =
                    typeof(AthenaBuilderExtensions).Assembly.GetManifestResourceStream(
                        EmbeddedFileNamespace + ".index.html");
                if (stream == null) throw new InvalidOperationException();

                using var sr = new StreamReader(stream);
                var htmlBuilder = new StringBuilder(await sr.ReadToEndAsync());
                await httpContext.Response.WriteAsync(htmlBuilder.ToString(), Encoding.UTF8);
            });

            endpointRouteBuilder.MapCustomSignalR();
            endpointRouteBuilder.MapControllers();
        });

        return app;
    }

    /// <summary>
    /// 前端路由
    /// </summary>
    /// <param name="app"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static WebApplication MapSpaFront(this WebApplication app, string pattern = "/")
    {
        app.MapGet(pattern, async httpContext =>
        {
            httpContext.Response.StatusCode = 200;
            httpContext.Response.ContentType = "text/html;charset=utf-8";

            await using var stream =
                typeof(AthenaBuilderExtensions).Assembly.GetManifestResourceStream(
                    "BasicPlatform.WebAPI.wwwroot" + ".index.html");
            if (stream == null) throw new InvalidOperationException();

            using var sr = new StreamReader(stream);
            var htmlBuilder = new StringBuilder(await sr.ReadToEndAsync());
            await httpContext.Response.WriteAsync(htmlBuilder.ToString(), Encoding.UTF8);
        });
        return app;
    }

    /// <summary>
    /// 健康检查
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static WebApplication MapHealth(this WebApplication app)
    {
        app.MapGet("/health", async context =>
        {
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("ok");
        });
        return app;
    }

    /// <summary>使用路由配置，用于注册路由映射</summary>
    /// <param name="app"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseRouter(this IApplicationBuilder app, Action<IEndpointRouteBuilder> configure)
    {
        if (configure == null) throw new ArgumentNullException(nameof(configure));

        // 设置默认路由。如果外部已经执行 UseRouting，则直接注册
        if (app.Properties.TryGetValue("__EndpointRouteBuilder", out var value) && value is IEndpointRouteBuilder eps)
        {
            configure(eps);
        }
        else
        {
            app.UseRouting();
            app.UseEndpoints(configure);
        }

        return app;
    }
}