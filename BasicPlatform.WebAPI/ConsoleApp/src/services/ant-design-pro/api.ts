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
/** 读取授权码 */
export async function getAuthCode(
  params: API.SSOAuthCodeParams,
  options?: { [key: string]: any },
): Promise<ApiResponse<API.LoginResult>> {
  return request<ApiResponse<API.LoginResult>>('/api/sso/getAuthCode', {
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

/** 获取规则列表 GET /api/rule */
export async function rule(
  params: {
    // query
    /** 当前的页码 */
    current?: number;
    /** 页面的容量 */
    pageSize?: number;
  },
  options?: { [key: string]: any },
) {
  return request<API.RuleList>('/api/rule', {
    method: 'GET',
    params: {
      ...params,
    },
    ...(options || {}),
  });
}

/** 新建规则 PUT /api/rule */
export async function updateRule(options?: { [key: string]: any }) {
  return request<API.RuleListItem>('/api/rule', {
    method: 'PUT',
    ...(options || {}),
  });
}

/** 新建规则 POST /api/rule */
export async function addRule(options?: { [key: string]: any }) {
  return request<API.RuleListItem>('/api/rule', {
    method: 'POST',
    ...(options || {}),
  });
}

/** 删除规则 DELETE /api/rule */
export async function removeRule(options?: { [key: string]: any }) {
  return request<Record<string, any>>('/api/rule', {
    method: 'DELETE',
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
