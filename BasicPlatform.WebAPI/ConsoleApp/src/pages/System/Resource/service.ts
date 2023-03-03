import { RequestData } from '@ant-design/pro-components';
import { request } from '@umijs/max';
import { SortOrder } from 'antd/es/table/interface';

/**
 * 同步资源
 */
export async function sync(): Promise<ApiResponse<boolean>> {
  return request('/api/Resource/Sync', {
    method: 'GET',
  });
}

/**
 * 重置资源
 * @returns 
 */
export async function reinitialize(): Promise<ApiResponse<boolean>> {
  return request('/api/Resource/Reinitialize', {
    method: 'GET',
  });
}

/** 查询数据 */
export async function query(
  params: {
    // query
    /** 当前的页码 */
    current?: number;
    /** 页面的容量 */
    pageSize?: number;
    /** 关键字 */
    keyword?: string;
    /** 日期 */
    dateRange?: string[];
  },
  sort: Record<string, SortOrder>,
  options?: { [key: string]: any },
): Promise<RequestData<API.ResourceInfo>> {
  let ext = {};
  if (params.keyword) {
    ext = {
      Q: params.keyword,
    };
  }
  if (Object.keys(sort).length > 0) {
    ext = {
      ...ext,
      sort: Object.keys(sort)[0],
      desc: sort[Object.keys(sort)[0]] === 'descend' ? 'True' : 'False',
    };
  }
  if (params.dateRange && params.dateRange.length === 2) {
    ext = {
      ...ext,
      dtStart: params.dateRange[0],
      dtEnd: params.dateRange[1],
    };
  }
  const res = await request<ApiResponse<API.ResourceInfo[]>>('/api/ApiPermission/GetMenuResources', {
    method: 'GET',
    params: {
      pageIndex: params.current,
      pageSize: params.pageSize,
      ...ext,
    },
    ...(options || {}),
  });
  return {
    data: res.data,
    success: res.success,
    total: 0,
  };
}
