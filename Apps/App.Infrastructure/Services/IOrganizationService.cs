using Athena.Infrastructure.ViewModels;

namespace App.Infrastructure.Services;

/// <summary>
/// 组织架构服务接口
/// </summary>
public interface IOrganizationService
{
    /// <summary>
    /// 获取级联信息
    /// </summary>
    /// <returns></returns>
    Task<List<CascaderViewModel>> GetCascaderListAsync(string? parentId = null);

    /// <summary>
    /// 读取树形列表
    /// </summary>
    /// <returns></returns>
    Task<List<TreeViewModel>> GetTreeListAsync(string? parentId = null);

    /// <summary>
    /// 读取下拉列表
    /// </summary>
    /// <returns></returns>
    Task<List<SelectViewModel>> GetSelectListAsync(string? parentId = null);

    /// <summary>
    /// 读取ID
    /// </summary>
    /// <param name="name">名称</param>
    /// <returns></returns>
    Task<string?> GetIdByNameAsync(string name);
}