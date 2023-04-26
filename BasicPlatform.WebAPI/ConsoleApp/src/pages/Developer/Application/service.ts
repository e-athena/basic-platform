import { get, paging, post, put } from '@/utils/request';

/** 列表 */
export function query(params: API.ApplicationPagingParams) {
  return paging<API.ApplicationListItem>('/api/Application/GetPaging', params);
}

/** 详情 */
export function detail(id: string) {
  return get<API.ApplicationDetailItem>('/api/Application/Get', { id });
}

/** 创建 */
export function create(data: API.CreateApplicationItem) {
  return post<API.CreateApplicationItem, string>('/api/Application/Post', data);
}

/** 更新 */
export function update(data: API.UpdateApplicationItem) {
  return put<API.UpdateApplicationItem, string>('/api/Application/Put', data);
}

/** 切换状态 */
export function statusChange(id: string) {
  return put('/api/Application/StatusChange', { id });
}