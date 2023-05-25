// @ts-ignore
/* eslint-disable */
import { request } from '@umijs/max';

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
/** 读取授权Token */
export async function getAuthToken(
  params: API.SSOAuthCodeParams,
  options?: { [key: string]: any },
): Promise<ApiResponse<API.LoginResult>> {
  return request<ApiResponse<API.LoginResult>>('/api/sso/getAuthToken', {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
    },
    params,
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
/** 查询应用菜单资源 GET /api/Util/GetApplicationMenuResources */
export async function queryApplicationMenuResources(options?: { [key: string]: any }) {
  return request<ApiResponse<API.ApplicationMenuResourceInfo[]>>(
    '/api/Util/GetApplicationMenuResources',
    {
      method: 'GET',
      ...(options || {}),
    },
  );
}
/** 查询应用数据权限资源 GET /api/Util/GetApplicationMenuResources */
export async function queryApplicationDataPermissionResources(options?: { [key: string]: any }) {
  return request<ApiResponse<API.ApplicationDataPermissionResourceInfo[]>>(
    '/api/Util/GetApplicationDataPermissionResources',
    {
      method: 'GET',
      ...(options || {}),
    },
  );
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
/** 详情数据列 */
export function queryDetailColumns(modelName: string) {
  return request<ApiResponse<API.TableColumnResponse>>(`/api/${modelName}/GetDetailColumns`, {
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

/** 读取子应用 */
export function queryApps() {
  return request<ApiResponse<API.MicroAppInfo[]>>('/api/Util/GetAppList', {
    method: 'GET',
  });
}

/** 读取子应用配置 */
export function queryAppConfig() {
  return request<ApiResponse<API.MicroConfig>>('/api/Util/GetAppConfig', {
    method: 'GET',
  });
}

/** 读取应用列表 */
export function queryAppList() {
  return request<ApiResponse<API.ApplicationListItem[]>>('/api/Application/GetList', {
    method: 'GET',
  });
}
