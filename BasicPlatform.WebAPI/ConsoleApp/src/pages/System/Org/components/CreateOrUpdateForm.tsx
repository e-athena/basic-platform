import { submitHandle } from '@/utils/utils';
import {
  ProFormText,
  ProFormTextArea,
  ModalForm,
  ProFormTreeSelect,
  ProFormSelect,
  ProFormSwitch
} from '@ant-design/pro-components';
import React from 'react';
import { update, create } from '../service';
import { roleList } from '@/services/ant-design-pro/system/role'
import { orgTreeSelectForSelf } from '@/services/ant-design-pro/system/org'

type CreateOrUpdateFormProps = {
  onCancel: () => void;
  onSuccess: () => void;
  open: boolean;
  values?: API.OrgDetailItem;
};

const CreateOrUpdateForm: React.FC<CreateOrUpdateFormProps> = (props) => {
  return (
    <ModalForm
      width={600}
      title={props.values?.id === undefined ? '创建组织/部门' : '更新组织/部门'}
      open={props.open}
      modalProps={{
        onCancel: () => {
          props.onCancel();
        },
        bodyStyle: { padding: '32px 40px 48px' },
        destroyOnClose: true,
      }}
      onFinish={async (values: API.UpdateOrgRequest) => {
        const isUpdate = props.values?.id !== undefined;
        let succeed;
        if (isUpdate) {
          values.id = props.values!.id!;
          succeed = await submitHandle(update, values);
        } else {
          values.status = values.status ? 1 : 2;
          succeed = await submitHandle(create, (values as API.CreateOrgRequest));
        }
        if (succeed) {
          props.onSuccess();
        }
      }}
      initialValues={{
        ...props.values,
        status: props.values?.id === undefined ? true : props.values?.status === 1,
      }}
    >
      <ProFormTreeSelect
        name="parentId"
        label="上级组织/部门"
        fieldProps={{
          showSearch: true,
        }}
        placeholder="默认为顶级组织"
        request={async () => {
          const { data } = await orgTreeSelectForSelf();
          return data || [];
        }}
      />
      <ProFormText
        name="name"
        label={'名称'}
        rules={[
          {
            required: true,
            message: '请输入名称',
          },
        ]}
      />
      <ProFormSelect
        name="roleIds"
        label="角色"
        showSearch
        request={async () => {
          const { data } = await roleList();
          return data || [];
        }}
        fieldProps={{
          mode: 'multiple',
        }}
      />
      <ProFormTextArea
        name="remarks"
        label={'描述'}
        placeholder={'请输入'}
      />
      {props.values?.id === undefined && (
        <ProFormSwitch name="status" label="启用" />)
      }
    </ModalForm >
  );
};

export default CreateOrUpdateForm;