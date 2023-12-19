﻿import type { RequestOptions } from '@@/plugin-request/request';
import type { RequestConfig } from '@umijs/max';
import { Modal, message, notification } from 'antd';
import { getToken, removeToken } from '@/utils/token';
import { history } from '@umijs/max';
import { isQiankun } from './utils/utils';

// 错误处理方案： 错误类型
enum ErrorShowType {
  SILENT = 0,
  WARN_MESSAGE = 1,
  ERROR_MESSAGE = 2,
  NOTIFICATION = 3,
  REDIRECT = 9,
}
// 与后端约定的响应数据格式
interface ResponseStructure {
  success: boolean;
  data: any;
  statusCode: number;
  errorCode?: number;
  errorMessage?: string;
  showType?: ErrorShowType;
}

/**
 * @name 错误处理
 * pro 自带的错误处理， 可以在这里做自己的改动
 * @doc https://umijs.org/docs/max/request#配置
 */
export const errorConfig: RequestConfig = {
  // 错误处理： umi@3 的错误处理方案。
  errorConfig: {
    // 错误抛出
    errorThrower: (res) => {
      const { success, data, errorCode, errorMessage, showType, statusCode } =
        res as unknown as ResponseStructure;
      if (!success && statusCode === 500) {
        const error: any = new Error(errorMessage);
        error.name = 'BizError';
        error.info = { errorCode, errorMessage, showType, data };
        throw error; // 抛出自制的错误
      }
    },
    // 错误接收及处理
    errorHandler: (error: any, opts: any) => {
      if (opts?.skipErrorHandler) throw error;
      // 我们的 errorThrower 抛出的错误。
      if (error.name === 'BizError') {
        const errorInfo: ResponseStructure | undefined = error.info;
        if (errorInfo) {
          const { errorMessage, errorCode } = errorInfo;
          switch (errorInfo.showType) {
            case ErrorShowType.SILENT:
              // do nothing
              break;
            case ErrorShowType.WARN_MESSAGE:
              message.warning(errorMessage);
              break;
            case ErrorShowType.ERROR_MESSAGE:
              message.error(errorMessage);
              break;
            case ErrorShowType.NOTIFICATION:
              notification.open({
                description: errorMessage,
                message: errorCode,
              });
              break;
            case ErrorShowType.REDIRECT:
              // TODO: redirect
              break;
            default:
              message.error(errorMessage);
          }
        }
      } else if (error.response) {
        // Axios 的错误
        // 请求成功发出且服务器也响应了状态码，但状态代码超出了 2xx 的范围
        switch (error.response.status) {
          case 401:
            removeToken();
            if (!isQiankun()) {
              Modal.destroyAll();
              Modal.confirm({
                title: '系统提示',
                content: '登录状态已过期，您可以继续留在该页面，或者重新登录',
                okText: '重新登录',
                cancelText: '取消',
                onOk: () => {
                  const { pathname, search } = history.location;
                  history.push(`${LOGIN_PATH}?redirect=${pathname}${search}`);
                },
              });
            } else {
              const { pathname, search } = history.location;
              history.push(`${LOGIN_PATH}?redirect=${pathname}${search}`);
            }
            break;
          case 400:
            Modal.destroyAll();
            Modal.error({
              title: '系统错误提示(400)',
              content: '请求参数错误，请联系管理员！',
            });
            break;
          case 403:
            message.error('资源未授权');
            break;
          case 404:
            message.error('资源不存在');
            break;
          case 500:
            notification.open({
              type: 'error',
              message: '服务器内部错误',
              description: `请联系管理员并提供追踪ID：${error.response.data.traceId}`,
            });
            break;
          default:
            Modal.destroyAll();
            if (error.response.status === 0) {
              Modal.confirm({
                title: '系统错误提示',
                content: '连接到服务器失败，请检查网络或联系管理员处理。',
                okText: '知道了',
                cancelText: '刷新页面',
                onCancel: () => {
                  window.location.reload();
                },
              });
            } else {
              Modal.error({
                title: '系统错误提示',
                content: '发生未知错误，请重试或联系管理员！',
              });
            }
            break;
        }
      } else if (error.request) {
        // 请求已经成功发起，但没有收到响应
        // \`error.request\` 在浏览器中是 XMLHttpRequest 的实例，
        // 而在node.js中是 http.ClientRequest 的实例
        message.error('None response! Please retry.');
      } else {
        // 发送请求时出了点问题
        message.error('Request error, please retry.');
      }
    },
  },

  // 请求拦截器
  requestInterceptors: [
    (config: RequestOptions) => {
      // 拦截请求配置，进行个性化处理
      const url = `${API_URL}${config?.url}`;
      const authHeader: any = {
        Authorization: getToken(),
      };
      if (isQiankun()) {
        authHeader.AppId = APPID;
      }
      config.headers = { ...config.headers, ...authHeader };
      return { ...config, url };
    },
  ],

  // 响应拦截器
  responseInterceptors: [
    (response) => {
      // // 拦截响应数据，进行个性化处理
      // const { data } = response as unknown as ResponseStructure;

      // if (data?.success === false) {
      //   message.error('请求失败！');
      // }
      return response;
    },
  ],
};
