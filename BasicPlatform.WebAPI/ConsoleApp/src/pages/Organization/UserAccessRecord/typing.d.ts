declare namespace API {
  /** 创建角色 */
  type CreateRoleItem = {
    name: string;
    remarks?: string;
    resources: ResourceModel[];
  };
  /** 更新角色 */
  type UpdateRoleItem = {
    id: string;
    name: string;
    remarks?: string;
    resources: ResourceModel[];
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
    resources: ResourceModel[];
  } & Partial<RoleListItem>;

  /**
   * 分页请求参数
   */
  type RolePagingParams = {
    name?: string;
  } & Partial<Record<string, any>>;
}
