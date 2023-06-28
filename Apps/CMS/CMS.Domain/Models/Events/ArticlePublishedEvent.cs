using Athena.Infrastructure.Event;

namespace CMS.Domain.Models.Events;

/// <summary>
/// 文章发布成功事件
/// </summary>
public class ArticlePublishedEvent : DomainEvent<string>
{
    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// 操作人
    /// </summary>
    public string? LastUpdatedUserId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public ArticlePublishedEvent()
    {
    }

    public ArticlePublishedEvent(string title, string? userId)
    {
        Title = title;
        LastUpdatedUserId = userId;
    }
}