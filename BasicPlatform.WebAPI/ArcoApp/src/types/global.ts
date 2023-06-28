export interface AnyObject {
  [key: string]: unknown;
}

export interface Options {
  value: unknown;
  label: string;
}

export interface NodeOptions extends Options {
  children?: NodeOptions[];
}

export interface GetParams {
  body: null;
  type: string;
  url: string;
}

export interface PostData {
  body: string;
  type: string;
  url: string;
}

export interface Pagination {
  current: number;
  pageSize: number;
  total?: number;
}

export type TimeRanger = [string, string];

export interface GeneralChart {
  xAxis: string[];
  data: Array<{ name: string; value: number[] }>;
}

/**
 * 下拉列表
 */
export interface SelectInfo {
  label: string;
  value: string;
  disabled: boolean;
  extend?: string;
}
/**
 * 树形下拉列表
 */
export interface TreeSelectInfo {
  id: string;
  parentId?: string;
  title: string;
  value: string;
  disabled: boolean;
  isLeaf: boolean;
  checked: boolean;
  children?: TreeSelectInfo[];
  extend?: string;
}
/**
 * 树形列表
 */
export interface TreeInfo {
  key: string | null;
  title: string;
  disabled: boolean;
  children?: TreeInfo[];
}
/**
 *
 */
export interface CascaderInfo {
  value: string;
  label: string;
  disabled: boolean;
  children?: CascaderInfo[];
}

/**
 * 分页结构
 */
export interface Paging<T = any> {
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
export interface ApiPagingResponse<T = any> {
  data: Paging<T>;
  success: boolean;
  message: string;
  statusCode: number;
  traceId?: string;
}

/**
 * 通用的请求返回结构
 */
export interface ApiResponse<T = any> {
  data: T;
  success: boolean;
  message: string;
  statusCode: number;
  traceId?: string;
}

/** 资源模型 */
export interface ResourceModel {
  applicationId?: string;
  key: string;
  code: string;
}

export interface FilterItem {
  key?: string;
  propertyType?: string;
  label?: string;
  value?: any;
  xor?: string;
  operator?: string;
  groupIndex: number;
  index?: number;
  extras?: SelectInfo[];
}

export interface FilterGroupItem {
  xor: string;
  filters: FilterItem[];
}

export interface ColSelectItem {
  label: string;
  value: string;
  propertyType: string;
  enumOptions: any[];
}

export interface ReportColumnItem {
  xField: string;
  yField: string;
  seriesField: string;
}

export interface QiniuUploadResponse {
  error?: string;
  error_code?: string;
  key?: string;
}

/** 状态变更请求 */
export interface StatusChangeRequest {
  id: string;
}
/** ID请求 */
export interface IdRequest {
  id: string;
}

/** 下载文件 */
export interface DownloadFileInfo {
  fileName: string;
  fileType: string;
  fileToken: string;
}

export interface FunctionInfo {
  parentCode: string;
  key: string;
  label: string;
  value: string;
  values: string[];
  description?: string;
}
/** 资源信息 */
export interface ResourceInfo {
  appId?: string;
  parentCode?: string;
  path: string;
  name: string;
  description?: string;
  code: string;
  icon: string;
  isVisible: boolean;
  isAuth: boolean;
  sort: number;
  id?: number;
  functions?: FunctionInfo[];
  children?: ResourceInfo[];
  [key: string]: any;
}

/** 应用菜单资源 */
export interface ApplicationMenuResourceInfo {
  applicationId: string;
  applicationName: string;
  resources: ResourceInfo[];
}

/** 创建相关属性 */
export interface CreatedInfo {
  createdOn?: string;
  createdUserId?: string;
  createdUserName?: string;
  organizationalUnitIds?: string;
  organizationalUnitIdList?: string[];
}
/** 更新相关属性 */
export interface UpdatedInfo {
  updatedOn?: string;
  lastUpdatedUserId?: string;
  lastUpdatedUserName?: string;
}

export type ExternalPage = {
  id: string;
  type: number;
  parentId?: string;
  name: string;
  path: string;
  icon: string;
  layout: string;
  sort: number;
  remarks?: string;
} & Partial<CreatedInfo> &
  Partial<UpdatedInfo>;

export type TableColumnItem = {
  /** 列名 */
  dataIndex: string;
  /** 列标题 */
  title: string;
  /** 列宽度 */
  width?: number | null;
  /** 在表格中隐藏 */
  hideInTable: boolean;
  /** 在搜索中隐藏 */
  hideInSearch: boolean;
  hideInDescriptions: boolean;
  /** 是否必须 */
  required: boolean;
  /** 固定到左侧 */
  fixed: string;
  /** 排序 */
  sort: number;
  /** 超出宽度显示省略号 */
  ellipsis?: boolean;
  // /** 是否可复制 */
  // copyable?: boolean;
  /** 文字对齐方式 */
  align?: 'left' | 'center' | 'right';
  /** 是否可排序 */
  sorter?: boolean;
  /** 是否可筛选 */
  filters?: boolean;
  /** 数据类型 */
  valueType?: any;
  /** 枚举值类型 */
  valueEnum?: any;
  /** 提示 */
  tooltip?: string;
  /** 属性类型 */
  propertyType: string;
  /** 属性名称 */
  propertyName: string;
  /** 枚举 */
  enumOptions?: SelectInfo[];
  /** 分组 */
  group: string;
  /** 分组详情 */
  groupDescription?: string;
};

/** 表格列响应信息 */
export type TableColumnResponse = {
  /** 模块名 */
  moduleName: string;
  /** 列信息 */
  columns: TableColumnItem[];
};

/** 数据权限属性 */
export type DataPermissionProperty = {
  label: string;
  value: string;
  key: string;
  propertyType?: string;
  enumOptions?: SelectInfo[];
};

/** 数据权限 */
export type DataPermission = {
  appId?: string;
  displayName?: string;
  resourceKey: string;
  dataScope: number;
  enabled: boolean;
  disableChecked: boolean;
  dataScopeCustom?: string;
  dataScopeCustoms: string[];
  properties: DataPermissionProperty[];
  queryFilterGroups: FilterGroupItem[];
  policyResourceKey: string;
};

/** 数据权限组 */
export type DataPermissionGroup = {
  displayName: string;
  items: DataPermission[];
};

/** 应用数据权限资源 */
export type ApplicationDataPermissionResourceInfo = {
  applicationId: string;
  applicationName: string;
  dataPermissionGroups: DataPermissionGroup[];
  extraSelectList?: SelectInfo[];
};

/** 分配数据权限 */
export type DataPermissionItem = {
  applicationId?: string;
  resourceKey: string;
  dataScope: number;
  enabled: boolean;
  dataScopeCustom?: string;
  queryFilterGroups?: FilterGroupItem[];
  policyResourceKey?: string;
};

/** 数据权限 */
export type UserDataPermission = {
  applicationId?: string;
  resourceKey: string;
  dataScope: number;
  enabled: boolean;
  disableChecked?: boolean;
  dataScopeCustom?: string;
  dataScopeCustoms: string[];
  queryFilterGroups?: FilterGroupItem[];
  policyResourceKey: string;
};

/** 分页参数 */
export type PagingParams = {
  current: number;
  pageSize: number;
} & Partial<Record<string, any>>;
