declare namespace API {
  /** 创建 */
  type CreateUserRequest = {
    userName: string;
    password: string;
    realName: string;
    phoneNumber?: string;
    email?: string;
    organizationIds: string[];
    roleIds: string[];
    resourceCodes: string[];
  };
  /** 更新 */
  type UpdateUserRequest = {
    id: string;
  } & Partial<CreateUserRequest>;
  /**
   * 列表
   */
  type UserListItem = {
    id: string;
    userName: string;
    password: string;
    realName: string;
    phoneNumber?: string;
    email?: string;
    isEnabled: boolean;
    status: Status;
  } & Partial<CreatedItem> &
    Partial<UpdatedItem>;

  /** 详情 */
  type UserDetailInfo = {
    resourceCodes: string[];
    organizationIds: string[];
    roleIds: string[];
  } & Partial<UserListItem>;

  /** 资源代码 */
  type UserResourceCodeInfo = {
    roleResourceCodes: string[];
    userResourceCodes: string[];
  };
  /** 分配资源 */
  type AssignUserResourcesRequest = {
    id: string;
    resourceCodes: string[];
  }
  /**
   * 分页请求参数
   */
  type UserPagingParams = {
    name?: string;
    organizationId?: string;
    roleId?: string;
    status?: Status[];
  } & Partial<Record<string, any>>
}
