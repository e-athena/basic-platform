declare namespace API {
  /**
   * 审计日志列表项
   */
  type LogListItem = {
    id: number;
    category: string;
    action: string;
    linkID: number;
    success: boolean;
    traceId: string;
    userName: string;
    remark?: string;
  } & Partial<CreateItem> &
    Partial<ExItem>;
}
