using BasicPlatform.AppService;
using BasicPlatform.AppService.ExternalPages.Models;
using BasicPlatform.AppService.Users;
using BasicPlatform.AppService.Users.Requests;
using BasicPlatform.AppService.Users.Responses;
using BasicPlatform.Infrastructure.Tables;

namespace BasicPlatform.WebAPI.Controllers.Organizations;

/// <summary>
/// 员工访问记录
/// </summary>
[Menu("员工访问记录",
    ModuleCode = "organization",
    ModuleName = "组织架构",
    ModuleIcon = "ApartmentOutlined",
    ModuleRoutePath = "/organization",
    ModuleSort = 1,
    RoutePath = "/organization/user-access-record",
    Sort = 3,
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