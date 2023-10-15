using Athena.Infrastructure.Event.Interfaces;
using CMS.Domain.Models.Articles;
using CMS.Domain.Models.Articles.Events;

namespace CMS.AppService.FreeSql.DomainEventHandlers;

/// <summary>
/// 测试领域事件
/// </summary>
public class TestEventHandler : ServiceBase<Article>,
    IDomainEventHandler<ArticlePublishedEvent>
{
    public TestEventHandler(UnitOfWorkManager unitOfWorkManager, ISecurityContextAccessor accessor) : base(
        unitOfWorkManager, accessor)
    {
    }

    /// <summary>
    /// 文章发布成功事件
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task Handle(ArticlePublishedEvent notification, CancellationToken cancellationToken)
    {
        var entity = await GetForUpdateAsync(notification.GetId()!, cancellationToken);
        // 将标题改成45555+时间戳
        entity.Title = "45555_" + DateTime.Now.Ticks;
        entity.Author = "领域事件修改的";
        await RegisterDirtyAsync(entity, cancellationToken);
    }
}