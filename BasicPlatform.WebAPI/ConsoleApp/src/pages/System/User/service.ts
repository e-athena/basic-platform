import { get, paging, post, put } from '@/utils/request';

/** 数据列 */
export function queryColumns() {
  return get<API.TableColumnItem[]>('/api/User/GetColumns');
}

/** 列表 */
export function query(params: API.UserPagingParams) {
  return paging<API.UserListItem>('/api/User/GetPaging', params);
}

/** 详情 */
export function detail(id: string) {
  return get<API.UserDetailInfo>('/api/User/Get', { id });
}

/** 创建 */
export function create(data: API.CreateUserRequest) {
  return post<API.CreateUserRequest, string>('/api/User/Post', data);
}

/** 更新 */
export function update(data: API.UpdateUserRequest) {
  return put<API.UpdateUserRequest, string>('/api/User/Put', data);
}

/** 资源代码 */
export function queryResourceCodeInfo(id: string) {
  return get<API.UserResourceCodeInfo>('/api/User/GetResourceCodeInfo', { id });
}

/** 分配资源 */
export function assignResources(data: API.AssignUserResourcesRequest) {
  return put<API.AssignUserResourcesRequest, string>('/api/User/AssignResources', data);
}
/** 数据权限列表 */
export function dataPermission(id: string) {
  return get<API.UserDataPermission[]>('/api/User/GetDataPermissions', { id });
}
/** 分配数据权限 */
export function assignDataPermissions(data: API.AssignUserDataPermissionsRequest) {
  return put<API.AssignUserDataPermissionsRequest, string>('/api/User/AssignDataPermissions', data);
}
/** 切换状态 */
export function statusChange(id: string) {
  return put('/api/User/StatusChange', { id });
}

/** 重置密码 */
export function resetPassword(id: string): Promise<ApiResponse<string>> {
  return post('/api/User/ResetPassword', { id });
}
