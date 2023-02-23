using BasicPlatform.AppService.Positions;
using BasicPlatform.AppService.Positions.Requests;
using BasicPlatform.AppService.Positions.Responses;

namespace BasicPlatform.AppService.FreeSql.Positions;

/// <summary>
/// 职位服务接口实现类
/// </summary>
[Component(LifeStyle.Transient)]
public class PositionQueryService : AppQueryServiceBase<Position>, IPositionQueryService
{
  public PositionQueryService(IFreeSql freeSql, ISecurityContextAccessor accessor) : base(freeSql, accessor)
  {
  }

  #region 查询

  /// <summary>
  /// 读取分页列表
  /// </summary>
  /// <param name="request"></param>
  /// <returns></returns>
  public async Task<Paging<GetPositionPagingResponse>> GetPagingAsync(
      GetPositionPagingRequest request)
  {
    var result = await QueryableNoTracking
        .HasWhere(request.Keyword, p => p.Name.Contains(request.Keyword!))
        .ToPagingAsync(request, p => new GetPositionPagingResponse
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
  public async Task<GetPositionByIdResponse> GetAsync(string id)
  {
    var result = await QueryableNoTracking
        .Where(p => p.Id == id)
        .ToOneAsync<GetPositionByIdResponse>();
    if (result == null)
    {
      throw FriendlyException.Of("找不到数据");
    }

    // 读取角色
    var roles = await Query<PositionRole>()
        .Where(p => p.PositionId == id)
        .ToListAsync(p => p.RoleId);
    result.RoleIds.AddRange(roles);
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
    GetTreeSelectChildren(list, result, parentId);
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
      var peerPosition = await QueryNoTracking<PositionUser>()
          .Where(p => p.UserId == UserId)
          .OrderBy(p => p.Position.ParentPath.Length)
          .ToOneAsync(p => p.Position);
      if (peerPosition != null)
      {
        list.Add(peerPosition);
      }
    }

    var result = new List<TreeSelectViewModel>();
    var parentId = list.MinBy(p => p.ParentPath.Length)?.ParentId;
    // 递归读取
    GetTreeSelectChildren(list, result, parentId);
    return result;
  }

  /// <summary>
  /// 根据角色Id读取职位Id列表
  /// </summary>
  /// <param name="roleId">角色ID</param>
  /// <returns></returns>
  public async Task<List<string>> GetIdsByRoleIdAsync(string roleId)
  {
    var result = await Query<PositionRole>()
        .Where(p => p.RoleId == roleId)
        .ToListAsync(p => p.PositionId);

    return result;
  }

  #region 私有方法

  /// <summary>
  /// 递归读取
  /// </summary>
  /// <param name="entities"></param>
  /// <param name="results"></param>
  /// <param name="parentId"></param>
  private static void GetTreeChildren(IList<Position> entities, ICollection<TreeViewModel> results,
      string? parentId = null)
  {
    IList<Position> result = string.IsNullOrEmpty(parentId)
        ? entities.Where(p => string.IsNullOrEmpty(p.ParentId)).ToList()
        : entities.Where(p => p.ParentId == parentId).ToList();

    foreach (var t1 in result)
    {
      var res = new TreeViewModel
      {
        Title = t1.Name,
        Key = t1.Id,
        Disabled = t1.Status == Status.Disabled,
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
  private static void GetTreeSelectChildren(IList<Position> entities, ICollection<TreeSelectViewModel> results,
      string? parentId = null)
  {
    IList<Position> result = string.IsNullOrEmpty(parentId)
        ? entities.Where(p => string.IsNullOrEmpty(p.ParentId)).ToList()
        : entities.Where(p => p.ParentId == parentId).ToList();

    foreach (var t1 in result)
    {
      var res = new TreeSelectViewModel
      {
        ParentId = t1.ParentId,
        Title = t1.Name,
        Value = t1.Id,
        Disabled = t1.Status == Status.Disabled,
        IsLeaf = !string.IsNullOrEmpty(t1.ParentId)
      };
      if (entities.Any(p => p.ParentId == t1.Id))
      {
        res.Children = new List<TreeSelectViewModel>();
        GetTreeSelectChildren(entities, res.Children, t1.Id);
      }

      results.Add(res);
    }
  }

  #endregion

  #endregion
}