namespace CMS.AppService.Articles.Requests;

/// <summary>
/// 更新文章请求类
/// </summary>
public class UpdateArticleRequest : CreateArticleRequest
{
    /// <summary>
    /// ID
    /// </summary>
    public string Id { get; set; } = null!;
}