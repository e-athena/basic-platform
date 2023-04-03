declare namespace API {
  /**
   * 列表
   */
  type UserAccessRecordListItem = {
    id: string;
    userRealName: string;
    accessUrl: string;
    accessTime: Date;
    accessIp: string;
    accessPhysicalAddress?: string;
    browserName: string;
    browserVersion: string;
    osName: string;
    osVersion: string;
  };
}
