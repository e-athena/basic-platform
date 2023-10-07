using System.Net.Mime;
using Athena.Infrastructure.Attributes;
using Athena.Infrastructure.Messaging.Responses;
using CMS.AppService.Articles;
using CMS.AppService.Articles.Models;
using CMS.AppService.Articles.Requests;
using CMS.Domain.Models.Articles;

namespace CMS.AppService.FreeSql.Articles;

/// <summary>
/// 网站系统应用查询接口服务实现类
/// </summary>
[Component]
public class ArticleQueryService : QueryServiceBase<Article>, IArticleQueryService
{
    public ArticleQueryService(IFreeSql freeSql, ISecurityContextAccessor accessor) : base(freeSql, accessor)
    {
    }

    /// <summary>
    /// 读取分页数据
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Paging<ArticleQueryModel>> GetPagingAsync(GetArticlePagingRequest request)
    {
        var result = await QueryableNoTracking
            .HasWhere(request.Keyword, p => p.Title.Contains(request.Keyword!))
            .ToPagingAsync<Article, ArticleQueryModel>(request);
        return result;
    }

    /// <summary>
    /// 读取详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<ArticleQueryModel?> GetAsync(string id)
    {
        return QueryableNoTracking
            .Where(p => p.Id == id)
            .ToOneAsync<ArticleQueryModel>()!;
    }
}