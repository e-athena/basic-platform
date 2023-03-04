import { get, paging, post, put } from '@/utils/request';

/** 查询数据 */
export function query(params: API.RolePagingParams) {
  return paging<API.RoleListItem>('/api/Role/GetPaging', params);
}

/** 查询详情 */
export function detail(id: string) {
  return get<API.RoleDetailItem>(`/api/Role/Get`, { id: id });
}

/** 创建数据 */
export function create(data: API.CreateRoleItem) {
  return post('/api/Role/Post', data);
}

/** 更新数据 */
export function update(data: API.UpdateRoleItem) {
  return put('/api/Role/Put', data);
}
