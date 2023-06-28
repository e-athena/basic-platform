using Athena.Infrastructure.Event;

namespace CMS.Domain.Models.Events;

/// <summary>
/// 文章创建成功事件
/// </summary>
public class ArticleCreatedEvent : DomainEvent<string>
{
    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// 摘要
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    [MaxLength(-1)]
    public string Content { get; set; } = null!;

    /// <summary>
    /// 作者
    /// </summary>
    public string Author { get; set; } = null!;

    /// <summary>
    /// 来源
    /// </summary>
    public string? Source { get; set; }

    /// <summary>
    /// 来源地址
    /// </summary>
    public string? SourceUrl { get; set; }

    /// <summary>
    /// 封面
    /// </summary>
    public string? Cover { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public string? CreatedUserId { get; set; }

    
    public ArticleCreatedEvent()
    {
    }

    public ArticleCreatedEvent(string title, string? summary, string content, string author, string? source,
        string? sourceUrl, string? cover, string? createdUserId)
    {
        Title = title;
        Summary = summary;
        Content = content;
        Author = author;
        Source = source;
        SourceUrl = sourceUrl;
        Cover = cover;
        CreatedUserId = createdUserId;
    }
}