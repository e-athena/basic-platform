import { PageContainer, ProDescriptions } from '@ant-design/pro-components';
import { useLocation, useModel } from '@umijs/max';
import { Button, Divider, message } from 'antd';
import { useEffect, useState } from 'react';
import { query } from './service';

const App: React.FC = () => {
  const { getResource } = useModel('resource');
  const location = useLocation();
  const resource = getResource(location.pathname);

  const [startTime, setStartTime] = useState<string>();
  const [runTime, setRunTime] = useState<string>();

  // 时钟效果
  useEffect(() => {
    if (startTime === undefined) {
      return;
    }
    const timer = setInterval(() => {
      // 计算已经运行了多久
      const now = new Date();
      const start = new Date(startTime);
      const diff = now.getTime() - start.getTime();
      const days = Math.floor(diff / (24 * 3600 * 1000));
      const leave1 = diff % (24 * 3600 * 1000);
      const hours = Math.floor(leave1 / (3600 * 1000));
      // 分钟
      const leave2 = leave1 % (3600 * 1000);
      const minutes = Math.floor(leave2 / (60 * 1000));
      // 秒
      const leave3 = leave2 % (60 * 1000);
      const seconds = Math.round(leave3 / 1000);
      setRunTime(`${days}天${hours}小时${minutes}分钟${seconds}秒`);
    }, 1000);
    return () => {
      clearInterval(timer);
    }
  }, [startTime]);

  const [dataSource, setDataSource] = useState<API.ServerInfo>();
  const [loading, setLoading] = useState<boolean>(true);
  useEffect(() => {
    const fetch = async () => {
      setLoading(false);
      const hide = message.loading('正在加载', 0);
      const res = await query();
      setDataSource(res.data);
      setStartTime(res.data?.startTime);
      hide();
    }
    if (loading) {
      fetch();
    }
  }, [loading]);

  // 10秒刷新一次
  useEffect(() => {
    const timer = setInterval(() => {
      setLoading(true);
    }, 10000);
    return () => {
      clearInterval(timer);
    }
  }, []);

  return (
    <PageContainer header={{
      title: resource?.name,
      children: resource?.description
    }}>
      <ProDescriptions<API.ServerInfo>
        column={2}
        title={'基础信息'}
        dataSource={dataSource}
      >
        <ProDescriptions.Item valueType="option">
          <Button key="primary" type="primary" onClick={() => {
            setLoading(true);
          }}>
            刷新
          </Button>
        </ProDescriptions.Item>
        <ProDescriptions.Item valueType="text" dataIndex={'appName'} label="应用名称" />
        <ProDescriptions.Item valueType="text" dataIndex={'appVersion'} label="应用版本" />
        <ProDescriptions.Item valueType="text" dataIndex={'osArchitecture'} label="操作系统平台" />
        <ProDescriptions.Item valueType="text" dataIndex={'processArchitecture'} label="操作系统架构" />
        <ProDescriptions.Item span={2} valueType="text" dataIndex={'osDescription'} label="操作系统说明" />
        <ProDescriptions.Item valueType="text" dataIndex={'basePath'} label="根目录" />
        <ProDescriptions.Item valueType="text" dataIndex={'threads'} label="线程数" />
        <ProDescriptions.Item valueType="text" dataIndex={'runtimeFramework'} label="运行时框架" />
        <ProDescriptions.Item valueType="text" dataIndex={'frameworkDescription'} label="运行时框架说明" />
        <ProDescriptions.Item valueType="text" dataIndex={'hostName'} label="主机名" />
        <ProDescriptions.Item valueType="text" dataIndex={'ipAddress'} label="IP地址" />
        <ProDescriptions.Item valueType="text" dataIndex={'processName'} label="进程名称" />
        <ProDescriptions.Item valueType="text" dataIndex={'memoryUsage'} label="内存使用情况" />
        <ProDescriptions.Item valueType="text" dataIndex={'startTime'} label="启动时间" />
        <ProDescriptions.Item valueType="text" label="已运行时间">{runTime}</ProDescriptions.Item>

        {/* <ProDescriptions.Item valueType="text" dataIndex={'totalProcessorTime'} label="处理器总时间" />
        <ProDescriptions.Item valueType="text" dataIndex={'userProcessorTime'} label="用户处理器时间" /> */}
      </ProDescriptions>
      <Divider />
      {dataSource && <ProDescriptions column={2} title={'环境变量'}>
        {Object.keys(dataSource.environments).map((key, index) => {
          const value = dataSource.environments[key];
          if (value === null) {
            return null;
          }
          return <ProDescriptions.Item key={index} valueType="text" label={key}>{value}</ProDescriptions.Item>;
        })}
      </ProDescriptions>}
    </PageContainer>
  );
};

export default App;