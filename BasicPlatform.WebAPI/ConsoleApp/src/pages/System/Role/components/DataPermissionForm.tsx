import { submitHandle } from '@/utils/utils';
import { ModalForm } from '@ant-design/pro-components';
import React, { useState, useEffect } from 'react';
import { dataPermission, assignDataPermissions } from '../service';
import DataPermission from '@/components/ApplicationDataPermission';

type DataPermissionFormProps = {
  onCancel: () => void;
  onSuccess: () => void;
  open: boolean;
  roleId: string;
  title?: string;
};

const DataPermissionForm: React.FC<DataPermissionFormProps> = (props) => {
  const [selectedData, setSelectedData] = useState<API.UserDataPermission[]>([]);

  useEffect(() => {
    const fetch = async () => {
      const result = await dataPermission(props.roleId);
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
      width={960}
      title={props.title || '分配权限'}
      open={props.open}
      modalProps={{
        onCancel: () => {
          props.onCancel();
        },
        // bodyStyle: { padding: '32px 40px 48px' },
        bodyStyle: {
          // padding: '10px 0',
          minHeight: 400
        },
        destroyOnClose: true,
      }}
      onFinish={async () => {
        // 将DataSources展开
        let permissions: API.DataPermissionItem[] = [];
        for (let i = 0; i < selectedData.length; i++) {
          const item = selectedData[i];
          permissions.push({
            applicationId: item.applicationId,
            resourceKey: item.resourceKey,
            dataScope: item.dataScope,
            enabled: item.enabled,
            dataScopeCustom: (item.dataScopeCustoms || []).join(','),
            queryFilterGroups: item.queryFilterGroups,
            policyResourceKey: item.policyResourceKey,
          });
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
          setSelectedData(data);
        }}
        selectedData={selectedData}
      />
    </ModalForm>
  );
};

export default DataPermissionForm;
