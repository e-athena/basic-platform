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
