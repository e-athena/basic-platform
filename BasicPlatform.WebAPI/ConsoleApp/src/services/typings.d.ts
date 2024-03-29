// @ts-ignore
/* eslint-disable */

declare namespace API {
  /** 创建相关属性 */
  type CreatedItem = {
    createdOn?: string;
    createdUserId?: string;
    createdUserName?: string;
  };
  /** 更新相关属性 */
  type UpdatedItem = {
    updatedOn?: string;
    updatedUserId?: string;
    updatedUserName?: string;
  };

  /** 状态 */
  enum Status {
    /** 启用 */
    Enabled = 1,
    /** 禁用 */
    Disabled = 2,
  }

  /** 表格列响应信息 */
  type TableColumnResponse = {
    /** 模块名 */
    moduleName: string;
    /** 列信息 */
    columns: TableColumnItem[];
  };

  type TableColumnItem = {
    /** 是否已过滤 */
    ignore: boolean;
    /** 是否已过滤 */
    tableIgnore: boolean;
    /** 列名 */
    dataIndex: string;
    /** 列标题 */
    title: string;
    /** 列宽度 */
    width: number | null;
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
    enumOptions?: any[];
    /** 分组 */
    group: string;
    /** 分组详情 */
    groupDescription?: string;
  };

  /** 应用数据权限资源 */
  type ApplicationDataPermissionResourceInfo = {
    applicationId: string;
    applicationName: string;
    dataPermissionGroups: DataPermissionGroup[];
    extraSelectList?: SelectInfo[];
  };

  /** 数据权限组 */
  type DataPermissionGroup = {
    displayName: string;
    items: DataPermission[];
  };

  /** 数据权限 */
  type DataPermission = {
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

  /** 分配数据权限 */
  type DataPermissionItem = {
    appId?: string;
    resourceKey: string;
    dataScope: number;
    enabled: boolean;
    dataScopeCustom?: string;
    queryFilterGroups?: FilterGroupItem[];
    policyResourceKey?: string;
  };

  /** 数据权限属性 */
  type DataPermissionProperty = {
    label: string;
    value: string;
    key: string;
    propertyType?: string;
    enumOptions?: SelectInfo[];
  };

  /** 数据权限 */
  type UserDataPermission = {
    appId?: string;
    resourceKey: string;
    dataScope: number;
    enabled: boolean;
    disableChecked?: boolean;
    dataScopeCustom?: string;
    dataScopeCustoms: string[];
    queryFilterGroups?: FilterGroupItem[];
    policyResourceKey: string;
  };

  /** 分配列权限 */
  type ColumnPermissionItem = {
    appId?: string;
    enabled: boolean;
    columnType: string;
    columnKey: string;
    isEnableDataMask: boolean;
    dataMaskType: number;
    maskLength: number;
    maskPosition: number;
    maskChar: string;
  };
}
