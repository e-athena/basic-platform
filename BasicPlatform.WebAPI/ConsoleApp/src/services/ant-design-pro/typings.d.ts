// @ts-ignore
/* eslint-disable */

declare namespace API {
  type CurrentUser = {
    userName?: string;
    realName?: string;
    avatar?: string;
    userId?: string;
    email?: string;
    signature?: string;
    title?: string;
    group?: string;
    tags?: { key?: string; label?: string }[];
    notifyCount?: number;
    unreadCount?: number;
    country?: string;
    geographic?: {
      province?: { label?: string; key?: string };
      city?: { label?: string; key?: string };
    };
    address?: string;
    phoneNumber?: string;
  };

  type LoginResult = {
    status?: string;
    type?: string;
    currentAuthority?: string;
    errorMessage?: string;
  };

  type PageParams = {
    current?: number;
    pageSize?: number;
  };

  type FakeCaptcha = {
    code?: number;
    status?: string;
  };

  type LoginParams = {
    username?: string;
    password?: string;
    autoLogin?: boolean;
    type?: string;
  };

  type ErrorResponse = {
    /** 业务约定的错误码 */
    errorCode: string;
    /** 业务上的错误信息 */
    errorMessage?: string;
    /** 业务上的请求是否成功 */
    success?: boolean;
  };

  type NoticeIconList = {
    data?: NoticeIconItem[];
    /** 列表的内容总数 */
    total?: number;
    success?: boolean;
  };

  type NoticeIconItemType = 'notification' | 'message' | 'event';

  type NoticeIconItem = {
    id?: string;
    extra?: string;
    key?: string;
    read?: boolean;
    avatar?: string;
    title?: string;
    status?: string;
    datetime?: string;
    description?: string;
    type?: NoticeIconItemType;
  };

  type ResourceInfo = {
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
  };

  type FunctionInfo = {
    parentCode: string;
    key: string;
    label: string;
    value: string;
    values: string[];
    description?: string;
  };

  type ExternalPage = {
    id: string;
    type: number;
    parentId?: string;
    name: string;
    path: string;
    icon: string;
    layout: string;
    sort: number;
    remarks?: string;
  } & Partial<CreatedItem> &
    Partial<UpdatedItem>;

  /**
   * 下拉列表
   */
  type SelectInfo = {
    label: string;
    value: string;
    disabled: boolean;
    extend?: string;
  };
  /**
   * 树形下拉列表
   */
  type TreeSelectInfo = {
    id: string;
    parentId?: string;
    title: string;
    value: string;
    disabled: boolean;
    isLeaf: boolean;
    checked: boolean;
    children?: TreeSelectInfo[];
    extend?: string;
  };
  /**
   * 树形列表
   */
  type TreeInfo = {
    key: string | null;
    title: string;
    disabled: boolean;
    children?: TreeInfo[];
  };

  type AddUserAccessRecordParams = {
    accessUrl: string;
  };
}
