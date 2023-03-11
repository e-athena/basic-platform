import { get } from '@/utils/request';

/** 组织架构树形下拉列表(包含自己及下级) */
export function orgTreeSelectForSelf() {
  return get<API.TreeSelectInfo[]>('/api/Organization/GetTreeSelectListForSelf');
}

/** 组织架构树形下拉列表(所有) */
export function orgTreeSelect() {
  return get<API.TreeSelectInfo[]>('/api/Organization/GetTreeSelectList');
}

/** 组织架构树形列表 */
export function orgTree() {
  return get<API.TreeInfo[]>('/api/Organization/GetTreeList');
}