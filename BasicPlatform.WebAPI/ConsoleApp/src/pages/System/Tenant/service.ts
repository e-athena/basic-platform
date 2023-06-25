import { get, paging, post, put } from '@/utils/request';

/** 列表 */
export function query(params: API.TenantPagingParams) {
  return paging<API.TenantListItem>('/api/Tenant/GetPaging', params);
  // return paging<API.TenantListItem>('/api/Tenant/GetPaging?tenant_id=xiaomi', params);
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

/** 读取超级管理员信息 */
export function querySuperAdmin(code: string) {
  return get<API.TenantSuperAdminItem>('/api/Tenant/GetSuperAdmin', { tenant_id: code });
}
/** 创建超级管理员 */
export function createSuperAdmin(data: API.CreateTenantSuperAdminRequest) {
  return post<API.CreateTenantSuperAdminRequest, string>(`/api/Tenant/CreateSuperAdmin?tenant_id=${data.code}`, data);
}
/** 更新超级管理员 */
export function updateSuperAdmin(data: API.UpdateTenantSuperAdminRequest) {
  return put<API.UpdateTenantSuperAdminRequest, string>(`/api/Tenant/UpdateSuperAdmin?tenant_id=${data.code}`, data);
}

/** 同步数据库结构 */
export function syncStructure(code: string) {
  return get<string>('/api/Tenant/SyncStructure', { code });
}
