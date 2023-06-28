var builder = WebApplication.CreateBuilder(args);

builder.Host.AddYarpJson();
builder.Services.AddCustomCors(builder.Configuration);
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddRateLimiter(options =>
{
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
        {
            await context.HttpContext.Response.WriteAsJsonAsync(new
            {
                context.HttpContext.Response.StatusCode,
                Message = $"请求太快啦,请{retryAfter.TotalSeconds}秒钟后重试～",
                Success = false
            }, cancellationToken: token);
        }
        else
        {
            await context.HttpContext.Response.WriteAsJsonAsync(new
            {
                context.HttpContext.Response.StatusCode,
                Message = "请求太快啦～",
                Success = false
            }, cancellationToken: token);
        }
    };
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        var partitionKey = httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString();
        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: partitionKey,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                // 是否自动刷新计数器
                AutoReplenishment = true,
                // 最大请求量
                PermitLimit = 1000,
                // 处理队列数，如果为0时，超过最大请求量的请求将被拒绝
                QueueLimit = 0,
                // 窗口的补充时间间隔
                Window = TimeSpan.FromSeconds(1)
            });
    });
});

var app = builder.Build();

app.UseCors();
app.UseHttpLogging();
app.UseRateLimiter();
app.MapReverseProxy();
app.Run();