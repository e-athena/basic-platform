using BasicPlatform.AppService.Positions.Requests;

namespace BasicPlatform.AppService.FreeSql.Positions;

/// <summary>
/// 
/// </summary>
public class PositionRequestHandler : AppServiceBase<Position>,
    IRequestHandler<CreatePositionRequest, string>,
    IRequestHandler<UpdatePositionRequest, string>,
    IRequestHandler<AssignRolesForPositionRequest, string>,
    IRequestHandler<PositionStatusChangeRequest, string>
{
    public PositionRequestHandler(UnitOfWorkManager unitOfWorkManager, ISecurityContextAccessor accessor) : base(
        unitOfWorkManager, accessor)
    {
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<string> Handle(CreatePositionRequest request, CancellationToken cancellationToken)
    {
        // 封装实体对象
        var entity = new Position(request.ParentId, request.Name, request.Remarks, request.Status, UserId);
        if (request.ParentId != null)
        {
            entity.ParentPath = await GetParentPathAsync(request.ParentId);
        }

        await RegisterNewAsync(entity, cancellationToken);

        // 新增关联数据
        if (request.RoleIds.Count > 0)
        {
            await RegisterNewRangeValueObjectAsync(request
                    .RoleIds
                    .Select(roleId => new PositionRole(entity.Id, roleId))
                    .ToList(),
                cancellationToken);
        }

        return entity.Id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<string> Handle(UpdatePositionRequest request, CancellationToken cancellationToken)
    {
        if (request.ParentId == request.Id)
        {
            throw FriendlyException.Of("不能选择自己作为上级");
        }

        // 封装实体对象
        var entity = await GetForEditAsync(request.Id);

        var parentPath = "";
        if (request.ParentId != null)
        {
            parentPath = await GetParentPathAsync(request.ParentId);
        }

        // 更新
        entity.Update(request.ParentId, parentPath, request.Name, request.Remarks, UserId);
        await RegisterDirtyAsync(entity, cancellationToken);

        // 删除旧数据
        await RegisterDeleteValueObjectAsync<PositionRole>(p => p.PositionId == entity.Id, cancellationToken);

        if (request.RoleIds.Count <= 0)
        {
            return entity.Id;
        }

        // 新增关联数据
        var positionRoles = request
            .RoleIds
            .Select(roleId => new PositionRole(entity.Id, roleId))
            .ToList();
        // 批量新增
        await RegisterNewRangeValueObjectAsync(positionRoles, cancellationToken);

        return entity.Id;
    }

    /// <summary>
    /// 分配角色
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> Handle(AssignRolesForPositionRequest request, CancellationToken cancellationToken)
    {
        var entity = await GetForEditAsync(request.PositionId);
        // 删除旧数据
        await RegisterDeleteValueObjectAsync<PositionUser>(p => p.PositionId == entity.Id, cancellationToken);

        if (!(request.RoleIds?.Count > 0))
        {
            return entity.Id;
        }

        // 创建新的关联数据
        var news = request
            .RoleIds
            .Select(roleId => new PositionRole(request.PositionId, roleId))
            .ToList();
        // 批量新增 
        await RegisterNewRangeValueObjectAsync(news, cancellationToken);

        return entity.Id;
    }

    /// <summary>
    /// 职位状态变更
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<string> Handle(PositionStatusChangeRequest request, CancellationToken cancellationToken)
    {
        var entity = await GetForEditAsync(request.Id);
        entity.StatusChange(UserId);
        await RegisterDirtyAsync(entity, cancellationToken);
        return entity.Id;
    }

    #region 私有方法

    /// <summary>
    /// 读取ParentPath
    /// </summary>
    /// <param name="parentId"></param>
    /// <returns></returns>
    private async Task<string> GetParentPathAsync(string parentId)
    {
        // 读取上级信息
        var parent = await Queryable
            .Where(p => p.Id == parentId)
            .ToOneAsync();

        if (parent == null)
        {
            throw FriendlyException.Of("找不到上级职位");
        }

        return string.IsNullOrEmpty(parent.ParentPath)
            ? parentId
            : $"{parent.ParentPath},{parentId}";
    }

    #endregion
}