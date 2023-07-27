declare namespace API {
  /** 创建应用 */
  type CreateApplicationItem = {
    environment?: string;
    name: string;
    clientId: string;
    frontendUrl?: string;
    apiUrl?: string;
    menuResourceRoute?: string;
    permissionResourceRoute?: string;
    remarks?: string;
  };
  /** 更新应用 */
  type UpdateApplicationItem = {
    id: string;
    name: string;
    clientId: string;
    frontendUrl?: string;
    apiUrl?: string;
    menuResourceRoute?: string;
    permissionResourceRoute?: string;
    remarks?: string;
  };
  /**
   * 应用列表
   */
  type ApplicationListItem = {
    id: string;
    name: string;
    clientId: string;
    clientSecret: string;
    frontendUrl?: string;
    apiUrl?: string;
    menuResourceRoute?: string;
    permissionResourceRoute?: string;
    status: Status;
    remarks?: string;
  } & Partial<CreatedItem> &
    Partial<UpdatedItem>;

  /** 应用详情 */
  type ApplicationDetailItem = Partial<ApplicationListItem>;

  /**
   * 分页请求参数
   */
  type ApplicationPagingParams = {
    environment?: string;
  } & Partial<Record<string, any>>;
}
