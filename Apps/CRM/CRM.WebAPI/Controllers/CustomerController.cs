using CRM.Commands.Customers;
using Microsoft.AspNetCore.Authorization;

namespace CRM.WebAPI.Controllers;

/// <summary>
/// 客户控制器
/// </summary>
[FrontEndRouting("客户管理",
    ModuleCode = "basic",
    ModuleName = "基础模块",
    ModuleRoutePath = "/basic",
    ModuleSort = 1,
    RoutePath = "/basic/customer",
    Sort = 1
)]
public class CustomerController : CustomControllerBase
{
    private readonly IMediator _mediator;

    public CustomerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public Task<string> PostAsync([FromBody] CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        return _mediator.SendAsync(command, cancellationToken);
    }
}