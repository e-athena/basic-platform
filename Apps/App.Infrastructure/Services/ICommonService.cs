namespace App.Infrastructure.Services;

/// <summary>
/// 通用服务接口
/// </summary>
public interface ICommonService
{
    /// <summary>
    /// 读取表格列信息
    /// </summary>
    /// <param name="appId">应用ID</param>
    /// <param name="userId">用户ID</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<GetTableColumnsResponse> GetColumnsAsync<T>(string appId, string? userId) where T : class;
}