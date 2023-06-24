declare namespace API {

  /** 同步数据库结构请求 */
  type SyncTenantDatabaseRequest = {
    code: string
  }

  /** 初始化 */
  type InitTenantRequest = {
    id: string;
    code: string;
    adminUserName?: string;
    adminPassword?: string;
  };

  /** 创建 */
  type CreateTenantItem = {
    name: string;
    code: string;
    contactName: string;
    contactPhoneNumber: string;
    contactEmail?: string;
    connectionString: string;
    remarks: string;
    effectiveTime: string;
    expiredTime: string;
    applications: TenantApplicationItem[];
  };
  /** 更新 */
  type UpdateTenantItem = {
    id: string;
  } & Partial<CreateTenantItem>;
  /**
   * 列表
   */
  type TenantListItem = {
    id: string;
    name: string;
    code: string;
    contactName: string;
    contactPhoneNumber: string;
    contactEmail?: string;
    connectionString: string;
    effectiveTime: string;
    expiredTime: string;
    remarks: string;
    status: number;
    isInitDatabase: boolean;
  } & Partial<CreatedItem> &
    Partial<UpdatedItem>;

  /** 详情 */
  type TenantDetailItem = {
    resources: ResourceModel[];
    applications: TenantApplicationItem[];
  } & Partial<TenantListItem>;

  /**
   * 分页请求参数
   */
  type TenantPagingParams = {
    name?: string;
  } & Partial<Record<string, any>>;

  /** 分配资源 */
  type AssignTenantResourcesRequest = {
    id: string;
    resources: ResourceModel[];
  };
  /** 租户应用 */
  type TenantApplicationItem = {
    tenantId: string;
    isEnabled: boolean;
    applicationId: string;
    applicationName: string;
    connectionString: string | null;
    expiredTime: string | null;
  };
}
