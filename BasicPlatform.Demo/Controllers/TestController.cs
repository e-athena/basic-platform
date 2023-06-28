using Athena.Infrastructure.ApiPermission.Attributes;
using BasicPlatform.WebAPI.Controllers;

namespace BasicPlatform.Demo.Controllers;

/// <summary>
/// 测试管理
/// </summary>
[Menu("测试管理",
    ModuleCode = "system",
    ModuleName = "业务模块",
    ModuleRoutePath = "/business",
    ModuleSort = 1,
    RoutePath = "/business/test",
    Sort = 2,
    Description = "测试的业务模块"
)]
public class TestController: CustomControllerBase
{
    
}