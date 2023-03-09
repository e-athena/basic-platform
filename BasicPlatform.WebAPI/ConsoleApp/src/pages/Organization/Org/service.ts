import { get, paging, post, put } from '@/utils/request';

/** 列表 */
export function query(params: API.OrgPagingParams) {
  return paging<API.OrgListItem>('/api/Organization/GetPaging', params);
}

/** 树形列表 */
export function queryTreeList(data: API.OrgTreeListParams) {
  return post<API.OrgTreeListParams, API.OrgTreeListItem[]>('/api/Organization/GetTreeList', data);
}

/** 详情 */
export function detail(id: string) {
  return get<API.OrgDetailItem>('/api/Organization/Get', { id });
}

/** 创建 */
export function create(data: API.CreateOrgRequest) {
  return post<API.CreateOrgRequest, string>('/api/Organization/Post', data);
}

/** 更新 */
export function update(data: API.UpdateOrgRequest) {
  return put<API.UpdateOrgRequest, string>('/api/Organization/Put', data);
}

/** 切换状态 */
export function statusChange(id: string) {
  return put('/api/Organization/StatusChange', { id });
}