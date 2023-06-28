using App.Infrastructure.Messaging.Requests;

namespace App.Infrastructure.Services.Impls;

/// <summary>
/// 用户服务默认实现
/// </summary>
[Component]
public class DefaultUserService : DefaultServiceBase, IUserService
{
    private const string ApiUrl = "http://localhost:5078";

    public DefaultUserService(ISecurityContextAccessor accessor) : base(accessor)
    {
    }

    /// <summary>
    /// 读取外部页面列表
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns></returns>
    public async Task<List<ExternalPageModel>> GetExternalPagesAsync(string userId)
    {
        const string url = $"{ApiUrl}/api/SubApplication/GetExternalPages";
        var result = await GetRequest(url)
            .SetQueryParams(new
            {
                userId
            })
            .GetJsonAsync<ApiResult<List<ExternalPageModel>>>();
        return result.Data ?? new List<ExternalPageModel>();
    }

    /// <summary>
    /// 读取用户自定表格列列表
    /// </summary>
    /// <param name="appId">应用ID</param>
    /// <param name="moduleName">模块名称</param>
    /// <param name="userId">用户ID</param>
    /// <returns></returns>
    public async Task<List<UserCustomColumnModel>> GetUserCustomColumnsAsync(string? appId, string moduleName,
        string? userId)
    {
        const string url = $"{ApiUrl}/api/SubApplication/GetUserCustomColumns";
        var result = await GetRequest(url)
            .SetQueryParams(new
            {
                userId,
                appId,
                moduleName
            })
            .GetJsonAsync<ApiResult<List<UserCustomColumnModel>>>();
        if (!result.Success || result.Data == null || result.Data.Count == 0)
        {
            return new List<UserCustomColumnModel>();
        }

        return result.Data;
    }

    /// <summary>
    /// 读取用户资源
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="appId">应用ID</param>
    /// <returns></returns>
    public async Task<List<ResourceModel>> GetUserResourceAsync(string userId, string appId)
    {
        const string url = $"{ApiUrl}/api/SubApplication/GetUserResource";
        var result = await GetRequest(url)
            .SetQueryParams(new
            {
                userId,
                appId
            })
            .GetJsonAsync<ApiResult<List<ResourceModel>>>();
        if (!result.Success || result.Data == null || result.Data.Count == 0)
        {
            return new List<ResourceModel>();
        }

        return result.Data;
    }

    /// <summary>
    /// 读取用户资源代码
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="appId"></param>
    /// <returns></returns>
    public async Task<List<string>> GetUserResourceCodesAsync(string userId, string appId)
    {
        const string url = $"{ApiUrl}/api/SubApplication/GetUserResourceCodes";
        var result = await GetRequest(url)
            .SetQueryParams(new
            {
                userId,
                appId
            })
            .GetJsonAsync<ApiResult<List<string>>>();
        if (!result.Success || result.Data == null || result.Data.Count == 0)
        {
            return new List<string>();
        }

        return result.Data;
    }

    /// <summary>
    /// 读取用户信息
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<GetUserInfoResponse?> GetUserInfoAsync(string userId)
    {
        const string url = $"{ApiUrl}/api/SubApplication/GetUserInfo";
        var result = await GetRequest(url)
            .SetQueryParams(new
            {
                userId,
            })
            .GetJsonAsync<ApiResult<GetUserInfoResponse>>();

        return result.Data;
    }

    /// <summary>
    /// 读取用户ID
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public async Task<string?> GetIdByUserNameAsync(string userName)
    {
        const string url = $"{ApiUrl}/api/SubApplication/GetUserIdByUserName";
        var result = await GetRequest(url)
            .SetQueryParams(new
            {
                userName
            })
            .GetJsonAsync<ApiResult<string>>();
        return result.Data;
    }

    /// <summary>
    /// 更新表格列表信息
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<long> UpdateUserCustomColumnsAsync(UpdateUserCustomColumnsRequest request,
        CancellationToken cancellationToken)
    {
        const string url = $"{ApiUrl}/api/SubApplication/UpdateUserCustomColumns";
        var result = await GetRequest(url)
            .PostJsonAsync(request, cancellationToken)
            .ReceiveJson<ApiResult<long>>();
        return result?.Data ?? 0;
    }

    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<string> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        const string url = $"{ApiUrl}/api/SubApplication/CreateUser";
        var result = await GetRequest(url)
            .PostJsonAsync(request, cancellationToken)
            .ReceiveJson<ApiResult<string>>();

        if (!result.Success || string.IsNullOrEmpty(result.Data))
        {
            throw FriendlyException.Of(result.Message);
        }

        return result.Data;
    }

    /// <summary>
    /// 更新用户
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<string> UpdateUserAsync(UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        const string url = $"{ApiUrl}/api/SubApplication/UpdateUser";
        var result = await GetRequest(url)
            .PostJsonAsync(request, cancellationToken)
            .ReceiveJson<ApiResult<string>>();

        if (!result.Success || string.IsNullOrEmpty(result.Data))
        {
            throw FriendlyException.Of(result.Message);
        }

        return result.Data;
    }
}