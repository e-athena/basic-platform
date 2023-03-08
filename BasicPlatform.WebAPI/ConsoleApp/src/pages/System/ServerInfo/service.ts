import { get } from '@/utils/request';

/** 详情 */
export function query() {
  return get<API.ServerInfo>('/api/Server/Get');
}
