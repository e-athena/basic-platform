import { get, paging, post, put } from '@/utils/request';

/** 列表 */
export function query(params: API.TenantPagingParams) {
  return paging<API.TenantListItem>('/api/Tenant/GetPaging', params);
}

/** 详情 */
export function detail(id: string) {
  return get<API.TenantDetailItem>('/api/Tenant/Get', { id });
}

/** 创建 */
export function create(data: API.CreateTenantItem) {
  return post<API.CreateTenantItem, string>('/api/Tenant/Post', data);
}

/** 更新 */
export function update(data: API.UpdateTenantItem) {
  return put<API.UpdateTenantItem, string>('/api/Tenant/Put', data);
}

/** 切换状态 */
export function statusChange(id: string) {
  return put('/api/Tenant/StatusChange', { id });
}

/** 分配资源 */
export function assignResources(data: API.AssignTenantResourcesRequest) {
  return put<API.AssignTenantResourcesRequest, string>('/api/Tenant/AssignResources', data);
}