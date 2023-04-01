using BasicPlatform.AppService.Users;
using BasicPlatform.AppService.Users.Responses;

namespace BasicPlatform.WebAPI.Controllers.System;

/// <summary>
/// 员工访问记录
/// </summary>
[Menu("员工访问记录",
    ModuleCode = "system",
    ModuleName = "系统管理",
    ModuleRoutePath = "/system",
    ModuleSort = 1,
    RoutePath = "/system/user-access-record",
    Sort = 6,
    Description = "员工访问记录管理"
)]
public class UserAccessRecordController : CustomControllerBase
{
    private readonly IUserQueryService _queryService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="queryService"></param>
    public UserAccessRecordController(IUserQueryService queryService)
    {
        _queryService = queryService;
    }

    /// <summary>
    /// 读取列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<Paging<GetUserAccessRecordPagingResponse>> GetPagingAsync([FromBody] GetCommonPagingRequest request)
    {
        return _queryService.GetAccessRecordPagingAsync(request);
    }
}