import { get } from '@/utils/request';

/** 场地租赁方下拉列表 */
export function querySiteLessorSelectList() {
  return get<API.SelectInfo[]>('/api/SiteLessor/getSelectList');
}
/** 资本方下拉列表 */
export function queryCapitalProviderSelectList() {
  return get<API.SelectInfo[]>('/api/CapitalProvider/getSelectList');
}
/** 供应商下拉列表 */
export function queryRawMaterialSupplierSelectList() {
  return get<API.SelectInfo[]>('/api/RawMaterialSupplier/getSelectList');
}
