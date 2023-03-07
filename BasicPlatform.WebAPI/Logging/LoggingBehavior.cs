using System.Diagnostics;
using Athena.Infrastructure.Jwt;

namespace BasicPlatform.WebAPI.Logging;

/// <summary>
/// 日志记录
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    private readonly ISecurityContextAccessor _accessor;

    /// <summary>
    /// 日志记录
    /// </summary>
    /// <param name="loggerFactory"></param>
    /// <param name="accessor"></param>
    public LoggingBehavior(
        ILoggerFactory loggerFactory, ISecurityContextAccessor accessor)
    {
        _accessor = accessor;
        _logger = loggerFactory.CreateLogger<LoggingBehavior<TRequest, TResponse>>();
    }

    /// <summary>
    /// 处理
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        const string prefix = nameof(LoggingBehavior<TRequest, TResponse>);
        var timer = new Stopwatch();
        timer.Start();
        var response = await next();
        timer.Stop();
        var timeTaken = timer.Elapsed;
        // 如果请求大于 3 秒，则记录警告日志
        if (timeTaken.Seconds > 3)
        {
            _logger.LogWarning("[{Prefix}] {TRequest}请求大于{Seconds}秒", prefix,
                typeof(TRequest).Name, timeTaken.Seconds);
        }
        _logger.LogInformation("请求内容：{@Request}，响应内容：{@Response}，请求IP：{IP}，耗时：{Milliseconds}毫秒",
            request,
            response,
            _accessor.IpAddress,
            timeTaken.Milliseconds
        );

        return response;
    }
}