using BasicPlatform.AppService.Organizations.Requests;

namespace BasicPlatform.AppService.FreeSql.Organizations;

/// <summary>
/// 组织架构请求处理程序
/// </summary>
public class OrganizationRequestHandler : AppServiceBase<Organization>,
    IRequestHandler<CreateOrganizationRequest, string>,
    IRequestHandler<UpdateOrganizationRequest, string>,
    IRequestHandler<OrganizationStatusChangeRequest, string>
{
    public OrganizationRequestHandler(
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
    public async Task<string> Handle(CreateOrganizationRequest request, CancellationToken cancellationToken)
    {
        var entity = new Organization(
            request.ParentId,
            request.Name,
            request.LeaderId,
            request.Remarks,
            request.Status,
            request.Sort,
            UserId
        );

        await RegisterNewAsync(entity, cancellationToken);

        // 新增关联数据
        if (request.RoleIds.Count <= 0)
        {
            return entity.Id;
        }

        var organizationRoles = request
            .RoleIds
            .Select(roleId =>
                new OrganizationRole(entity.Id, roleId)
            ).ToList();
        // 添加关联数据
        await RegisterNewRangeValueObjectAsync(organizationRoles, cancellationToken);
        return entity.Id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="FriendlyException"></exception>
    public async Task<string> Handle(UpdateOrganizationRequest request, CancellationToken cancellationToken)
    {
        if (request.ParentId == request.Id)
        {
            throw FriendlyException.Of("不能选择自己作为上级");
        }

        // 封装实体对象
        var entity = await GetForUpdateAsync(request.Id, cancellationToken);

        if (!string.IsNullOrWhiteSpace(request.ParentId))
        {
            entity.ParentPath = await GetParentPathAsync(request.ParentId);
            if (entity.ParentPath.Contains(entity.Id))
            {
                throw FriendlyException.Of("不能选择自己的下级作为上级");
            }
        }

        // 更新
        entity.Update(request.ParentId, request.Name, request.LeaderId, request.Remarks, request.Sort, UserId);
        await RegisterDirtyAsync(entity, cancellationToken);
        // 删除旧数据
        await RegisterDeleteValueObjectAsync<OrganizationRole>(p => p.OrganizationId == entity.Id, cancellationToken);
        // 新增关联数据
        if (request.RoleIds.Count <= 0)
        {
            return entity.Id;
        }

        var organizationRoles = request
            .RoleIds
            .Select(roleId =>
                new OrganizationRole(entity.Id, roleId)
            ).ToList();
        // 新增关联数据
        await RegisterNewRangeValueObjectAsync(organizationRoles, cancellationToken);

        return entity.Id;
    }

    /// <summary>
    /// 变更状态
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> Handle(OrganizationStatusChangeRequest request, CancellationToken cancellationToken)
    {
        var entity = await GetForUpdateAsync(request.Id, cancellationToken);
        // 变更状态
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
            throw FriendlyException.Of("找不到上级组织架构");
        }

        return string.IsNullOrEmpty(parent.ParentPath)
            ? parentId
            : $"{parent.ParentPath},{parentId}";
    }

    #endregion
}