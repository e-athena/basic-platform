using BasicPlatform.AppService.ExternalPages.Models;
using BasicPlatform.AppService.Resources.Models;
using BasicPlatform.AppService.Users;
using BasicPlatform.AppService.Users.Models;
using BasicPlatform.AppService.Users.Requests;
using BasicPlatform.AppService.Users.Responses;
using BasicPlatform.WebAPI.Attributes;

namespace BasicPlatform.WebAPI.Controllers;

/// <summary>
/// 
/// </summary>
[EnableCors]
[Route("api/[controller]/[action]")]
[ApiController]
[SubApplicationFilter]
public class SubApplicationController : ControllerBase
{
    private readonly IUserQueryService _queryService;
    private readonly IMediator _mediator;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="queryService"></param>
    /// <param name="mediator"></param>
    public SubApplicationController(IUserQueryService queryService, IMediator mediator)
    {
        _queryService = queryService;
        _mediator = mediator;
    }

    /// <summary>
    /// 读取用户自定义列列表
    /// </summary>
    /// <param name="appId"></param>
    /// <param name="moduleName"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<List<UserCustomColumnModel>> GetUserCustomColumnsAsync(string appId, string moduleName, string userId)
    {
        return _queryService.GetUserCustomColumnsAsync(appId, moduleName, userId);
    }

    /// <summary>
    /// 读取用户资源
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="appId"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<List<ResourceModel>> GetUserResourceAsync(
        [FromQuery] string userId,
        [FromQuery] string appId
    )
    {
        return _queryService.GetUserResourceAsync(userId, appId);
    }

    /// <summary>
    /// 读取用户资源代码
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="appId"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<List<string>> GetUserResourceCodesAsync(
        [FromQuery] string userId,
        [FromQuery] string appId
    )
    {
        return _queryService.GetResourceCodesAsync(userId, appId);
    }

    /// <summary>
    /// 读取用户信息
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<GetCurrentUserResponse> GetUserInfoAsync([FromQuery] string userId)
    {
        return _queryService.GetCurrentUserAsync(userId);
    }

    /// <summary>
    /// 更新表格列表信息
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<long> UpdateUserCustomColumnsAsync([FromBody] UpdateUserCustomColumnsRequest request,
        CancellationToken cancellationToken)
    {
        return _mediator.SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// 读取外部页面列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<IList<ExternalPageModel>> GetExternalPagesAsync(string userId)
    {
        return _queryService.GetUserExternalPagesAsync(userId);
    }
}