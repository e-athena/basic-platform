import { get } from '@/utils/request';

/** 查询数据 */
export function query(resourceUrl?: string): Promise<ApiResponse<API.ResourceInfo[]>> {
  if (resourceUrl) {
    return get('/api/Resource/GetSubAppResources', { resourceUrl });
  }
  return get('/api/ApiPermission/GetMenuResources');
}

/**
 * 同步资源
 */
export async function sync(): Promise<ApiResponse<boolean>> {
  return get('/api/Resource/Sync');
}
