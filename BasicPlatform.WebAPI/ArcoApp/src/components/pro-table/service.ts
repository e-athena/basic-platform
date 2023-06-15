import { SelectInfo, TableColumnResponse } from '@/types/global';
import { post, get } from '@/utils/request';

/** 数据列 */
export function queryColumns(modelName: string) {
  return get<TableColumnResponse>(`/api/${modelName}/GetColumns`);
}
/** 详情数据列 */
export function queryDetailColumns(modelName: string) {
  return get<TableColumnResponse>(`/api/${modelName}/GetDetailColumns`);
}

/** 更新数据列 */
export function updateUserCustomColumns(body: any) {
  return post<any, number>('/api/user/updateUserCustomColumns', body);
}

/** 用户下拉列表 */
export function queryUserSelectList(organizationId?: string) {
  return get<SelectInfo[]>('/api/User/GetSelectList', {
    organizationId: organizationId || null,
  });
}
