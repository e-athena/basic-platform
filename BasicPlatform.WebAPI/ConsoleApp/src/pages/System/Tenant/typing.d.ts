declare namespace API {
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
    expireTime: string;
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
    expireTime: string;
    remarks: string;
    status: number;
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
    expireTime: string | null;
  };
}
