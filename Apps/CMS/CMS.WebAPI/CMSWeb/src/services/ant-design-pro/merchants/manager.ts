import { get } from '@/utils/request';

/** 代理商列表 */
export function queryCityAgentSelectList() {
  return get<API.SelectInfo[]>('/api/Manager/getCityAgentSelectList');
}
/** 下拉列表 */
export function queryManagerSelectList() {
  return get<API.SelectInfo[]>('/api/Manager/getSelectList');
}
/** 级联列表 */
export function queryManagerCascaderList() {
  return get<API.CascaderInfo[]>('/api/Manager/getCascaderList');
}
