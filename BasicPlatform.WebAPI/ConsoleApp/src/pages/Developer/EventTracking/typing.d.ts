declare namespace API {
  /**
   * 列表
   */
  type EventTrackingListItem = {
    id: string;
    createdOn: string;
    parentId: any;
    traceId: string;
    eventType: number;
    eventName: string;
    eventTypeFullName: string;
    trackStatus: number;
    beginExecuteTime: any;
    endExecuteTime: any;
    payload: any;
    processorFullName: any;
    exceptionMessage: any;
    exceptionInnerMessage: any;
    exceptionInnerType: any;
    exceptionStackTrace: any;
    hasError: boolean;
  };
  
  /**
   * 分页请求参数
   */
  type EventTrackingPagingParams = {
    name?: string;
  } & Partial<Record<string, any>>;
}
