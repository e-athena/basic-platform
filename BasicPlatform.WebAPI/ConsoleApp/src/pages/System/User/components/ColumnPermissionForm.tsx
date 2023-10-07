import { submitHandle } from '@/utils/utils';
import { ModalForm, ProFormDateTimePicker } from '@ant-design/pro-components';
import React, { useState, useEffect } from 'react';
import { columnPermission, assignColumnPermissions } from '../service';
import ApplicationColumnPermission, { UserColumnProperty } from '@/components/ApplicationColumnPermission';
import { Empty, Spin } from 'antd';

type ColumnPermissionFormProps = {
  onCancel: () => void;
  onSuccess: () => void;
  open: boolean;
  userId: string;
  title?: string;
};

const ColumnPermissionForm: React.FC<ColumnPermissionFormProps> = (props) => {
  const [selectedData, setSelectedData] = useState<UserColumnProperty[]>();
  const [loading, setLoading] = useState<boolean>(true);
  useEffect(() => {
    const fetch = async () => {
      const result = await columnPermission(props.userId);
      setLoading(false);
      if (result.success) {
        const data = result.data || [];
        setSelectedData(data);
      }
    };
    if (props.open) {
      fetch();
    }
  }, [props.open]);
  return (
    <ModalForm
      // width={960}
      title={props.title || '分配列权限'}
      open={props.open}
      modalProps={{
        onCancel: () => {
          props.onCancel();
        },
        // bodyStyle: { padding: '32px 40px 48px' },
        bodyStyle: {
          // padding: '10px 0',
          minHeight: 400,
        },
        destroyOnClose: true,
      }}
      onFinish={async (values) => {
        let permissions: API.ColumnPermissionItem[] = [];
        // 根据columnType分组，然后将每组的数据转换成API.ColumnPermissionItem
        const groupByColumnType = (selectedData || []).reduce((prev, current) => {
          const key = current.columnType!;
          (prev[key] = prev[key] || []).push(current);
          return prev;
        }, {} as { [key: string]: UserColumnProperty[] });
        // 读取每组数据，如果enabled为false，或者isEnableDataMask为true，则添加到permissions中
        const data = Object.keys(groupByColumnType).map((key) => {
          return !(groupByColumnType[key].filter(p => p.enabled).length === groupByColumnType[key].length &&
            groupByColumnType[key].filter(p => p.isEnableDataMask).length === 0) ? groupByColumnType[key] : [];
        }).filter(p => p.length > 0);
        for (let i = 0; i < data.length; i++) {
          const items = data[i];
          for (let j = 0; j < items.length; j++) {
            const item = items[j];
            permissions.push({
              appId: item.appId,
              columnType: item.columnType!,
              columnKey: item.columnKey!,
              enabled: item.enabled!,
              isEnableDataMask: item.isEnableDataMask!,
              dataMaskType: item.dataMaskType!,
              maskLength: item.maskLength!,
              maskPosition: item.maskPosition!,
              maskChar: item.maskChar!,
            });
          }
        }
        const succeed = await submitHandle(assignColumnPermissions, {
          ...values,
          id: props.userId,
          permissions,
        });
        if (succeed) {
          props.onSuccess();
        }
      }}
      loading={loading}
    >
      <ProFormDateTimePicker
        name="expireAt"
        label="有效期至"
        tooltip="权限授权的有效期，超过有效期将自动失效。"
        placeholder={'请选择'}
        formItemProps={{
          style: {
            marginBottom: 0,
          },
        }}
      />
      {!loading && selectedData ?
        <ApplicationColumnPermission
          title={props.title || '分配列权限'}
          onChange={(value) => {
            setSelectedData(value);
          }}
          selectedData={selectedData}
        /> : <Spin>
          <Empty description={'加载中'} />
        </Spin>
      }
    </ModalForm>
  );
};

export default ColumnPermissionForm;
