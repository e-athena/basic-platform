import { ApiPagingResponse, ApiResponse } from '@/types/global';
import axios from 'axios';

/**
 * 读取分页数据
 * @param url 地址
 * @param data 参数
 * @returns 结果集
 */
export async function paging<T>(
  url: string,
  data: Record<string, object>
): Promise<ApiPagingResponse<T>> {
  return axios.post(url, data);
}
/**
 * GET请求
 * @param url
 * @param params
 * @returns
 */
export async function get<T>(
  url: string,
  params?: { [key: string]: any }
): Promise<ApiResponse<T>> {
  return axios.get(url, {
    method: 'GET',
    params: params || {},
  });
}

/**
 * POST请求
 * @param url 地址
 * @param data 数据
 * @returns 结果集
 */
export async function post<T, TR>(
  url: string,
  data: T
): Promise<ApiResponse<TR>> {
  return axios.post(url, data);
}

/**
 * DELETE请求
 * @param url
 * @param params
 * @returns
 */
export async function deleteObj<T>(
  url: string,
  data?: { [key: string]: any }
): Promise<ApiResponse<T>> {
  return axios.delete(url, { data });
}

/**
 * PUT请求
 * @param url 地址
 * @param data 数据
 * @returns 结果集
 */
export async function put<T, TR>(
  url: string,
  data?: T
): Promise<ApiResponse<TR>> {
  return axios.put(url, { data });
}
