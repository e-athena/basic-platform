using Athena.Infrastructure.ViewModels;

namespace App.Infrastructure.Services;

/// <summary>
/// 角色服务接口
/// </summary>
public interface IRoleService
{
    /// <summary>
    /// 读取下拉列表
    /// </summary>
    /// <param name="organizationId">组织架构ID</param>
    /// <returns></returns>
    Task<List<SelectViewModel>> GetSelectListAsync(string? organizationId = null);
}