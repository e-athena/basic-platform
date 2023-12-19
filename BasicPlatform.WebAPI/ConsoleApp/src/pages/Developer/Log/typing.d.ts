declare namespace API {
  /**
   * 列表
   */
  type LogListItem = {
    id: number;
    serviceName: string;
    aliasName: string;
    traceId: string;
    ipAddress: any;
    userAgent: string;
    logLevel: number;
    route: string;
    httpMethod: string;
    statusCode: number;
    userId: string;
    startTime: string;
    endTime: string;
    elapsedMilliseconds: number;
    createdOn: string;
    browser: string;
    os: string;
    device: string;
  };

  /**
   * 详情
   */
  type LogDetail = {
    id: number;
    serviceName: string;
    aliasName: string;
    traceId: string;
    ipAddress: string;
    userAgent: string;
    logLevel: number;
    route: string;
    httpMethod: string;
    requestBody: string;
    responseBody: string;
    rawData: string;
    statusCode: number;
    userId: string;
    startTime: string;
    endTime: string;
    elapsedMilliseconds: number;
    errorMessage: string;
    createdOn: string;
    browser: string;
    os: string;
    device: string;
  }

  /**
   * 分页请求参数
   */
  type LogPagingParams = {
    serviceName?: string;
    dateRange: dayjs.Dayjs[];
    userId?: string;
    realName?: string;
    traceId?: string;
    logLevel?: number;
  } & Partial<Record<string, any>>;
}
