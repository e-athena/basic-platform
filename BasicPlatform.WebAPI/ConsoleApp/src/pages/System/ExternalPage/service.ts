import { get, paging, post, put, deleteObj } from '@/utils/request';

/** 列表 */
export function query(params: API.ExternalPagePagingParams) {
  return paging<API.ExternalPageListItem>('/api/ExternalPage/GetPaging', params);
}

/** 详情 */
export function detail(id: string) {
  return get<API.ExternalPageDetailItem>('/api/ExternalPage/Get', { id });
}

/** 创建 */
export function create(data: API.CreateExternalPageRequest) {
  return post<API.CreateExternalPageRequest, string>('/api/ExternalPage/Post', data);
}
/** 更新 */
export function update(data: API.UpdateExternalPageRequest) {
  return put<API.UpdateExternalPageRequest, string>('/api/ExternalPage/Put', data);
}
/** 删除 */
export function remove(id: string) {
  return deleteObj('/api/ExternalPage/Delete', { id });
}
/** 树形列表 */
export function treeList() {
  return get<API.TreeInfo[]>('/api/ExternalPage/GetTreeList');
}
/** 下拉列表 */
export function selectList() {
  return get<API.TreeSelectInfo[]>('/api/ExternalPage/GetSelectList');
}