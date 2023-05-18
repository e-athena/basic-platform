// @ts-ignore
/* eslint-disable */
import { request } from '@umijs/max';
import { get } from '@/utils/request';

/** 获取当前的用户 GET /api/currentUser */
export async function currentUser(options?: { [key: string]: any }) {
  return request<{
    data: API.CurrentUser;
  }>('/api/Account/currentUser', {
    method: 'GET',
    ...(options || {}),
  });
}

/** 退出登录接口 POST /api/login/outLogin */
export async function outLogin(options?: { [key: string]: any }) {
  return request<Record<string, any>>('/api/login/outLogin', {
    method: 'POST',
    ...(options || {}),
  });
}

/** 登录接口 POST /api/login/account */
export async function login(
  body: API.LoginParams,
  options?: { [key: string]: any },
): Promise<ApiResponse<API.LoginResult>> {
  return request<ApiResponse<API.LoginResult>>('/api/account/login', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    data: body,
    skipErrorHandler: true,
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 GET /api/notices */
export async function getNotices(options?: { [key: string]: any }) {
  return request<API.NoticeIconList>('/api/notices', {
    method: 'GET',
    ...(options || {}),
  });
}

/** 查询外部页面 GET /api/User/GetExternalPages */
export async function queryExternalPages(options?: { [key: string]: any }) {
  return request<ApiResponse<API.ExternalPage[]>>('/api/User/GetExternalPages', {
    method: 'GET',
    ...(options || {}),
  });
}
/** 查询菜单 GET /api/User/GetResources */
export async function queryUserResources(options?: { [key: string]: any }) {
  return request<ApiResponse<API.ResourceInfo[]>>('/api/User/GetResources', {
    method: 'GET',
    ...(options || {}),
  });
}
/** 查询菜单 GET /api/ApiPermission/GetMenuResources */
export async function queryMenus(options?: { [key: string]: any }) {
  return request<ApiResponse<API.ResourceInfo[]>>('/api/ApiPermission/GetMenuResources', {
    method: 'GET',
    ...(options || {}),
  });
}

/** 添加访问记录 POST /api/account/addUserAccessRecord */
export async function addUserAccessRecord(data: API.AddUserAccessRecordParams) {
  return request<number>('/api/account/addUserAccessRecord', {
    method: 'POST',
    data,
  });
}

/** 数据列 */
export function queryColumns(modelName: string) {
  return request<ApiResponse<API.TableColumnResponse>>(`/api/${modelName}/GetColumns`, {
    method: 'GET',
  });
}

/** 更新数据列 */
export function updateUserCustomColumns(body: any) {
  return request<ApiResponse<number>>('/api/user/updateUserCustomColumns', {
    method: 'POST',
    data: body,
  });
}

// 读取七牛上传token
export async function getUploadToken() {
  return get<string>('/api/QiNiuCloud/getUploadToken');
}
