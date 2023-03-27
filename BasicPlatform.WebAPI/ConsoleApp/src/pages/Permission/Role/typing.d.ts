declare namespace API {
  /** 创建角色 */
  type CreateRoleItem = {
    name: string;
    remarks?: string;
    resources?: ResourceModel[];
  };
  /** 更新角色 */
  type UpdateRoleItem = {
    id: string;
    name: string;
    remarks?: string;
    resources?: ResourceModel[];
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
    users?: API.SelectInfo[];
    dataScopeCustomList: string[];
    dataScopeCustomSelectList: SelectInfo[];
  } & Partial<RoleListItem>;

  /**
   * 分页请求参数
   */
  type RolePagingParams = {
    name?: string;
  } & Partial<Record<string, any>>;

  /** 分配资源 */
  type AssignRoleResourcesRequest = {
    id: string;
    resources: ResourceModel[];
  };

  /** 分配用户 */
  type AssignRolUsersRequest = {
    id: string;
    userIds: string[];
  };

  /** 数据权限组 */
  type RoleDataPermissionGroup = {
    displayName: string;
    items: RoleDataPermission[];
  };

  /** 数据权限 */
  type RoleDataPermission = {
    displayName?: string;
    resourceKey: string;
    dataScope: number;
    enabled: boolean;
    dataScopeCustom?: string;
    dataScopeCustoms: string[];
  };

  /** 分配数据权限 */
  type AssignRoleDataPermissionsRequest = {
    id: string;
    permissions: RoleDataPermissionItem[];
  };
  /** 分配数据权限 */
  type RoleDataPermissionItem = {
    resourceKey: string;
    dataScope: number;
    enabled: boolean;
    dataScopeCustom?: string;
  };
}
