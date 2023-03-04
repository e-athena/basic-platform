declare namespace API {
  /** 创建角色 */
  type CreateRoleItem = {
    name: string;
    remarks?: string;
    resourceCodes: string[];
  };
  /** 更新角色 */
  type UpdateRoleItem = {
    id: string;
    name: string;
    remarks?: string;
    resourceCodes: string[];
  };
  /**
   * 角色列表
   */
  type RoleListItem = {
    id: string;
    name: string;
    remarks?: string;
    status: Status;
  } & Partial<CreatedItem> &
    Partial<UpdatedItem>;

  /** 角色详情 */
  type RoleDetailItem = {
    resourceCodes: string[];
  } & Partial<RoleListItem>;

  /**
   * 分页请求参数
   */
  type RolePagingParams = {
    name?: string;
  } & Partial<Record<string, any>>
}
