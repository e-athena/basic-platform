declare namespace API {
  /** 保存 */
  type SaveEventTrackingConfigItem = {
    configs: EventTrackingConfigItem[];
  };
  /**  */
  type EventTrackingConfigItem = {
    id: string;
    createdOn?: string;
    configId: string;
    parentId: string;
    eventType: number;
    eventTypeTitle: string;
    eventName: string;
    eventTypeName: string;
    eventTypeFullName: string;
    processorName: string;
    processorFullName: string;
  }
  /**
   * 列表
   */
  type EventTrackingConfigListItem = {
    id?: string;
    createdOn?: string;
    configId?: string;
    parentId?: string;
    eventType: number;
    eventName: string;
    eventTypeName: string;
    eventTypeFullName: string;
    processorName: string;
    processorFullName: string;
  };

  /** 详情 */
  type EventTrackingConfigDetailItem = {
    children?: EventTrackingConfigDetailItem[];
  } & Partial<SaveEventTrackingConfigItem>;

  /**
   * 分页请求参数
   */
  type EventTrackingConfigPagingParams = {
    name?: string;
  } & Partial<Record<string, any>>;
}
