import { get } from '@/utils/request';

/** 列表 */
export function queryTreeSelectList() {
  return get<API.TreeSelectInfo[]>('/api/Region/getRegions');
}
/** 列表 */
export function queryTreeList() {
  return get<API.TreeSelectInfo[]>('/api/Region/getCityTreeSelectList');
}
