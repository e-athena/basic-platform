import { get, paging, post, put } from '@/utils/request';

/** 列表 */
export function query(params: API.UserPagingParams) {
  return paging<API.UserListItem>('/api/User/GetPaging', params);
}

/** 详情 */
export function detail(id: string) {
  return get<API.UserDetailInfo>('/api/User/Get', { id });
}

/** 创建 */
export function create(data: API.CreateUserRequest) {
  return post<API.CreateUserRequest, string>('/api/User/Post', data);
}

/** 更新 */
export function update(data: API.UpdateUserRequest) {
  return put<API.UpdateUserRequest, string>('/api/User/Put', data);
}

/** 资源代码 */
export function queryResourceCodeInfo(id: string) {
  return get<API.UserResourceCodeInfo>('/api/User/GetResourceCodeInfo', { id });
}

/** 分配资源 */
export function assignResources(data: API.AssignUserResourcesRequest) {
  return put<API.AssignUserResourcesRequest, string>('/api/User/AssignResources', data);
}