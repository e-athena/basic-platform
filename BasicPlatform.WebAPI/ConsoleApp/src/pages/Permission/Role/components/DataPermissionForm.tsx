import { submitHandle } from '@/utils/utils';
import { ModalForm } from '@ant-design/pro-components';
import React, { useState, useEffect } from 'react';
import { dataPermission, assignDataPermissions } from '../service';
import DataPermission from '@/components/DataPermission';

type DataPermissionFormProps = {
  onCancel: () => void;
  onSuccess: () => void;
  open: boolean;
  roleId: string;
  title?: string;
};

const DataPermissionForm: React.FC<DataPermissionFormProps> = (props) => {
  const [dataSources, setDataSources] = useState<API.DataPermissionGroup[]>([]);
  const [data, setData] = useState<API.DataPermissionGroup[]>([]);

  useEffect(() => {
    const fetch = async () => {
      const result = await dataPermission(props.roleId);
      if (result.success) {
        const data = result.data || [];
        setData(data);
        setDataSources(data);
      }
    }
    if (props.open) {
      fetch();
    };
  }, [props.open]);
  return (
    <ModalForm
      width={960}
      title={props.title || '分配权限'}
      open={props.open}
      modalProps={{
        onCancel: () => {
          props.onCancel();
        },
        // bodyStyle: { padding: '32px 40px 48px' },
        bodyStyle: { padding: '10px 0', minHeight: 400 },
        destroyOnClose: true,
      }}
      onFinish={async () => {
        // 将DataSources展开
        let permissions: API.DataPermissionItem[] = [];
        for (let j = 0; j < dataSources.length; j++) {
          const group = dataSources[j];
          for (let i = 0; i < group.items.length; i++) {
            const item = group.items[i];
            permissions.push({
              resourceKey: item.resourceKey,
              dataScope: item.dataScope,
              enabled: item.enabled,
              dataScopeCustom: (item.dataScopeCustoms || []).join(','),
              queryFilterGroups: item.queryFilterGroups,
              policyResourceKey: item.policyResourceKey,
            });
          }
        }
        const succeed = await submitHandle(assignDataPermissions, {
          id: props.roleId,
          permissions,
        });
        if (succeed) {
          props.onSuccess();
        }
      }}
    >
      <DataPermission
        onChange={(data) => {
          setDataSources(data);
        }}
        data={data}
      />
    </ModalForm>
  );
};

export default DataPermissionForm;