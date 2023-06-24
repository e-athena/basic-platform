import { submitHandle } from '@/utils/utils';
import {
  ProFormText,
  ModalForm,
} from '@ant-design/pro-components';
import React from 'react';
import { init } from '../service';

type InitFormProps = {
  onCancel: () => void;
  onSuccess: () => void;
  open: boolean;
  values: API.TenantDetailItem;
};

const InitForm: React.FC<InitFormProps> = (props) => {
  return (
    <ModalForm
      width={480}
      title={'初始化租户'}
      open={props.open}
      modalProps={{
        onCancel: () => {
          props.onCancel();
        },
        bodyStyle: { padding: '32px 40px 48px' },
        destroyOnClose: true,
      }}
      onFinish={async (values: API.InitTenantRequest) => {
        const succeed = await submitHandle(init, {
          ...values,
          code: props.values.code!,
          id: props.values.id!,
        });
        if (succeed) {
          props.onSuccess();
        }
      }}
      initialValues={{
        ...props.values,
        adminUserName: props.values.contactPhoneNumber
      }}
    >
      <ProFormText
        name="contactName"
        label={'管理员姓名'}
        readonly
      />
      <ProFormText
        name="contactPhoneNumber"
        label={'管理员手机号'}
        readonly
      />
      <ProFormText
        name="contactEmail"
        label={'管理员邮箱'}
        readonly
      />
      <ProFormText
        name="adminUserName"
        label={'登录帐号'}
        tooltip={'管理员登录帐号，用于登录后台管理系统'}
        rules={[
          {
            required: true,
            message: '请输入登录帐号',
          },
        ]}
      />
      <ProFormText.Password
        name="adminPassword"
        label={'登录密码'}
        placeholder={'为空时将使用默认密码'}
      />
    </ModalForm>
  );
};

export default InitForm;
