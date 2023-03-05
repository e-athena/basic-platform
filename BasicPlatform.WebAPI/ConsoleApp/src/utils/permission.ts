export default {
  /** 组织架构管理 */
  organization: {
    /** 读取分页列表 */
    getPagingAsync: 'OrganizationController_GetPagingAsync',
    /** 详情 */
    getAsync: 'OrganizationController_GetAsync',
    /** 创建 */
    postAsync: 'OrganizationController_PostAsync',
    /** 编辑 */
    putAsync: 'OrganizationController_PutAsync',
    /** 切换状态 */
    statusChangeAsync: 'OrganizationController_StatusChangeAsync',
    /** 读取树形列表 */
    getTreeDataAsync: 'OrganizationController_GetTreeDataAsync',
  },
  /** 资源管理 */
  resource: {
    /** 同步资源 */
    syncAsync: 'ResourceController_SyncAsync',
    /** 重置资源 */
    reinitializeAsync: 'ResourceController_ReinitializeAsync',
  },
  /** 角色管理 */
  role: {
    /** 读取列表 */
    getPagingAsync: 'RoleController_GetPagingAsync',
    /** 详情 */
    getAsync: 'role:detail',
    /** 创建 */
    postAsync: 'RoleController_PostAsync',
    /** 更新 */
    putAsync: 'RoleController_PutAsync',
    /** 状态变更 */
    statusChangeAsync: 'RoleController_StatusChangeAsync',
  },
  /** 用户管理 */
  user: {
    /** 读取列表 */
    getPagingAsync: 'UserController_GetPagingAsync',
    /** 根据ID读取 */
    getAsync: 'UserController_GetAsync',
    /** 创建 */
    postAsync: 'UserController_PostAsync',
    /** 更新 */
    putAsync: 'UserController_PutAsync',
    /** 状态变更 */
    statusChangeAsync: 'UserController_StatusChangeAsync',
    /**  */
    getUserAsync: 'UserController_GetUserAsync',
    /** 读取用户 */
    getSelectDataAsync: 'UserController_GetSelectDataAsync',
    /** 读取组织架构用户树形列表 */
    getOrganizationUserTreeSelectListAsync: 'UserController_GetOrganizationUserTreeSelectListAsync',
    /** 读取组织架构和用户树形列表 */
    getOrganizationAndUserTreeSelectListAsync: 'UserController_GetOrganizationAndUserTreeSelectListAsync',
    /** 读取资源 */
    getResourcesAsync: 'UserController_GetResourcesAsync',
  },
};