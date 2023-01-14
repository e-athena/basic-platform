using BasicPlatform.AppService.Users.Models;
using BasicPlatform.AppService.Users.Requests;

namespace BasicPlatform.AppService.Users;

/// <summary>
/// 用户服务接口
/// </summary>
public interface IUserQueryService
{
    /// <summary>
    /// 读取分页信息
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Paging<UserModel>> GetAsync(GetUserPagingRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// 读取信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<UserModel> GetAsync(string id, CancellationToken cancellationToken = default);
}