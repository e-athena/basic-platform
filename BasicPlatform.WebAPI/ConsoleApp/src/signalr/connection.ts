import { HttpTransportType, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import rtc from './rtc-message';
import { notification } from "antd";
import notice from './index';
import { getToken } from '@/utils/token';

/**
 * SignalR Notice
 * @returns connection
 */
export const fetchSignalRConnectionNotice = async () => {
  const token = getToken();
  const tenantId = false;
  let url = `${RTC_URL}/hubs/notice`;
  if (tenantId) {
    url += `?tenant_id=${tenantId}`;
  }
  const connection = new HubConnectionBuilder()
    .withUrl(url, {
      transport: HttpTransportType.WebSockets,
      skipNegotiation: true,
      accessTokenFactory: () => token.split(' ')[1],
    })
    .configureLogging(LogLevel.Error)
    .withAutomaticReconnect()
    .build();
  // start
  async function start() {
    try {
      await connection.start();
    } catch (err) {
      setTimeout(start, 5000);
    }
  }
  connection.off(rtc.methodName.systemMessage);
  connection.on(rtc.methodName.systemMessage, (msg) => {
    notification.info({
      message: '系统消息',
      description: msg,
    });
  });
  connection.off(rtc.methodName.noticeMessage);
  connection.on(rtc.methodName.noticeMessage, (message) => {
    console.log(message);
    // 通知消息处理
    notice.noticeMessageHandle(message);
  });
  // 启动
  await start();
  return connection;
};


/**
 * SignalR Event
 * @returns 
 */
export const fetchSignalRConnectionEvent = async () => {
  const token = getToken();
  const tenantId = false;
  let url = `${RTC_URL}/hubs/event`;
  if (tenantId) {
    url += `?tenant_id=${tenantId}`;
  }
  const connection = new HubConnectionBuilder()
    .withUrl(url, {
      transport: HttpTransportType.WebSockets,
      skipNegotiation: true,
      accessTokenFactory: () => token.split(' ')[1],
    })
    .configureLogging(LogLevel.Error)
    .withAutomaticReconnect()
    .build();
  // start
  async function start() {
    try {
      await connection.start();
    } catch (err) {
      setTimeout(start, 5000);
    }
  }
  connection.off(rtc.methodName.controlReply);
  connection.on(rtc.methodName.controlReply, (obj) => {
    console.log('controlReply', obj);
  });
  // 启动
  await start();
  return connection;
};