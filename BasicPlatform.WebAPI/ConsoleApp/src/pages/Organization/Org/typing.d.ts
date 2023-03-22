declare namespace API {
  /** 创建 */
  type CreateOrgRequest = {
    parentId?: string;
    name: string;
    remarks?: string;
    leaderId?: string;
    status: Status;
    sort: number;
    roleIds: string[];
  };
  /** 更新 */
  type UpdateOrgRequest = {
    id: string;
  } & Partial<CreateOrgRequest>;
  /**
   * 列表
   */
  type OrgListItem = {
    id?: string;
    parentId?: string;
    name?: string;
    parentPath?: string;
    leaderId?: string;
    remarks?: string;
    status?: Status;
    sort?: number;
  } & Partial<CreatedItem> &
    Partial<UpdatedItem>;

  /**
   * 树形列表
   */
  type OrgTreeListItem = {
    children: OrgTreeListItem[];
  } & Partial<OrgListItem>;

  /** 详情 */
  type OrgDetailItem = {
    roleIds?: string[];
  } & Partial<OrgListItem>;

  /**
   * 分页请求参数
   */
  type OrgPagingParams = {
    name?: string;
    status?: Status[];
  } & Partial<Record<string, any>>;

  /**
   * 分页请求参数
   */
  type OrgPagingParams = {
    status?: Status[];
  } & Partial<Record<string, any>>;
  /**
   * 树形请求参数
   */
  type OrgTreeListParams = {
    status?: Status[];
  } & Partial<Record<string, any>>;
}
