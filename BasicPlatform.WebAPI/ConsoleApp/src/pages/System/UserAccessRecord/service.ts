import { paging } from '@/utils/request';

/** 列表 */
export function query(params: Record<string, any>) {
  return paging<API.UserAccessRecordListItem>('/api/UserAccessRecord/GetPaging', params);
}
