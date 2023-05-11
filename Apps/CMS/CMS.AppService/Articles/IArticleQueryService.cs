using CMS.AppService.Articles.Models;
using CMS.AppService.Articles.Requests;

namespace CMS.AppService.Articles;

/// <summary>
/// 文章查询接口服务
/// </summary>
public interface IArticleQueryService
{
    /// <summary>
    /// 读取分页列表
    /// </summary>
    /// <param name="request">请求类</param>
    /// <returns></returns>
    Task<Paging<ArticleQueryModel>> GetPagingAsync(GetArticlePagingRequest request);

    /// <summary>
    /// 读取信息
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns></returns>
    Task<ArticleQueryModel?> GetAsync(string id);
}