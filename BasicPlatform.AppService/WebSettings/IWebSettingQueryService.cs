using BasicPlatform.AppService.WebSettings.Models;

namespace BasicPlatform.AppService.WebSettings;

/// <summary>
/// 网站设置查询服务接口
/// </summary>
public interface IWebSettingQueryService
{
    /// <summary>
    /// 读取信息
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WebSettingModel> GetAsync(CancellationToken cancellationToken = default);
}