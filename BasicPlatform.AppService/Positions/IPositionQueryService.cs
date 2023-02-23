using BasicPlatform.AppService.Positions.Requests;
using BasicPlatform.AppService.Positions.Responses;

namespace BasicPlatform.AppService.Positions;

/// <summary>
/// 职位服务接口
/// </summary>
public interface IPositionQueryService
{

  #region 查询

  /// <summary>
  /// 读取分页列表
  /// </summary>
  /// <param name="request">请求类</param>
  /// <returns></returns>
  Task<Paging<GetPositionPagingResponse>> GetPagingAsync(GetPositionPagingRequest request);

  /// <summary>
  /// 读取信息
  /// </summary>
  /// <param name="id">Id</param>
  /// <returns></returns>
  Task<GetPositionByIdResponse> GetAsync(string id);

  /// <summary>
  /// 读取树形数据列表
  /// </summary>
  /// <returns></returns>
  Task<List<TreeViewModel>> GetTreeDataAsync();
  /// <summary>
  /// 读取树形选择框数据列表
  /// </summary>
  /// <returns></returns>
  Task<List<TreeSelectViewModel>> GetTreeSelectDataAsync();

  /// <summary>
  /// 读取树形选择框数据列表
  /// </summary>
  /// <returns></returns>
  Task<List<TreeSelectViewModel>> GetTreeSelectDataForSelfAsync();

  /// <summary>
  /// 根据角色Id读取职位Id列表
  /// </summary>
  /// <param name="roleId">角色ID</param>
  /// <returns></returns>
  Task<List<string>> GetIdsByRoleIdAsync(string roleId);

  #endregion
}