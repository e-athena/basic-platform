export default {
  // 组织架构管理
  organization: {
    // 读取分页列表
    getPagingAsync: 'OrganizationController_GetPagingAsync',
    // 详情
    getAsync: 'OrganizationController_GetAsync',
    // 创建
    postAsync: 'OrganizationController_PostAsync',
    // 编辑
    putAsync: 'OrganizationController_PutAsync',
    // 切换状态
    statusChangeAsync: 'OrganizationController_StatusChangeAsync',
    // 读取树形列表
    getTreeDataAsync: 'OrganizationController_GetTreeDataAsync',
  },
  // 职位管理
  position: {
    // 读取列表
    getPagingAsync: 'PositionController_GetPagingAsync',
    // 根据ID读取
    getAsync: 'PositionController_GetAsync',
    // 创建
    postAsync: 'PositionController_PostAsync',
    // 更新
    putAsync: 'PositionController_PutAsync',
    // 状态变更
    statusChangeAsync: 'PositionController_StatusChangeAsync',
    // 读取树形数据列表
    getTreeDataAsync: 'PositionController_GetTreeDataAsync',
    // 读取树形下拉数据列表
    getTreeSelectDataAsync: 'PositionController_GetTreeSelectDataAsync',
    // 读取树形选择框数据列表
    getTreeSelectDataForSelfAsync: 'PositionController_GetTreeSelectDataForSelfAsync',
    // 根据角色Id读取职位Id列表
    getIdsByRoleIdAsync: 'PositionController_GetIdsByRoleIdAsync',
    // 为职位分配角色
    assignRolesAsync: 'PositionController_AssignRolesAsync',
  },
  // 资源管理
  resource: {
    // 同步资源
    syncAsync: 'ResourceController_SyncAsync',
    // 重置资源
    reinitializeAsync: 'ResourceController_ReinitializeAsync',
  },
  // 角色管理
  role: {
    // 读取列表
    getPagingAsync: 'RoleController_GetPagingAsync',
    // 根据ID读取
    getAsync: 'RoleController_GetAsync',
    // 创建
    postAsync: 'RoleController_PostAsync',
    // 更新
    putAsync: 'RoleController_PutAsync',
    // 状态变更
    statusChangeAsync: 'RoleController_StatusChangeAsync',
  },
  // 用户管理
  user: {
    // 新增
    postAsync: 'UserController_PostAsync',
    // 读取详情
    getAsync: 'UserController_GetAsync',
  },
};