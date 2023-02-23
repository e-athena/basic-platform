using BasicPlatform.AppService.Organizations;
using BasicPlatform.AppService.Organizations.Requests;
using BasicPlatform.AppService.Organizations.Responses;

namespace BasicPlatform.AppService.FreeSql.Organizations;

/// <summary>
/// 
/// </summary>
[Component(LifeStyle.Transient)]
public class OrganizationQueryService : AppQueryServiceBase<Organization>, IOrganizationQueryService
{
    public OrganizationQueryService(IFreeSql freeSql, ISecurityContextAccessor accessor) : base(freeSql, accessor)
    {
    }

    #region 查询

    /// <summary>
    /// 读取分页列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Paging<GetOrganizationPagingResponse>> GetPagingAsync(
        GetOrganizationPagingRequest request)
    {
        var result = await QueryableNoTracking
            .HasWhere(request.Keyword, p => p.Name.Contains(request.Keyword!))
            .ToPagingAsync(request, p => new GetOrganizationPagingResponse
            {
                CreatedUserName = p.CreatedUser!.RealName
            });

        return result;
    }

    /// <summary>
    /// 读取信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<GetOrganizationByIdResponse?> GetAsync(string id)
    {
        var result = await QueryableNoTracking
            .Where(p => p.Id == id)
            .ToOneAsync<GetOrganizationByIdResponse>();

        // 组织架构角色
        var roleIds = await Query<OrganizationRole>()
            .Where(p => p.OrganizationId == id)
            .ToListAsync(p => p.RoleId);

        result?.RoleIds.AddRange(roleIds);

        return result;
    }

    /// <summary>
    /// 读取树形数据
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<List<GetOrganizationTreeDataResponse>> GetTreeDataAsync(
        GetOrganizationTreeDataRequest request)
    {
        var list = await QueryableNoTracking
            .HasWhere(request.Keyword, p => p.Name.Contains(request.Keyword!))
            .ToListAsync(p => new GetOrganizationTreeDataResponse
            {
                CreatedUserName = p.CreatedUser!.RealName
            });

        var result = new List<GetOrganizationTreeDataResponse>();
        var parentId = list.MinBy(p => p.ParentPath.Length)?.ParentId;
        // 递归读取
        GetTreeChildren(list, result, parentId);
        return result;
    }

    /// <summary>
    /// 读取树形数据列表
    /// </summary>
    /// <returns></returns>
    public async Task<List<TreeViewModel>> GetTreeDataAsync()
    {
        var list = await QueryableNoTracking.ToListAsync();
        var result = new List<TreeViewModel>();
        var parentId = list.MinBy(p => p.ParentPath.Length)?.ParentId;
        // 递归读取
        GetTreeChildren(list, result, parentId);
        return result;
    }

    /// <summary>
    /// 读取树形选择框数据列表
    /// </summary>
    /// <returns></returns>
    public async Task<List<TreeSelectViewModel>> GetTreeSelectDataAsync()
    {
        var list = await QueryableNoTracking.ToListAsync();
        var result = new List<TreeSelectViewModel>();
        var parentId = list.MinBy(p => p.ParentPath.Length)?.ParentId;
        // 递归读取
        GetTreeChildrenBySelect(list, result, parentId);
        return result;
    }

    /// <summary>
    /// 读取树形选择框数据列表
    /// </summary>
    /// <returns></returns>
    public async Task<List<TreeSelectViewModel>> GetTreeSelectDataForSelfAsync()
    {
        var list = await QueryableNoTracking.ToListAsync();
        if (!IsRoot)
        {
            var peerOrganization = await QueryNoTracking<OrganizationUser>()
                .Where(p => p.UserId == UserId)
                .OrderBy(p => p.Organization.ParentPath.Length)
                .ToOneAsync(p => p.Organization);
            if (peerOrganization != null)
            {
                list.Add(peerOrganization);
            }
        }

        var result = new List<TreeSelectViewModel>();
        var parentId = list.MinBy(p => p.ParentPath.Length)?.ParentId;
        // 递归读取
        GetTreeChildrenBySelect(list, result, parentId);
        return result;
    }

    /// <summary>
    /// 获取全部组织架构人员树
    /// </summary>
    /// <returns></returns>
    // ReSharper disable once IdentifierTypo
    public async Task<List<CascaderViewModel>> GetCascaderDataAsync()
    {
        var list = await QueryableNoTracking.ToListAsync();
        if (!IsRoot)
        {
            var peerOrganization = await QueryNoTracking<OrganizationUser>()
                .Where(p => p.UserId == UserId)
                .OrderBy(p => p.Organization.ParentPath.Length)
                .ToOneAsync(p => p.Organization);
            if (peerOrganization != null)
            {
                list.Add(peerOrganization);
            }
        }

        var result = new List<CascaderViewModel>();
        var parentId = list.MinBy(p => p.ParentPath.Length)?.ParentId;
        // 递归读取
        GetTreeChildrenCascader(list, result, parentId);
        return result;
    }

    #region 私有方法

    /// <summary>
    /// 递归读取
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="results"></param>
    /// <param name="parentId"></param>
    // ReSharper disable once IdentifierTypo
    private static void GetTreeChildrenCascader(IList<Organization> entities,
        ICollection<CascaderViewModel> results,
        string? parentId = null)
    {
        IList<Organization> result = string.IsNullOrEmpty(parentId)
            ? entities.Where(p => string.IsNullOrEmpty(p.ParentId)).ToList()
            : entities.Where(p => p.ParentId == parentId).ToList();
        foreach (var t1 in result)
        {
            var res = new CascaderViewModel
            {
                Label = t1.Name,
                Value = t1.Id,
                Disabled = t1.Status == Status.Disabled,
            };
            if (entities.Any(p => p.ParentId == t1.Id))
            {
                res.Children = new List<CascaderViewModel>();
                GetTreeChildrenCascader(entities, res.Children, t1.Id);
            }

            results.Add(res);
        }
    }

    /// <summary>
    /// 递归读取
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="results"></param>
    /// <param name="parentId"></param>
    private static void GetTreeChildrenBySelect(IList<Organization> entities,
        ICollection<TreeSelectViewModel> results,
        string? parentId = null)
    {
        IList<Organization> result = string.IsNullOrEmpty(parentId)
            ? entities.Where(p => string.IsNullOrEmpty(p.ParentId)).ToList()
            : entities.Where(p => p.ParentId == parentId).ToList();

        foreach (var t1 in result)
        {
            var res = new TreeSelectViewModel
            {
                Id = t1.Id,
                ParentId = t1.ParentId,
                Title = t1.Name,
                Value = t1.Id,
                Disabled = t1.Status == Status.Disabled,
                IsLeaf = !string.IsNullOrEmpty(t1.ParentId)
            };
            if (entities.Any(p => p.ParentId == t1.Id))
            {
                res.Children = new List<TreeSelectViewModel>();
                GetTreeChildrenBySelect(entities, res.Children, t1.Id);
            }

            results.Add(res);
        }
    }

    /// <summary>
    /// 递归读取
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="results"></param>
    /// <param name="parentId"></param>
    private static void GetTreeChildren(IList<Organization> entities,
        ICollection<TreeViewModel> results,
        string? parentId = null)
    {
        IList<Organization> result = string.IsNullOrEmpty(parentId)
            ? entities.Where(p => string.IsNullOrEmpty(p.ParentId)).ToList()
            : entities.Where(p => p.ParentId == parentId).ToList();

        foreach (var t1 in result)
        {
            var res = new TreeViewModel
            {
                Title = t1.Name,
                Key = t1.Id,
                Disabled = t1.Status == Status.Disabled
            };
            if (entities.Any(p => p.ParentId == t1.Id))
            {
                res.Children = new List<TreeViewModel>();
                GetTreeChildren(entities, res.Children, t1.Id);
            }

            results.Add(res);
        }
    }

    /// <summary>
    /// 递归读取
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="results"></param>
    /// <param name="parentId"></param>
    private static void GetTreeChildren(IList<GetOrganizationTreeDataResponse> entities,
        ICollection<GetOrganizationTreeDataResponse> results,
        string? parentId = null)
    {
        IList<GetOrganizationTreeDataResponse> result = string.IsNullOrEmpty(parentId)
            ? entities.Where(p => string.IsNullOrEmpty(p.ParentId)).ToList()
            : entities.Where(p => p.ParentId == parentId).ToList();

        foreach (var item in result)
        {
            var res = new GetOrganizationTreeDataResponse
            {
                Id = item.Id,
                ParentId = item.ParentId,
                Name = item.Name,
                Remarks = item.Remarks,
                CreatedOn = item.CreatedOn,
                UpdatedOn = item.UpdatedOn,
                Status = item.Status,
                CreatedUserName = item.CreatedUserName
            };
            if (entities.Any(p => p.ParentId == item.Id))
            {
                res.Children = new List<GetOrganizationTreeDataResponse>();
                GetTreeChildren(entities, res.Children, item.Id!);
            }

            results.Add(res);
        }
    }

    #endregion

    #endregion
}