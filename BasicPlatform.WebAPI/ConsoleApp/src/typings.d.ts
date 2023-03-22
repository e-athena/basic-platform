declare module 'slash2';
declare module '*.css';
declare module '*.less';
declare module '*.scss';
declare module '*.sass';
declare module '*.svg';
declare module '*.png';
declare module '*.jpg';
declare module '*.jpeg';
declare module '*.gif';
declare module '*.bmp';
declare module '*.tiff';
declare module 'omit.js';
declare module 'numeral';
declare module '@antv/data-set';
declare module 'mockjs';
declare module 'react-fittext';
declare module 'bizcharts-plugin-slider';

declare const REACT_APP_ENV: 'test' | 'dev' | 'pre' | false;
// 以下变量声明对应config.[env].ts文件内define的变量
declare const API_URL: string;
declare const APP_TOKEN_KEY: string;

/**
 * 分页结构
 */
declare interface Paging<T = any> {
  /**
   * 数据集
   */
  items?: T[];
  /**
   * 当前页码
   */
  currentPage: number;
  /**
   * 总页数
   */
  totalPages: number;
  /**
   * 总记录数
   */
  totalItems: number;
  /**
   * 每页的记录数
   */
  itemsPerPage: number;
  /**
   * 是否为第一页
   */
  isFirstPage: boolean;
  /**
   * 是否为最后一页
   */
  isLastPage: boolean;
}
/**
 * 分页请求返回结构
 */
declare interface ApiPagingResponse<T = any> {
  data?: Paging<T>;
  success: boolean;
  message: string;
  statusCode: number;
  traceId?: string;
}

/**
 * 通用的请求返回结构
 */
declare interface ApiResponse<T = any> {
  data?: T;
  success: boolean;
  message: string;
  statusCode: number;
  traceId?: string;
}

/** 资源模型 */
declare interface ResourceModel {
  key: string;
  code: string;
}
