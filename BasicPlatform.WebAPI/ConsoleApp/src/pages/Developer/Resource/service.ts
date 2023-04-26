import { get } from '@/utils/request';

/** 查询数据 */
export function query(): Promise<ApiResponse<API.ResourceInfo[]>> {
  return get('/api/ApiPermission/GetMenuResources');
}

/**
 * 同步资源
 */
export async function sync(): Promise<ApiResponse<boolean>> {
  return get('/api/Resource/Sync');
}
