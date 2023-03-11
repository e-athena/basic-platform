declare namespace API {
  /** 创建 */
  type CreateExternalPageRequest = {
    type: number;
    parentId?: string;
    name: string;
    path: string;
    icon: string;
    layout: string;
    isPublic: boolean;
    sort: number;
    remarks?: string;
  };
  /** 更新 */
  type UpdateExternalPageRequest = {
    id: string;
  } & Partial<CreateExternalPageRequest>;
  /**
   * 列表
   */
  type ExternalPageListItem = {
    id?: string;
    type?: number;
    parentId?: string;
    name?: string;
    path?: string;
    icon?: string;
    layout?: string;
    isPublic?: boolean;
    sort?: number;
    remarks?: string;
  } & Partial<CreatedItem> &
    Partial<UpdatedItem>;

  /** 详情 */
  type ExternalPageDetailItem = {
    roleIds?: string[];
  } & Partial<ExternalPageListItem>;

  /**
   * 分页请求参数
   */
  type ExternalPagePagingParams = {
    parentId?: string | null;
  } & Partial<Record<string, any>>
}
