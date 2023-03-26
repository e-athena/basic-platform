declare namespace API {
  /** 创建 */
  type CreatePositionItem = {
    organizationId: string | string[];
    name: string;
    remarks?: string;
    status: Status;
  };
  /** 更新 */
  type UpdatePositionItem = {
    id: string;
  } & Partial<CreatePositionItem>;
  /**
   * 列表
   */
  type PositionListItem = {
    id: string;
    organizationId: string;
    name: string;
    remarks?: string;
    status: Status;
  } & Partial<CreatedItem> &
    Partial<UpdatedItem>;

  /** 详情 */
  type PositionDetailItem = {
    organizationPath?: string;
  } & Partial<PositionListItem>;

  /**
   * 分页请求参数
   */
  type PositionPagingParams = {
    name?: string;
  } & Partial<Record<string, any>>;
}
