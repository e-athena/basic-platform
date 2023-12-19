using BasicPlatform.AppService.WebSettings;
using BasicPlatform.AppService.WebSettings.Models;
using BasicPlatform.Domain.Models.WebSettings;

namespace BasicPlatform.AppService.FreeSql.WebSettings;

/// <summary>
/// 职位查询服务接口实现类
/// </summary>
[Component]
public class WebSettingQueryService : QueryServiceBase<WebSetting>, IWebSettingQueryService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="freeSql"></param>
    /// <param name="accessor"></param>
    public WebSettingQueryService(
        IFreeSql freeSql,
        ISecurityContextAccessor accessor) : base(freeSql, accessor)
    {
    }

    /// <summary>
    /// 读取详情
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<WebSettingModel> GetAsync(CancellationToken cancellationToken = default)
    {
        return QueryableNoTracking.FirstAsync<WebSettingModel>(cancellationToken);
    }
}