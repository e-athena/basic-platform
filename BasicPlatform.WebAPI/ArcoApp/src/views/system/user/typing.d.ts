/** 创建 */
export type CreateUserRequest = {
  userName: string;
  password: string;
  avatar?: string;
  realName: string;
  phoneNumber?: string;
  email?: string;
  organizationId: string;
  roleIds: string[];
  resources: ResourceModel[];
};
/** 更新 */
export type UpdateUserRequest = {
  id: string;
} & Partial<CreateUserRequest>;
/**
 * 列表
 */
export type UserListItem = {
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
export type UserDetailInfo = {
  resources: ResourceModel[];
  organizationId: string;
  organizationPath?: string;
  roleIds: string[];
} & Partial<UserListItem>;

/** 资源代码 */
export type UserResourceCodeInfo = {
  roleResources: ResourceModel[];
  userResources: ResourceModel[];
};
/** 分配资源 */
export type AssignUserResourcesRequest = {
  id: string;
  resources: ResourceModel[];
  expireAt?: string;
};
/**
 * 分页请求参数
 */
export type UserPagingParams = {
  name?: string;
  organizationId?: string | null;
  roleId?: string;
  status?: Status[];
} & Partial<Record<string, any>>;

/** 分配数据权限 */
export type AssignUserDataPermissionsRequest = {
  id: string;
  permissions: DataPermissionItem[];
  expireAt?: string;
};
