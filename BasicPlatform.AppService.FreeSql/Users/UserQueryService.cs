using BasicPlatform.AppService.Users;
using BasicPlatform.AppService.Users.Models;
using BasicPlatform.AppService.Users.Requests;

namespace BasicPlatform.AppService.FreeSql.Users;

/// <summary>
/// 用户服务接口实现类
/// </summary>
[Component]
public class UserQueryService : QueryServiceBase<User>, IUserQueryService
{
    public UserQueryService(IFreeSql freeSql) : base(freeSql)
    {
    }

    /// <summary>
    /// 读取信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<UserModel> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        return QueryableNoTracking.Where(p => p.Id == id).FirstAsync<UserModel>(cancellationToken);
    }

    /// <summary>
    /// 读取分页列表
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Paging<UserModel>> GetAsync(GetUserPagingRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await QueryableNoTracking
            .HasWhere(request.Keyword, p => p.UserName.Contains(request.Keyword!))
            .ToPagingAsync<User, UserModel>(request, cancellationToken);
        return result;
    }
}