using BasicPlatform.AppService.ExternalPages;
using BasicPlatform.AppService.ExternalPages.Requests;
using BasicPlatform.AppService.ExternalPages.Responses;

namespace BasicPlatform.AppService.FreeSql.ExternalPages;

/// <summary>
/// 
/// </summary>
[Component]
public class ExternalPageQueryService : QueryServiceBase<ExternalPage>, IExternalPageQueryService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="freeSql"></param>
    /// <param name="accessor"></param>
    public ExternalPageQueryService(IFreeSql freeSql, ISecurityContextAccessor accessor) : base(freeSql, accessor)
    {
    }

    #region 查询

    /// <summary>
    /// 读取分页列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Paging<GetExternalPagePagingResponse>> GetPagingAsync(
        GetExternalPagePagingRequest request)
    {
        var result = await QueryableNoTracking
            .HasWhere(request.Keyword, p => p.Name.Contains(request.Keyword!))
            .HasWhere(request.ParentId, p => p.ParentId == request.ParentId)
            .HasWhere(!IsRoot, p => p.OwnerId == UserId || string.IsNullOrEmpty(p.OwnerId))
            .ToPagingAsync(request, p => new GetExternalPagePagingResponse
            {
                CreatedUserName = p.CreatedUser!.RealName,
                UpdatedUserName = p.LastUpdatedUser!.RealName,
            });

        return result;
    }

    /// <summary>
    /// 读取信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<GetExternalPageByIdResponse?> GetAsync(string id)
    {
        var result = await QueryableNoTracking
            .Where(p => p.Id == id)
            .HasWhere(!IsRoot, p => p.OwnerId == UserId || string.IsNullOrEmpty(p.OwnerId))
            .ToOneAsync<GetExternalPageByIdResponse>();

        // 如果为一级页面
        if (string.IsNullOrEmpty(result.ParentId))
        {
            // 如果有子页面，则为分组
            result.IsGroup = await QueryableNoTracking
                .AnyAsync(p => p.ParentId == result.Id);
        }

        return result;
    }

    /// <summary>
    /// 读取选择框数据列表
    /// </summary>
    /// <returns></returns>
    public async Task<List<SelectViewModel>> GetSelectListAsync()
    {
        var result = await QueryableNoTracking
            .Where(p => string.IsNullOrEmpty(p.ParentId))
            .HasWhere(!IsRoot, p => p.OwnerId == UserId)
            .ToListAsync(p => new SelectViewModel
            {
                Label = p.Name,
                Value = p.Id
            });
        return result;
    }

    /// <summary>
    /// 读取树形数据列表
    /// </summary>
    /// <returns></returns>
    public async Task<List<TreeViewModel>> GetTreeListAsync()
    {
        var list = await QueryableNoTracking
            .HasWhere(!IsRoot, p => p.OwnerId == UserId || string.IsNullOrEmpty(p.OwnerId))
            .ToListAsync();
        var result = new List<TreeViewModel>();
        // 递归读取
        GetTreeChildren(list, result);
        return result;
    }

    #region 私有方法

    /// <summary>
    /// 递归读取
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="results"></param>
    /// <param name="parentId"></param>
    private static void GetTreeChildren(IList<ExternalPage> entities,
        ICollection<TreeViewModel> results,
        string? parentId = null)
    {
        IList<ExternalPage> result = string.IsNullOrEmpty(parentId)
            ? entities.Where(p => string.IsNullOrEmpty(p.ParentId)).ToList()
            : entities.Where(p => p.ParentId == parentId).ToList();

        foreach (var t1 in result.OrderBy(p => p.Sort))
        {
            var res = new TreeViewModel
            {
                Title = t1.Name,
                Key = t1.Id
            };
            if (entities.Any(p => p.ParentId == t1.Id))
            {
                res.Children = new List<TreeViewModel>();
                GetTreeChildren(entities, res.Children, t1.Id);
            }

            results.Add(res);
        }
    }

    #endregion

    #endregion
}