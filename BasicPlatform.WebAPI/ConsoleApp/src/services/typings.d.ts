// @ts-ignore
/* eslint-disable */

declare namespace API {
  /** 创建相关属性 */
  type CreatedItem = {
    createdOn?: string;
    createdUserId?: string;
    createdUserName?: string;
  };
  /** 更新相关属性 */
  type UpdatedItem = {
    updatedOn?: string;
    updatedUserId?: string;
    updatedUserName?: string;
  };

  /** 状态 */
  enum Status {
    /** 启用 */
    Enabled = 1,
    /** 禁用 */
    Disabled = 2,
  }
}
