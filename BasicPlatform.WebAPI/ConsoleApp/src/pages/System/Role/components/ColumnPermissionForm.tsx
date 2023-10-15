import { submitHandle } from '@/utils/utils';
import { ModalForm } from '@ant-design/pro-components';
import React, { useState, useEffect } from 'react';
import { columnPermission, assignColumnPermissions } from '../service';
import ApplicationColumnPermission, { UserColumnProperty } from '@/components/ApplicationColumnPermission';

type ColumnPermissionFormProps = {
  onCancel: () => void;
  onSuccess: () => void;
  open: boolean;
  roleId: string;
  title?: string;
};

const ColumnPermissionForm: React.FC<ColumnPermissionFormProps> = (props) => {
  const [selectedData, setSelectedData] = useState<UserColumnProperty[]>();
  const [loading, setLoading] = useState<boolean>(true);
  useEffect(() => {
    const fetch = async () => {
      const result = await columnPermission(props.roleId);
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
      onFinish={async () => {
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
          id: props.roleId,
          permissions,
        });
        if (succeed) {
          props.onSuccess();
        }
      }}
      loading={loading}
    >
      {!loading && selectedData &&
        <ApplicationColumnPermission
          title={props.title || '分配列权限'}
          onChange={(value) => {
            setSelectedData(value);
          }}
          selectedData={selectedData}
        />
      }
    </ModalForm>
  );
};

export default ColumnPermissionForm;
