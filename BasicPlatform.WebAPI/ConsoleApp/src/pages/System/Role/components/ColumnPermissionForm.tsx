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
      title={props.title || '分配权限'}
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

        // 将DataSources展开
        let permissions: API.ColumnPermissionItem[] = [];
        for (let i = 0; i < (selectedData || []).length; i++) {
          const item = selectedData![i];
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
        const succeed = await submitHandle(assignColumnPermissions, {
          id: props.roleId,
          permissions,
        });
        if (succeed) {
          props.onSuccess();
        }
        // console.log(props.roleId, permissions);
        // console.log(selectedData);
      }}
      loading={loading}
    >
      {!loading && selectedData &&
        <ApplicationColumnPermission
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
