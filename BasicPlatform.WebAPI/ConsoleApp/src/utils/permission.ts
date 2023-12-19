export default {
    /** 外部页面管理 */
    externalPage: {
        /** 读取列表 */
        getPagingAsync: 'ExternalPageController_GetPagingAsync',
        /** 详情 */
        getAsync: 'externalPage:detail',
        /** 创建 */
        postAsync: 'ExternalPageController_PostAsync',
        /** 编辑 */
        putAsync: 'ExternalPageController_PutAsync',
        /** 删除 */
        deleteAsync: 'ExternalPageController_DeleteAsync',
    },
    /** 组织架构管理 */
    organization: {
        /** 读取列表 */
        getPagingAsync: 'OrganizationController_GetPagingAsync',
        /** 详情 */
        getAsync: 'org:detail',
        /** 创建 */
        postAsync: 'OrganizationController_PostAsync',
        /** 编辑 */
        putAsync: 'OrganizationController_PutAsync',
        /** 状态变更 */
        statusChangeAsync: 'OrganizationController_StatusChangeAsync',
    },
    /** 职位管理 */
    position: {
        /** 读取列表 */
        getPagingAsync: 'PositionController_GetPagingAsync',
        /** 详情 */
        getAsync: 'position:detail',
        /** 创建 */
        postAsync: 'PositionController_PostAsync',
        /** 更新 */
        putAsync: 'PositionController_PutAsync',
        /** 状态变更 */
        statusChangeAsync: 'PositionController_StatusChangeAsync',
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
        /** 分配资源 */
        assignResourcesAsync: 'RoleController_AssignResourcesAsync',
        /** 分配权限 */
        assignDataPermissionsAsync: 'RoleController_AssignDataPermissionsAsync',
        /** 分配列权限 */
        assignColumnPermissionsAsync: 'RoleController_AssignColumnPermissionsAsync',
        /** 分配用户 */
        assignUsersAsync: 'RoleController_AssignUsersAsync',
    },
    /** 服务器信息 */
    server: {
        /** 读取服务器信息 */
        get: 'ServerController_Get',
    },
    /** 租户管理 */
    tenant: {
        /** 读取列表 */
        getPagingAsync: 'TenantController_GetPagingAsync',
        /** 详情 */
        getAsync: 'tenant:detail',
        /** 创建 */
        postAsync: 'TenantController_PostAsync',
        /** 更新 */
        putAsync: 'TenantController_PutAsync',
        /** 状态变更 */
        statusChangeAsync: 'TenantController_StatusChangeAsync',
        /** 分配资源 */
        assignResourcesAsync: 'TenantController_AssignResourcesAsync',
        /** 同步数据库 */
        syncStructureAsync: 'TenantController_SyncStructureAsync',
        /** 创建超级管理员 */
        createSuperAdminAsync: 'TenantController_CreateSuperAdminAsync',
        /** 更新超级管理员 */
        updateSuperAdminAsync: 'TenantController_UpdateSuperAdminAsync',
    },
    /** 员工访问记录 */
    userAccessRecord: {
        /** 读取列表 */
        getPagingAsync: 'UserAccessRecordController_GetPagingAsync',
    },
    /** 用户管理 */
    user: {
        /** 读取列表 */
        getPagingAsync: 'UserController_GetPagingAsync',
        /** 详情 */
        getAsync: 'user:detail',
        /** 创建 */
        postAsync: 'UserController_PostAsync',
        /** 更新 */
        putAsync: 'UserController_PutAsync',
        /** 删除 */
        deleteAsync: 'UserController_DeleteAsync',
        /** 状态变更 */
        statusChangeAsync: 'UserController_StatusChangeAsync',
        /** 分配资源 */
        assignResourcesAsync: 'UserController_AssignResourcesAsync',
        /** 分配权限 */
        assignDataPermissionsAsync: 'UserController_AssignDataPermissionsAsync',
        /** 分配列权限 */
        assignColumnPermissionsAsync: 'UserController_AssignColumnPermissionsAsync',
        /** 重置密码 */
        resetPasswordAsync: 'UserController_ResetPasswordAsync',
        /** 读取资源 */
        getResourcesAsync: 'UserController_GetResourcesAsync',
        /** 读取外部页面列表 */
        getExternalPagesAsync: 'UserController_GetExternalPagesAsync',
    },
    /** 应用管理 */
    application: {
        /** 读取列表 */
        getPagingAsync: 'ApplicationController_GetPagingAsync',
        /** 读取列表 */
        getListAsync: 'ApplicationController_GetListAsync',
        /** 详情 */
        getAsync: 'application:detail',
        /** 创建 */
        postAsync: 'ApplicationController_PostAsync',
        /** 编辑 */
        putAsync: 'ApplicationController_PutAsync',
        /** 状态变更 */
        statusChangeAsync: 'ApplicationController_StatusChangeAsync',
    },
    /** 事件存储管理 */
    eventStorage: {
        /** 读取列表 */
        getPagingAsync: 'EventStorageController_GetPagingAsync',
        /** 读取内容 */
        getAsync: 'EventStorageController_GetAsync',
    },
    /** 事件追踪配置管理 */
    eventTrackingConfig: {
        /** 读取列表 */
        getPagingAsync: 'EventTrackingConfigController_GetPagingAsync',
        /** 根据ID读取 */
        getAsync: 'EventTrackingConfigController_GetAsync',
        /** 根据追踪特性列表 */
        getSelectList: 'event-tracking-select-list',
        /** 根据事件下拉列表 */
        getEventSelectList: 'event-tracking-event-select-list',
        /** 保存 */
        saveAsync: 'EventTrackingConfigController_SaveAsync',
        /** 删除 */
        deleteAsync: 'EventTrackingConfigController_DeleteAsync',
    },
    /** 事件追踪管理 */
    eventTracking: {
        /** 读取列表 */
        getPagingAsync: 'EventTrackingController_GetPagingAsync',
        /** 根据ID读取 */
        getAsync: 'EventTrackingController_GetAsync',
        /** 读取分解树图 */
        getDecompositionTreeGraphAsync: 'EventTrackingController_GetDecompositionTreeGraphAsync',
    },
    /** 日志管理 */
    log: {
        /** 读取列表 */
        getPagingAsync: 'LogController_GetPagingAsync',
        /** 读取日志详情 */
        getByIdAsync: 'LogController_GetByIdAsync',
        /** 读取日志详情 */
        getByTraceIdAsync: 'LogController_GetByTraceIdAsync',
        /** 读取调用次数 */
        getCallCountAsync: 'LogController_GetCallCountAsync',
        /** 读取服务列表 */
        getServiceSelectListAsync: 'log:service:select',
    },
    /** 资源管理 */
    resource: {
        /** 同步资源 */
        syncAsync: 'ResourceController_SyncAsync',
        /** 读取子应用资源 */
        getSubAppResourcesAsync: 'ResourceController_GetSubAppResourcesAsync',
    },
};