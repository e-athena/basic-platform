namespace BasicPlatform.WebAPI.Controllers;

/// <summary>
/// 首页
/// </summary>
[Menu("首页",
    ModuleCode = "home",
    ModuleName = "首页",
    ModuleRoutePath = "/",
    ModuleIcon = "HomeOutlined",
    ModuleSort = 0,
    IsVisible = false,
    IsAuth = false
)]
public class HomeController
{
}