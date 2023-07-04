using BasicPlatform.AppService.ExternalPages.Requests;

namespace BasicPlatform.AppService.FreeSql.ExternalPages;

/// <summary>
/// 组织架构请求处理程序
/// </summary>
public class ExternalPageRequestHandler : AppServiceBase<ExternalPage>,
    IRequestHandler<CreateExternalPageRequest, string>,
    IRequestHandler<UpdateExternalPageRequest, string>,
    IRequestHandler<DeleteExternalPageRequest, string>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="unitOfWorkManager"></param>
    /// <param name="accessor"></param>
    public ExternalPageRequestHandler(
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
    public async Task<string> Handle(CreateExternalPageRequest request, CancellationToken cancellationToken)
    {
        // 检查跳转地址是否重复
        bool exists;
        if (request.IsPublic)
        {
            exists = await QueryableNoTracking
                .Where(x => x.Path == request.Path && string.IsNullOrEmpty(x.OwnerId))
                .AnyAsync(cancellationToken);
        }
        else
        {
            exists = await QueryableNoTracking
                .Where(x => x.Path == request.Path && x.OwnerId != UserId)
                .AnyAsync(cancellationToken);
        }

        if (exists)
        {
            throw FriendlyException.Of("跳转地址已存在");
        }

        var ownerId = IsRoot && request.IsPublic ? null : UserId;
        var entity = new ExternalPage(
            request.ParentId,
            ownerId,
            request.Name,
            request.Type,
            request.Path,
            request.Icon,
            request.Layout,
            request.Sort,
            request.Remarks,
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
    public async Task<string> Handle(UpdateExternalPageRequest request, CancellationToken cancellationToken)
    {
        if (request.ParentId == request.Id)
        {
            throw FriendlyException.Of("不能选择自己作为上级");
        }

        // 检查跳转地址是否重复
        bool exists;
        if (request.IsPublic)
        {
            exists = await QueryableNoTracking
                .Where(p => p.Id != request.Id && p.Path == request.Path && string.IsNullOrEmpty(p.OwnerId))
                .AnyAsync(cancellationToken);
        }
        else
        {
            exists = await QueryableNoTracking
                .Where(p => p.Id != request.Id && p.Path == request.Path && p.OwnerId != UserId)
                .AnyAsync(cancellationToken);
        }

        if (exists)
        {
            throw FriendlyException.Of("跳转地址已存在");
        }

        // 封装实体对象
        var entity = await GetForUpdateAsync(request.Id, cancellationToken);
        var ownerId = IsRoot && request.IsPublic ? null : UserId;
        // 更新
        entity.Update(
            request.ParentId,
            ownerId,
            request.Name,
            request.Type,
            request.Path,
            request.Icon,
            request.Layout,
            request.Sort,
            request.Remarks,
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
    public async Task<string> Handle(DeleteExternalPageRequest request, CancellationToken cancellationToken)
    {
        // 检查是否有子页面
        var hasChild = await QueryableNoTracking
            .Where(p => p.ParentId == request.Id)
            .AnyAsync(cancellationToken);
        if (hasChild)
        {
            throw FriendlyException.Of("请先删除子页面");
        }

        if (!IsRoot)
        {
            var flag = await QueryableNoTracking
                .Where(p => p.OwnerId == UserId)
                .Where(p => p.Id == request.Id)
                .AnyAsync(cancellationToken);

            if (!flag)
            {
                throw FriendlyException.Of("没有权限");
            }
        }

        // 删除
        await RegisterDeleteAsync(p => p.Id == request.Id, cancellationToken);
        return request.Id;
    }
}