using BasicPlatform.AppService.Positions.Requests;

namespace BasicPlatform.AppService.FreeSql.Positions;

/// <summary>
/// 职位请求处理程序
/// </summary>
public class PositionRequestHandler : DataPermissionServiceBase<Position>,
    IRequestHandler<CreatePositionRequest, string>,
    IRequestHandler<UpdatePositionRequest, string>,
    IRequestHandler<PositionStatusChangeRequest, string>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="unitOfWorkManager"></param>
    /// <param name="contextAccessor"></param>
    public PositionRequestHandler(UnitOfWorkManager unitOfWorkManager, ISecurityContextAccessor contextAccessor)
        : base(unitOfWorkManager, contextAccessor)
    {
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> Handle(CreatePositionRequest request, CancellationToken cancellationToken)
    {
        var entity = new Position(
            request.OrganizationId,
            request.Name,
            request.Remarks,
            request.Status,
            request.Sort,
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
    public async Task<string> Handle(UpdatePositionRequest request, CancellationToken cancellationToken)
    {
        var entity = await GetForUpdateAsync(request.Id, cancellationToken);
        entity.Update(request.OrganizationId, request.Name, request.Remarks, request.Sort, UserId);
        await RegisterDirtyAsync(entity, cancellationToken);
        return entity.Id;
    }

    /// <summary>
    /// 状态变更
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> Handle(PositionStatusChangeRequest request, CancellationToken cancellationToken)
    {
        var entity = await GetForUpdateAsync(request.Id, cancellationToken);
        entity.StatusChange(UserId);
        await RegisterDirtyAsync(entity, cancellationToken);
        return entity.Id;
    }
}