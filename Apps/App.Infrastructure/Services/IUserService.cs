using App.Infrastructure.Messaging.Requests;

namespace App.Infrastructure.Services;

/// <summary>
/// 用户服务
/// </summary>
public interface IUserService
{
    /// <summary>
    /// 读取外部页面列表
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns></returns>
    Task<List<ExternalPageModel>> GetExternalPagesAsync(string userId);

    /// <summary>
    /// 读取用户自定表格列列表
    /// </summary>
    /// <param name="appId">应用ID</param>
    /// <param name="moduleName">模块名称</param>
    /// <param name="userId">用户ID</param>
    /// <returns></returns>
    Task<List<UserCustomColumnModel>> GetUserCustomColumnsAsync(string? appId, string moduleName, string? userId);

    /// <summary>
    /// 读取用户资源
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="appId"></param>
    /// <returns></returns>
    Task<List<ResourceModel>> GetUserResourceAsync(string userId, string appId);

    /// <summary>
    /// 读取用户资源代码
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="appId"></param>
    /// <returns></returns>
    Task<List<string>> GetUserResourceCodesAsync(string userId, string appId);

    /// <summary>
    /// 读取用户信息
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<GetUserInfoResponse?> GetUserInfoAsync(string userId);

    /// <summary>
    /// 读取用户信息
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <returns></returns>
    Task<string?> GetIdByUserNameAsync(string userName);

    /// <summary>
    /// 更新表格列表信息
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<long> UpdateUserCustomColumnsAsync(UpdateUserCustomColumnsRequest request,
        CancellationToken cancellationToken);

    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// 更新用户
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string> UpdateUserAsync(UpdateUserRequest request, CancellationToken cancellationToken = default);
}