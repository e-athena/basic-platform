namespace CMS.AppService.Articles.Requests;

/// <summary>
/// 删除请求类
/// </summary>
public class DeleteArticleRequest : IdRequest, ITxRequest<string>
{
}