export default {
  methodName: {
    noticeMessage: 'NoticeMessage',
    systemMessage: 'SystemMessage',
    controlReply: 'controlReply',
  },
  messageType: {
    notice: 0,
    event: 1,
  },
  // 通知类型
  noticeType: {
    // 上线通知
    online: 'OnlineNotice',
    // 系统升级
    systemUpgrade: 'SystemUpgradeNotice',
    // 系统升级完成
    systemUpgradeCompleted: 'SystemUpgradeCompletedNotice',
  },
  // 事件类型
  eventType: {
    userDisabled: 'UserDisabledEvent',
    userRemoved: 'UserRemovedEvent',
  },
};
