using Athena.Infrastructure.Event.Attributes;
using Athena.InstantMessaging;
using Athena.InstantMessaging.Models;
using CMS.Domain.Models.Articles.Events;

namespace CMS.IntegratedEventHandler;

/// <summary>
/// 
/// </summary>
public class SignalRMessageHandler :
    IMessageHandler<ArticleCreatedEvent>,
    IMessageHandler<ArticlePublishedEvent>
{
    private const string TopicGroup = "signalr.message.group";
    private readonly INoticeHubService _noticeHubService;

    public SignalRMessageHandler(INoticeHubService noticeHubService)
    {
        _noticeHubService = noticeHubService;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="payload"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [IntegratedEventSubscribe(nameof(ArticleCreatedEvent), Group = TopicGroup)]
    public Task HandleAsync(ArticleCreatedEvent payload, CancellationToken cancellationToken)
    {
        return _noticeHubService.SendMessageToAllAsync(new InstantMessaging<string>
        {
            NoticeType = "OnlineNotice",
            Data = $"{payload.CreatedUserId}创建了一篇文章~",
            Type = MessageType.Notice,
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="payload"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [IntegratedEventSubscribe(nameof(ArticlePublishedEvent), Group = TopicGroup)]
    public Task HandleAsync(ArticlePublishedEvent payload, CancellationToken cancellationToken)
    {
        return _noticeHubService.SendMessageToAllAsync(new InstantMessaging<string>
        {
            NoticeType = "OnlineNotice",
            Data = $"{payload.LastUpdatedUserId}发布了文章:{payload.Title}~",
            Type = MessageType.Notice,
        });
    }
}