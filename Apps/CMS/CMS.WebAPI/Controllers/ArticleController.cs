using App.Infrastructure.Messaging.Responses;
using CMS.AppService.Articles;
using CMS.AppService.Articles.Models;
using CMS.AppService.Articles.Requests;
using Microsoft.AspNetCore.Authorization;

namespace CMS.WebAPI.Controllers;

/// <summary>
/// 文章管理管理
/// </summary>
[Menu("文章管理",
    ModuleCode = "content",
    ModuleName = "内容模块",
    ModuleRoutePath = "/content",
    ModuleSort = 1,
    RoutePath = "/content/article",
    Sort = 4,
    Description = "文章管理"
)]
public class ArticleController : CustomControllerBase
{
    private readonly IArticleQueryService _queryService;
    private readonly IMediator _mediator;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="queryService"></param>
    /// <param name="mediator"></param>
    public ArticleController(IArticleQueryService queryService, IMediator mediator)
    {
        _queryService = queryService;
        _mediator = mediator;
    }

    #region 基础接口

    /// <summary>
    /// 读取数据列
    /// </summary>
    /// <param name="commonService"></param>
    /// <returns></returns>
    [HttpGet]
    [SkipApiPermissionVerification]
    [ApiPermission(IsVisible = false)]
    public Task<GetTableColumnsResponse> GetColumnsAsync([FromServices] ICommonService commonService)
    {
        return commonService.GetColumnsAsync<ArticleQueryModel>(
            GlobalConstant.DefaultAppId,
            UserId
        );
    }

    /// <summary>
    /// 读取列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<Paging<ArticleQueryModel>> GetPagingAsync([FromBody] GetArticlePagingRequest request)
    {
        return _queryService.GetPagingAsync(request);
    }

    /// <summary>
    /// 根据ID读取
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission(ApiPermissionConstant.ArticleDetail, DisplayName = "详情")]
    public Task<ArticleQueryModel?> GetAsync([FromQuery] string id)
    {
        return _queryService.GetAsync(id);
    }


    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<string> PostAsync([FromBody] CreateArticleRequest request, CancellationToken cancellationToken)
    {
        return _mediator.SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut]
    [ApiPermission(DisplayName = "编辑", AdditionalRules = new[]
    {
        ApiPermissionConstant.ArticleDetail
    })]
    public Task<string> PutAsync([FromBody] UpdateArticleRequest request, CancellationToken cancellationToken)
    {
        return _mediator.SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete]
    public Task<string> DeleteAsync([FromBody] DeleteArticleRequest request, CancellationToken cancellationToken)
    {
        return _mediator.SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// 发布/取消发布
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut]
    public Task<string> PublishAsync([FromBody] PublishArticleRequest request, CancellationToken cancellationToken)
    {
        return _mediator.SendAsync(request, cancellationToken);
    }

    #endregion
}