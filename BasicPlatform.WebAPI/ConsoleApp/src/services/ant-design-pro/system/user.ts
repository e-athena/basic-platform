import { get } from '@/utils/request';

/** 用户下拉列表 */
export function userList(organizationId?: string) {
  return get<API.SelectInfo[]>('/api/User/GetSelectList', {
    organizationId: organizationId || null,
  });
}

/** 用户下拉列表 */
export function querySelectList(organizationId?: string) {
  return get<API.SelectInfo[]>('/api/User/GetSelectList', {
    organizationId: organizationId || null,
  });
}
