using BasicPlatform.AppService.WebSettings;
using BasicPlatform.AppService.WebSettings.Models;
using BasicPlatform.AppService.WebSettings.Requests;

namespace BasicPlatform.WebAPI.Controllers.System;

/// <summary>
/// 网站设置控制器
/// </summary>
[FrontEndRouting("网站设置",
    ModuleCode = "system",
    ModuleName = "系统管理",
    ModuleRoutePath = "/system",
    ModuleSort = 1,
    RoutePath = "/system/setting",
    Sort = 7
)]
public class WebSettingController : CustomControllerBase
{
    /// <summary>
    /// 读取设置
    /// </summary>
    /// <param name="queryService"></param>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public Task<WebSettingModel> GetAsync([FromServices] IWebSettingQueryService queryService)
    {
        return queryService.GetAsync();
    }

    /// <summary>
    /// 保存设置
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<string> SaveAsync(
        [FromServices] ISender sender,
        [FromBody] SaveWebSettingRequest request,
        CancellationToken cancellationToken)
    {
        return sender.SendAsync(request, cancellationToken);
    }
}