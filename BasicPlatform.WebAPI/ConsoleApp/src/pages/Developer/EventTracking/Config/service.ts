import { deleteObj, get, paging, post } from '@/utils/request';

/** 列表 */
export function query(params: API.EventTrackingConfigPagingParams) {
  return paging<API.EventTrackingConfigListItem>('/api/EventTrackingConfig/GetPaging', params);
}

/** 详情 */
export function detail(id: string) {
  return get<API.EventTrackingConfigDetailItem>('/api/EventTrackingConfig/Get', { id });
}

/** 保存 */
export function save(data: API.SaveEventTrackingConfigItem) {
  return post<API.SaveEventTrackingConfigItem, string>('/api/EventTrackingConfig/Save', data);
}

/** 删除 */
export function remove(id: string) {
  return deleteObj<number>('/api/EventTrackingConfig/Delete', { id });
}

/** 读取列表 */
export function querySelectList() {
  return get<API.EventTrackingConfigListItem[]>('/api/EventTrackingConfig/GetSelectList');
}