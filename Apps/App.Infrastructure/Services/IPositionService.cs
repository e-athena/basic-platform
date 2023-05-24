using Athena.Infrastructure.ViewModels;

namespace App.Infrastructure.Services;

/// <summary>
/// 职位服务接口
/// </summary>
public interface IPositionService
{
    /// <summary>
    /// 读取下拉列表
    /// </summary>
    /// <param name="organizationId"></param>
    /// <returns></returns>
    Task<List<SelectViewModel>> GetSelectListAsync(string? organizationId = null);
}