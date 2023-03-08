import { PageContainer, ProDescriptions } from '@ant-design/pro-components';
import { useLocation, useModel } from '@umijs/max';
import { Button, Divider, message } from 'antd';
import { useEffect, useState } from 'react';
import { query } from './service';

const App: React.FC = () => {
  const { getResource } = useModel('resource');
  const location = useLocation();
  const resource = getResource(location.pathname);

  const [dataSource, setDataSource] = useState<API.ServerInfo>();
  const [loading, setLoading] = useState<boolean>(true);
  useEffect(() => {
    const fetch = async () => {
      setLoading(false);
      const hide = message.loading('正在加载');
      const res = await query();
      setDataSource(res.data);
      hide();
    }
    if (loading) {
      fetch();
    }
  }, [loading]);

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
        <ProDescriptions.Item span={2} valueType="text" dataIndex={'basePath'} label="基本路径" />
        <ProDescriptions.Item valueType="text" dataIndex={'runtimeFramework'} label="运行时框架" />
        <ProDescriptions.Item valueType="text" dataIndex={'frameworkDescription'} label="运行时框架说明" />
        <ProDescriptions.Item valueType="text" dataIndex={'hostName'} label="主机名" />
        <ProDescriptions.Item valueType="text" dataIndex={'ipAddress'} label="IP地址" />
        <ProDescriptions.Item valueType="text" dataIndex={'processName'} label="进程名称" />
        <ProDescriptions.Item valueType="text" dataIndex={'memoryUsage'} label="内存使用情况" />
        <ProDescriptions.Item valueType="text" dataIndex={'startTime'} label="启动时间" />
        <ProDescriptions.Item valueType="text" dataIndex={'threads'} label="线程数" />
        <ProDescriptions.Item valueType="text" dataIndex={'totalProcessorTime'} label="处理器总时间" />
        <ProDescriptions.Item valueType="text" dataIndex={'userProcessorTime'} label="用户处理器时间" />
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