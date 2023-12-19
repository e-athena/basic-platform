namespace BasicPlatform.AppService.Users.Requests;

/// <summary>
/// 删除用户请求
/// </summary>
public class DeleteUserRequest : IdRequest, ITxRequest<int>
{
}
//
// // 删除组织架构
// // Path: BasicPlatform.AppService/Organizations/Requests/DeleteOrganizationRequest.cs
// namespace BasicPlatform.AppService.Organizations.Requests;
//
// /// <summary>
// /// 删除组织架构请求
// /// </summary>
// public class DeleteOrganizationRequest : IdRequest, ITxRequest<int>
// {
// }
//
// // 删除角色
// // Path: BasicPlatform.AppService/Roles/Requests/DeleteRoleRequest.cs
// namespace BasicPlatform.AppService.Roles.Requests;
//
// /// <summary>
// /// 删除角色请求
// /// </summary>
// /// <remarks>
// /// 1. 如果角色下有用户，不允许删除
// /// </remarks>
// public class DeleteRoleRequest : IdRequest, ITxRequest<int>
// {
// }
//
// // 删除职位
// // Path: BasicPlatform.AppService/Positions/Requests/DeletePositionRequest.cs
// namespace BasicPlatform.AppService.Positions.Requests;
//
// /// <summary>
// /// 删除职位请求
// /// </summary>
// /// <remarks>
// /// 1. 如果职位下有用户，不允许删除
// /// </remarks>
// public class DeletePositionRequest : IdRequest, ITxRequest<int>
// {
// }
//
