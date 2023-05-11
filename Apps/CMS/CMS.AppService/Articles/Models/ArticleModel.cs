namespace CMS.AppService.Articles.Models;

/// <summary>
/// 文章
/// </summary>
public class ArticleModel : ModelBase
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
    /// </summary>Ï
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
    /// 浏览量
    /// </summary>
    public int ViewCount { get; set; }

    /// <summary>
    /// 点赞量
    /// </summary>
    public int LikeCount { get; set; }

    /// <summary>
    /// 评论量
    /// </summary>
    public int CommentCount { get; set; }

    /// <summary>
    /// 是否置顶
    /// </summary>
    public bool IsTop { get; set; }

    /// <summary>
    /// 是否推荐
    /// </summary>
    public bool IsRecommend { get; set; }

    /// <summary>
    /// 是否发布
    /// </summary>
    public bool IsPublish { get; set; }

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime? PublishTime { get; set; }
}