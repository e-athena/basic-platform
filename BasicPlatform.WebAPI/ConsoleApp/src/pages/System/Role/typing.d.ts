declare namespace API {
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

  /**
   * 分页请求参数
   */
  type RolePagingParams = {
    name?: string;
  }
}
