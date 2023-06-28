import { ProCard } from '@ant-design/pro-components';
import React, { useEffect, useState } from 'react';
import { useModel } from '@umijs/max';
import DataPermission from '../DataPermission';

type ApplicationDataPermissionProps = {
  onChange: (data: API.UserDataPermission[]) => void;
  selectedData?: API.UserDataPermission[];
};

const ApplicationDataPermission: React.FC<ApplicationDataPermissionProps> = (props) => {
  const [currentTab, setCurrentTab] = useState<string>('system');
  const { initialState } = useModel('@@initialState');
  const [dataSource, setDataSource] = useState<API.ApplicationDataPermissionResourceInfo[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  useEffect(() => {
    const fetch = async () => {
      setLoading(false);
      const res = await initialState?.fetchApplicationDataPermissionResources?.();
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
        loading={loading}
        tabs={{
          tabPosition: 'top',
          activeKey: currentTab,
          items: dataSource.map((p) => {
            if (props.selectedData || [].length > 0) {
              // 合并数据，以data为准
              for (let i = 0; i < p.dataPermissionGroups.length; i++) {
                const group = p.dataPermissionGroups[i];
                for (let j = 0; j < group.items.length; j++) {
                  const item = group.items[j];
                  const dataItem = props.selectedData?.find(
                    (d) =>
                      d.resourceKey === item.resourceKey && d.applicationId === p.applicationId,
                  );
                  if (dataItem) {
                    // 替换数据
                    item.dataScope = dataItem.dataScope;
                    item.enabled = dataItem.enabled;
                    item.disableChecked = dataItem.disableChecked || false;
                    item.dataScopeCustom = dataItem.dataScopeCustom;
                    item.dataScopeCustoms = dataItem.dataScopeCustoms;
                    item.queryFilterGroups = dataItem.queryFilterGroups || [];
                  }
                }
              }
            }
            return {
              label: p.applicationName,
              key: p.applicationId,
              children: (
                <DataPermission
                  data={p.dataPermissionGroups || []}
                  extraSelectList={p.extraSelectList}
                  onChange={(values) => {
                    let changeValues: API.UserDataPermission[] = [];
                    for (let i = 0; i < values.length; i++) {
                      const group = values[i];
                      for (let j = 0; j < group.items.length; j++) {
                        const item = group.items[j];
                        changeValues.push({
                          applicationId: item.appId,
                          resourceKey: item.resourceKey,
                          dataScope: item.dataScope,
                          enabled: item.enabled,
                          disableChecked: item.disableChecked,
                          dataScopeCustom: item.dataScopeCustom,
                          dataScopeCustoms: item.dataScopeCustoms,
                          queryFilterGroups: item.queryFilterGroups,
                          policyResourceKey: item.policyResourceKey,
                        });
                      }
                    }
                    props.onChange(changeValues);
                  }}
                />
              ),
            };
          }),
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

export default ApplicationDataPermission;
