import { get, post } from '@/utils/request';

/** 详情 */
export function detail() {
  return get<WebSetting>('/api/WebSetting/Get');
}

/** 保存 */
export function save(data: WebSetting) {
  return post<WebSetting, string>('/api/WebSetting/Save', data);
}