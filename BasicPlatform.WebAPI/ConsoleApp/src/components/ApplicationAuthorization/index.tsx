import { ProCard } from '@ant-design/pro-components';
import React, { useEffect, useState } from 'react';
import { useModel } from '@umijs/max';
import Authorization from '../Authorization';

type ApplicationAuthorizationProps = {
  onChange?: (codes: ResourceModel[], currentCodes?: ResourceModel[]) => void;
  resources?: ResourceModel[];
  // 禁用的资源代码
  disabledResourceKeys?: string[];
  height?: number;
};

const ApplicationAuthorization: React.FC<ApplicationAuthorizationProps> = (props) => {
  const [currentTab, setCurrentTab] = useState<string>('system');
  const { initialState } = useModel('@@initialState');
  const [dataSource, setDataSource] = useState<API.ApplicationMenuResourceInfo[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  useEffect(() => {
    const fetch = async () => {
      setLoading(false);
      const res = await initialState?.fetchApplicationResources?.();
      setDataSource(res || []);
    };
    if (loading) {
      fetch();
    }
  }, [loading]);
  return (
    <>
      <ProCard
        headerBordered
        tabs={{
          tabPosition: 'top',
          activeKey: currentTab,
          items: (dataSource || []).map((p) => ({
            label: p.applicationName,
            key: p.applicationId,
            children: <Authorization {...props} dataSource={p.resources || []} />,
          })),
          cardProps: {
            bodyStyle: {
              paddingInlineStart: 0,
              paddingInlineEnd: 0,
            },
          },
          onChange: (key) => {
            setCurrentTab(key);
          },
        }}
      />
    </>
  );
};

export default ApplicationAuthorization;
