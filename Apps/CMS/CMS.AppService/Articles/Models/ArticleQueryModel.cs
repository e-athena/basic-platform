namespace CMS.AppService.Articles.Models;

/// <summary>
/// 文章
/// </summary>
public class ArticleQueryModel : QueryModelBase
{
    /// <summary>
    /// 标题
    /// </summary>
    [TableColumn(Sort = 11)]
    public string Title { get; set; } = null!;

    /// <summary>
    /// 摘要
    /// </summary>
    [TableColumn(HideInTable = true)]
    public string? Summary { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    [TableColumn(HideInTable = true, HideInSearch = true)]
    public string Content { get; set; } = null!;

    /// <summary>
    /// 作者
    /// </summary>
    [TableColumn(Sort = 22, Width = 120)]
    public string Author { get; set; } = null!;

    /// <summary>
    /// 来源
    /// </summary>
    [TableColumn(Sort = 33, Width = 120)]
    public string? Source { get; set; }

    /// <summary>
    /// 来源地址
    /// </summary>
    [TableColumn(HideInTable = true, HideInSearch = true)]
    public string? SourceUrl { get; set; }

    /// <summary>
    /// 封面
    /// </summary>
    [TableColumn(HideInTable = true, HideInSearch = true)]
    public string? Cover { get; set; }

    /// <summary>
    /// 浏览量
    /// </summary>
    [TableColumn(Sort = 44, Width = 90)]
    public int ViewCount { get; set; }

    /// <summary>
    /// 点赞量
    /// </summary>
    [TableColumn(Sort = 55, Width = 90)]
    public int LikeCount { get; set; }

    /// <summary>
    /// 评论量
    /// </summary>
    [TableColumn(Sort = 66, Width = 90)]
    public int CommentCount { get; set; }

    /// <summary>
    /// 是否置顶
    /// </summary>
    [TableColumn(Sort = 77, Width = 90)]
    public bool IsTop { get; set; }

    /// <summary>
    /// 是否推荐
    /// </summary>
    [TableColumn(Sort = 88, Width = 90)]
    public bool IsRecommend { get; set; }

    /// <summary>
    /// 是否发布
    /// </summary>
    [TableColumn(Sort = 99, Width = 90)]
    public bool IsPublish { get; set; }

    /// <summary>
    /// 发布时间
    /// </summary>
    [TableColumn(Sort = 100)]
    public DateTime? PublishTime { get; set; }
}