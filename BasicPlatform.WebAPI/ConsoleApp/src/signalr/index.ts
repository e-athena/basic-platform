import { Modal, message } from 'antd';
import rtc from './rtc-message';
import { history } from '@umijs/max';
import { removeToken } from '@/utils/token';

type NoticeMessageType = {
  type: MessageType;
  data: any;
  eventType: string;
  noticeType: string;
}

/** 状态 */
enum MessageType {
  /** 启用 */
  Notice = 0,
  /** 禁用 */
  Event = 1,
}

export default {
  // 通知消息处理
  noticeMessageHandle(obj: NoticeMessageType) {
    const { type, data, eventType, noticeType } = obj;
    // 通知
    if (type === rtc.messageType.notice) {
      const { systemUpgrade, systemUpgradeCompleted, online } = rtc.noticeType;
      switch (noticeType) {
        // 系统更新前通知
        case systemUpgrade:
          Modal.info({
            title: '系统通知',
            content: data,
          });
          break;
        // 系统更新完成通知
        case systemUpgradeCompleted:
          Modal.success({
            title: '系统通知',
            content: data,
          });
          break;
        // 用户上线通知
        case online:
          message.info(data);
          // notification.info({
          //   message: data,
          //   placement: 'bottom'
          //   // description: data,
          // });
          break;
        default:
          // notification.info(data);
          // sendNotify(data.message, {
          //   body: data.description,
          //   tag: data.sender,
          //   data,
          // });
          break;
      }
    }
    // 事件
    if (type === rtc.messageType.event) {
      const { userDisabled, userRemoved } = rtc.eventType;
      switch (eventType) {
        // 用户禁用，用户删除
        case userDisabled:
        case userRemoved:
          // 退出登录
          removeToken();
          Modal.warning({
            title: '系统通知',
            content: data,
            onOk: () => {
              // 退出登录
              history.push('/user/login');
            },
          });
          break;
        default:
          break;
      }
    }
  },
};
