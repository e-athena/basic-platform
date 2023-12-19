import { get, paging } from '@/utils/request';

/** 列表 */
export function query(params: API.LogPagingParams) {
  return paging<API.LogListItem>('/api/Log/GetPaging', params);
}
/** 详情 */
export function detail(id: number, serviceName: string) {
  return get<API.LogDetail>('/api/Log/GetById', { id, serviceName });
}

/** 读取下拉选项 */
export async function getServiceSelectOptions() {
  return get<API.SelectInfo[]>('/api/Log/GetServiceSelectList');
}