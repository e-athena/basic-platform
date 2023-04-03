import { get, paging, post, put } from '@/utils/request';

/** 列表 */
export function query(params: API.PositionPagingParams) {
  return paging<API.PositionListItem>('/api/Position/GetPaging', params);
}

/** 详情 */
export function detail(id: string) {
  return get<API.PositionDetailItem>('/api/Position/Get', { id });
}

/** 创建 */
export function create(data: API.CreatePositionItem) {
  return post<API.CreatePositionItem, string>('/api/Position/Post', data);
}

/** 更新 */
export function update(data: API.UpdatePositionItem) {
  return put<API.UpdatePositionItem, string>('/api/Position/Put', data);
}
/** 切换状态 */
export function statusChange(id: string) {
  return put('/api/Position/StatusChange', { id });
}
