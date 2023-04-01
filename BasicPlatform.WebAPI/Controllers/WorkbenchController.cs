namespace BasicPlatform.WebAPI.Controllers;

/// <summary>
/// 首页
/// </summary>
[Menu("工作台",
    ModuleCode = "dashboard",
    ModuleName = "Dashboard",
    ModuleRoutePath = "/dashboard",
    ModuleIcon = "DashboardOutlined",
    ModuleSort = 0,
    RoutePath = "/dashboard/workbench",
    Icon = "HomeOutlined",
    IsAuth = false
)]
public class WorkbenchController
{
}