import { get, paging, post, put } from '@/utils/request';

/** 列表 */
export function query(params: API.RolePagingParams) {
  return paging<API.RoleListItem>('/api/Role/GetPaging', params);
}

/** 详情 */
export function detail(id: string) {
  return get<API.RoleDetailItem>('/api/Role/Get', { id });
}

/** 创建 */
export function create(data: API.CreateRoleItem) {
  return post<API.CreateRoleItem, string>('/api/Role/Post', data);
}

/** 更新 */
export function update(data: API.UpdateRoleItem) {
  return put<API.UpdateRoleItem, string>('/api/Role/Put', data);
}
/** 切换状态 */
export function statusChange(id: string) {
  return put('/api/Role/StatusChange', { id });
}

/** 分配资源 */
export function assignResources(data: API.AssignRoleResourcesRequest) {
  return put<API.AssignRoleResourcesRequest, string>('/api/Role/AssignResources', data);
}
/** 分配用户 */
export function assignUsers(data: API.AssignRolUsersRequest) {
  return put<API.AssignRolUsersRequest, string>('/api/Role/AssignUsers', data);
}
/** 数据权限列表 */
export function dataPermission(id: string) {
  return get<API.RoleDataPermissionGroup[]>('/api/Role/GetDataPermissions', { id });
}
/** 分配数据权限 */
export function assignDataPermissions(data: API.AssignRoleDataPermissionsRequest) {
  return put<API.AssignRoleDataPermissionsRequest, string>('/api/Role/AssignDataPermissions', data);
}
