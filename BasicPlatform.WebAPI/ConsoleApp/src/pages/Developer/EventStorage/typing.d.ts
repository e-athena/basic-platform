declare namespace API {
  /**
   * 列表
   */
  type EventStorageListItem = {
    sequence: number;
    aggregateRootTypeName: string;
    aggregateRootId: string;
    version: number;
    eventId: string;
    eventName: string;
    createdOn: string;
    events?: string;
  };
  
  /**
   * 分页请求参数
   */
  type EventStoragePagingParams = {
    id: string;
  } & Partial<Record<string, any>>;
}
