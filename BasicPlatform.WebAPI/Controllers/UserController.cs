using BasicPlatform.AppService.Users;
using BasicPlatform.AppService.Users.Models;
using BasicPlatform.AppService.Users.Requests;
using BasicPlatform.AppService.Users.Responses;

namespace BasicPlatform.WebAPI.Controllers;

/// <summary>
/// 用户管理
/// </summary>
public class UserController : CustomControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserQueryService _queryService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="queryService"></param>
    public UserController(IMediator mediator, IUserQueryService queryService)
    {
        _mediator = mediator;
        _queryService = queryService;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public Task<string> PostAsync([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        return _mediator.SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// 读取详情
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<GetUserByIdResponse> GetAsync([FromQuery] string id, CancellationToken cancellationToken)
    {
        return _queryService.GetAsync(id, cancellationToken);
    }
}