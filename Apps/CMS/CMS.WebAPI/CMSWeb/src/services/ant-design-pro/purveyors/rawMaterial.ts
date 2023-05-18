import { get } from '@/utils/request';

/** 下拉列表 */
export function queryRawMaterialSelectList(params: any) {
  return get<API.SelectInfo[]>('/api/RawMaterial/getSelectList', params);
}
