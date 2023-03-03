import { paging } from '@/utils/request';

/** 查询数据 */
export function query(params: API.RolePagingParams) {
  return paging<API.RoleListItem>('/api/Role/GetPaging', params);
}