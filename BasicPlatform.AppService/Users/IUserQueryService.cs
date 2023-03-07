using BasicPlatform.AppService.Users.Requests;
using BasicPlatform.AppService.Users.Responses;

namespace BasicPlatform.AppService.Users;

/// <summary>
/// 用户服务接口
/// </summary>
public interface IUserQueryService
{
    /// <summary>
    /// 读取分页列表
    /// </summary>
    /// <param name="request">请求类</param>
    /// <returns></returns>
    Task<Paging<GetUserPagingResponse>> GetPagingAsync(GetUserPagingRequest request);

    /// <summary>
    /// 读取信息
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GetUserByIdResponse> GetAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 读取信息
    /// </summary>
    /// <param name="userName">帐户名</param>
    /// <returns></returns>
    Task<GetUserByUserNameResponse> GetByUserNameAsync(string userName);

    /// <summary>
    /// 根据角色Id读取用户列表
    /// </summary>
    /// <param name="roleId">角色Id</param>
    /// <returns></returns>
    Task<List<string?>> GetIdsByRoleIdAsync(string roleId);

    /// <summary>
    /// 读取当前登录用户信息
    /// </summary>
    /// <returns></returns>
    Task<GetCurrentUserResponse> GetCurrentUserAsync();

    /// <summary>
    /// 读取用户数据
    /// </summary>
    /// <returns></returns>
    Task<List<SelectViewModel>> GetSelectListAsync();

    /// <summary>
    /// 读取组织架构用户树形列表
    /// </summary>
    /// <returns></returns>
    Task<List<CascaderViewModel>> GetOrganizationUserTreeSelectListAsync();

    /// <summary>
    /// 读取组织架构和用户树形列表
    /// </summary>
    /// <returns></returns>
    Task<List<CascaderViewModel>> GetOrganizationAndUserTreeSelectListAsync();

    /// <summary>
    /// 读取用户资源
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<List<ResourceModel>> GetUserResourceAsync(string? userId);

    /// <summary>
    /// 读取用户拥有的资源编码
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns></returns>
    Task<GetUserResourceCodeInfoResponse> GetResourceCodeInfoAsync(string userId);
}