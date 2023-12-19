declare namespace API {

  /** 同步数据库结构请求 */
  type SyncTenantDatabaseRequest = {
    code: string
  }

  /** 创建超级管理员 */
  type CreateTenantSuperAdminRequest = {
    userName: string;
    password: string;
    avatar: string;
    realName: string;
    gender: number;
    nickName: string;
    phoneNumber: string;
    email: string;
    code: string;
  };

  /** 更新超级管理员 */
  type UpdateTenantSuperAdminRequest = {
    id: string;
  } & Partial<CreateTenantSuperAdminRequest>;

  /** 超级管理员信息 */
  type TenantSuperAdminItem = {
    id?: string;
    userName: string;
    password?: string;
    avatar?: string;
    realName: string;
    gender?: number;
    nickName: string;
    phoneNumber: string;
    email?: string;
    code: string;
  }

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
    isolationLevel: number,
    contactName: string;
    contactPhoneNumber: string;
    contactEmail?: string;
    connectionString?: string;
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
    applicationName: string;
    appId: string;
    isolationLevel: number,
    connectionString: string | null;
    expiredTime: string | null;
  };
}
