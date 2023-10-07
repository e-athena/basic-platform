import { ProCard } from '@ant-design/pro-components';
import React, { useEffect, useState } from 'react';
import { useModel } from '@umijs/max';
import ColumnPermission from '../ColumnPermission';
import { ColumnProperty } from '../ColumnPermission/components/ColumnConfig';

export type UserColumnProperty = {
  appId: string;
} & Partial<ColumnProperty>;

type ApplicationColumnPermissionProps = {
  onChange: (data: UserColumnProperty[]) => void;
  selectedData: UserColumnProperty[];
  title: string;
};

const ApplicationColumnPermission: React.FC<ApplicationColumnPermissionProps> = (props) => {
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
          items: dataSource.map((d) => {
            return {
              label: d.applicationName,
              key: d.applicationId,
              children: (
                <ColumnPermission
                  title={props.title}
                  data={(d.dataPermissionGroups || []).map(p => {
                    return {
                      displayName: p.displayName!,
                      items: p.items
                        .map(item => {
                          return {
                            displayName: item.displayName!,
                            groupKey: item.policyResourceKey!,
                            items: item.properties.filter(p => !p.key.includes("Id")).map((p) => {
                              const child = props.selectedData?.find(c =>
                                c.appId === d.applicationId &&
                                c.columnKey === p.key &&
                                c.columnType === item.policyResourceKey
                              );
                              return {
                                enabled: child === undefined ? true : child.enabled!,
                                columnName: p.label,
                                columnType: item.policyResourceKey,
                                columnKey: p.key,
                                isEnableDataMask: child === undefined ? false : child.isEnableDataMask!,
                                dataMaskType: child === undefined ? 99 : child.dataMaskType!,
                                maskLength: child === undefined ? 4 : child.maskLength!,
                                maskPosition: child === undefined ? 2 : child.maskPosition!,
                                maskChar: child === undefined ? '*' : child.maskChar!,
                                propertyType: p.propertyType,
                              };
                            }),
                          };
                        }),
                    };
                  })}
                  onChange={(values) => {
                    let changeValues: UserColumnProperty[] = [...props.selectedData];
                    for (let i = 0; i < values.length; i++) {
                      const group = values[i];
                      for (let j = 0; j < group.items.length; j++) {
                        const item = group.items[j];
                        // // 跳过未配置的
                        // if (
                        //   item.items.length === item.items.filter(p => p.enabled).length &&
                        //   item.items.filter(p => p.isEnableDataMask).length === 0
                        // ) {
                        //   continue;
                        // }
                        for (let k = 0; k < item.items.length; k++) {
                          const kItem = item.items[k];
                          const index = changeValues.findIndex(c =>
                            c.appId === d.applicationId &&
                            c.columnKey === kItem.columnKey &&
                            c.columnType === kItem.columnType
                          );
                          if (index === -1) {
                            changeValues.push({
                              appId: d.applicationId,
                              ...kItem,
                            });
                          } else {
                            changeValues[index] = {
                              appId: d.applicationId,
                              ...kItem,
                            };
                          }
                        }
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

export default ApplicationColumnPermission;
