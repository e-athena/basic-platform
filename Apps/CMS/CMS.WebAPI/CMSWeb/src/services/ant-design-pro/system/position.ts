import { get } from '@/utils/request';

/** 职位下拉列表 */
export function positionList(organizationId?: string) {
  return get<API.SelectInfo[]>('/api/Position/GetSelectList', {
    organizationId: organizationId || null,
  });
}
