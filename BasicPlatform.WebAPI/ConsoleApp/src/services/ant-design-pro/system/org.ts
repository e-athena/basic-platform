import { get } from '@/utils/request';

/** 组织架构树形下拉列表(所有) */
export function orgTreeSelect() {
  return get<API.TreeSelectInfo[]>('/api/Organization/GetTreeSelectList');
}
/** 组织架构树形列表 */
export function orgTree() {
  return get<API.TreeInfo[]>('/api/Organization/GetTreeList');
}
/** 组织架构树形列表 */
export function orgCascader() {
  return get<API.CascaderInfo[]>('/api/Organization/GetCascaderList');
}
/** 组织/部门下拉列表 */
export function orgList(parentId?: string) {
  return get<API.SelectInfo[]>('/api/Organization/GetSelectList', {
    parentId: parentId || null,
  });
}
