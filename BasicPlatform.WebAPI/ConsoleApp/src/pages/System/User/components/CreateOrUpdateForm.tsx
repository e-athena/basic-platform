import pattern from '@/utils/pattern';
import { submitHandle } from '@/utils/utils';
import {
  ProFormText,
  ModalForm,
  ProForm,
  ProFormSelect,
  ProFormTreeSelect
} from '@ant-design/pro-components';
import React from 'react';
import { update, create } from '../service';
import { roleList } from '@/services/ant-design-pro/system/role'
import { orgTreeSelect } from '@/services/ant-design-pro/system/org'

type CreateOrUpdateFormProps = {
  onCancel: () => void;
  onSuccess: () => void;
  open: boolean;
  values?: API.UserDetailInfo;
};

const CreateOrUpdateForm: React.FC<CreateOrUpdateFormProps> = (props) => {
  return (
    <ModalForm
      width={600}
      title={props.values === undefined ? '创建新用户' : '更新用户'}
      open={props.open}
      modalProps={{
        onCancel: () => {
          props.onCancel();
        },
        bodyStyle: { padding: '32px 40px 48px' },
        destroyOnClose: true,
      }}
      onFinish={async (values: API.UpdateUserRequest) => {
        const isUpdate = props.values !== undefined;
        // values.resourceCodes = codes;
        if (isUpdate) {
          values.id = props.values!.id!;
        }
        let succeed;
        if (isUpdate) {
          succeed = await submitHandle(update, values);
        } else {
          succeed = await submitHandle(create, (values as API.CreateUserRequest));
        }
        if (succeed) {
          props.onSuccess();
        }
      }}
      initialValues={{
        ...props.values,
      }}
    >
      <ProForm.Group>
        <ProFormText
          name="userName"
          label={'登录名'}
          width="sm"
          rules={[
            {
              required: true,
              message: '请输入登录名',
            },
          ]}
        />
        <ProFormText.Password
          name="password"
          label={'密码'}
          width="sm"
          rules={[
            {
              required: true,
              message: '请输入密码',
            },
          ]}
        />
      </ProForm.Group>
      <ProForm.Group>
        <ProFormText
          name="realName"
          label={'真实姓名'}
          width="sm"
          rules={[
            {
              required: true,
              message: '请输入真实姓名',
            },
          ]}
        />
        <ProFormText
          name="phoneNumber"
          label={'手机号'}
          width="sm"
          rules={[{
            message: '请输入正确的手机号!',
            pattern: pattern.mobile
          }]}
        />
      </ProForm.Group>
      <ProFormText
        name="email"
        label={'邮箱'}
        rules={[{
          message: '请输入正确的邮箱!',
          pattern: pattern.email
        }]}
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
      <ProFormTreeSelect
        name="organizationIds"
        label="部门"
        fieldProps={{
          showSearch: true,
          multiple: true,
        }}
        request={async () => {
          const { data } = await orgTreeSelect();
          return data || [];
        }}
      />
    </ModalForm>
  );
};

export default CreateOrUpdateForm;