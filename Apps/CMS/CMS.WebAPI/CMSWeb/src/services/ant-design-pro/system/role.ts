import { get } from '@/utils/request';

/** 角色下拉列表 */
export function roleList() {
  return get<API.SelectInfo[]>('/api/Role/GetSelectList');
}
