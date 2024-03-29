import { submitHandle } from '@/utils/utils';
import { ModalForm, ProFormDateTimePicker } from '@ant-design/pro-components';
import React, { useEffect, useState } from 'react';
import { assignDataPermissions, dataPermission } from '../service';
import DataPermission from '@/components/ApplicationDataPermission';
import { Alert } from 'antd';

type DataPermissionFormProps = {
  onCancel: () => void;
  onSuccess: () => void;
  open: boolean;
  userId: string;
  title?: string;
};

const DataPermissionForm: React.FC<DataPermissionFormProps> = (props) => {
  const [selectedData, setSelectedData] = useState<API.UserDataPermission[]>([]);

  useEffect(() => {
    const fetch = async () => {
      const result = await dataPermission(props.userId);
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
        bodyStyle: {
          // padding: '10px 0',
          minHeight: 400,
        },
        destroyOnClose: true,
      }}
      onFinish={async (values) => {
        // 将DataSources展开
        let permissions: API.DataPermissionItem[] = [];
        for (let i = 0; i < selectedData.length; i++) {
          const item = selectedData[i];
          permissions.push({
            appId: item.appId,
            resourceKey: item.resourceKey,
            dataScope: item.dataScope,
            enabled: item.enabled,
            dataScopeCustom: (item.dataScopeCustoms || []).join(','),
            queryFilterGroups: item.queryFilterGroups,
            policyResourceKey: item.policyResourceKey,
          });
        }
        const params = {
          ...values,
          id: props.userId,
          permissions,
        };
        const succeed = await submitHandle(assignDataPermissions, params);
        if (succeed) {
          props.onSuccess();
        }
      }}
    >
      <Alert
        style={{ marginBottom: 24 }}
        message="禁用的权限是角色自带的，用户已经拥有，其他可选的是用户可分配的额外权限。角色自带的也可自定义配置数据范围。"
        type="warning"
      />
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
