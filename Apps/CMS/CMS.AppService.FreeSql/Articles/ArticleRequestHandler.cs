using CMS.AppService.Articles.Requests;

namespace CMS.AppService.FreeSql.Articles;

/// <summary>
/// 组织架构请求处理程序
/// </summary>
public class ArticleRequestHandler : AppServiceBase<Article>,
    IRequestHandler<CreateArticleRequest, string>,
    IRequestHandler<UpdateArticleRequest, string>,
    IRequestHandler<DeleteArticleRequest, string>,
    IRequestHandler<PublishArticleRequest, string>
{
    public ArticleRequestHandler(
        UnitOfWorkManager unitOfWorkManager,
        ISecurityContextAccessor accessor) : base(unitOfWorkManager, accessor)
    {
    }

    /// <summary>
    /// 创建组织架构
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> Handle(CreateArticleRequest request, CancellationToken cancellationToken)
    {
        var entity = new Article(
            request.Title,
            request.Summary,
            request.Content,
            request.Author,
            request.Source,
            request.SourceUrl,
            request.Cover,
            UserId
        );

        await RegisterNewAsync(entity, cancellationToken);
        return entity.Id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="FriendlyException"></exception>
    public async Task<string> Handle(UpdateArticleRequest request, CancellationToken cancellationToken)
    {
        // 封装实体对象
        var entity = await GetForUpdateAsync(request.Id, cancellationToken);
        // 更新
        entity.Update(
            request.Title,
            request.Summary,
            request.Content,
            request.Author,
            request.Source,
            request.SourceUrl,
            request.Cover,
            UserId
        );
        await RegisterDirtyAsync(entity, cancellationToken);
        return entity.Id;
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> Handle(DeleteArticleRequest request, CancellationToken cancellationToken)
    {
        // 删除
        await RegisterDeleteAsync(p => p.Id == request.Id, cancellationToken);
        return request.Id;
    }

    /// <summary>
    /// 发布/取消发布
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> Handle(PublishArticleRequest request, CancellationToken cancellationToken)
    {
        // 封装实体对象
        var entity = await GetForUpdateAsync(request.Id, cancellationToken);
        if (entity.IsPublish)
        {
            entity.UnPublish(UserId);
        }
        else
        {
            // 更新
            entity.Publish(UserId);
        }

        await RegisterDirtyAsync(entity, cancellationToken);
        return entity.Id;
    }
}