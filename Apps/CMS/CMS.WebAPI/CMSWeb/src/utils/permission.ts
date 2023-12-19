export default {
  /** 文章管理管理 */
  article: {
    /** 读取列表 */
    getPagingAsync: 'ArticleController_GetPagingAsync',
    /** 详情 */
    getAsync: 'article:detail',
    /** 创建 */
    postAsync: 'ArticleController_PostAsync',
    /** 编辑 */
    putAsync: 'ArticleController_PutAsync',
    /** 删除 */
    deleteAsync: 'ArticleController_DeleteAsync',
    /** 发布/取消发布 */
    publishAsync: 'ArticleController_PublishAsync',
  },
  /** 用户控制器 */
  user: {
    /** 读取资源 */
    getResourcesAsync: 'UserController_GetResourcesAsync',
    /** 读取外部页面列表 */
    getExternalPagesAsync: 'UserController_GetExternalPagesAsync',
    /** 更新表格列表信息 */
    updateUserCustomColumnsAsync: 'UserController_UpdateUserCustomColumnsAsync',
  },
};
