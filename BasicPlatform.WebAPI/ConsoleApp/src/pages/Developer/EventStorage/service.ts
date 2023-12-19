import { get, paging } from '@/utils/request';

/** 列表 */
export function query(params: API.EventStoragePagingParams) {
  return paging<API.EventStorageListItem>('/api/EventStorage/GetPaging', params);
}
/** 详情 */
export function detail(id: number) {
  return get<string>('/api/EventStorage/Get', { id });
}