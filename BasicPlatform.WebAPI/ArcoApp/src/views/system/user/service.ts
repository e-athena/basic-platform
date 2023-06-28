import {
  ApiResponse,
  TableColumnItem,
  UserDataPermission,
} from '@/types/global';
import { get, paging, post, put } from '@/utils/request';
import {
  UserPagingParams,
  UserListItem,
  UserDetailInfo,
  CreateUserRequest,
  UpdateUserRequest,
  UserResourceCodeInfo,
  AssignUserResourcesRequest,
  AssignUserDataPermissionsRequest,
} from './typing';

/** 数据列 */
export function queryColumns() {
  return get<TableColumnItem[]>('/api/User/GetColumns');
}

/** 列表 */
export function query(params: UserPagingParams) {
  return paging<UserListItem>('/api/User/GetPaging', params);
}

/** 详情 */
export function detail(id: string) {
  return get<UserDetailInfo>('/api/User/Get', { id });
}

/** 创建 */
export function create(data: CreateUserRequest) {
  return post<CreateUserRequest, string>('/api/User/Post', data);
}

/** 更新 */
export function update(data: UpdateUserRequest) {
  return put<UpdateUserRequest, string>('/api/User/Put', data);
}

/** 资源代码 */
export function queryResourceCodeInfo(id: string) {
  return get<UserResourceCodeInfo>('/api/User/GetResourceCodeInfo', { id });
}

/** 分配资源 */
export function assignResources(data: AssignUserResourcesRequest) {
  return put<AssignUserResourcesRequest, string>(
    '/api/User/AssignResources',
    data
  );
}
/** 数据权限列表 */
export function dataPermission(id: string) {
  return get<UserDataPermission[]>('/api/User/GetDataPermissions', { id });
}
/** 分配数据权限 */
export function assignDataPermissions(data: AssignUserDataPermissionsRequest) {
  return put<AssignUserDataPermissionsRequest, string>(
    '/api/User/AssignDataPermissions',
    data
  );
}
/** 切换状态 */
export function statusChange(id: string) {
  return put('/api/User/StatusChange', { id });
}

/** 重置密码 */
export function resetPassword(id: string): Promise<ApiResponse<string>> {
  return post('/api/User/ResetPassword', { id });
}
