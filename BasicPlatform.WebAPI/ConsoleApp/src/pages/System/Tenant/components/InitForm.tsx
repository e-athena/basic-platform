import pattern from '@/utils/pattern';
import { submitHandle } from '@/utils/utils';
import {
  ProFormText,
  ModalForm,
  ProForm,
  ProFormRadio,
  FormInstance,
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
  const formRef = React.useRef<FormInstance>();
  return (
    <ModalForm
      width={592}
      title={'创建租户管理员'}
      open={props.open}
      formRef={formRef}
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
        userName: props.values.contactPhoneNumber,
        realName: props.values.contactName,
        nickName: props.values.contactName,
        phoneNumber: props.values.contactPhoneNumber,
      }}
    >
      <ProForm.Group>
        <ProForm.Group>
          <ProFormText
            name="realName"
            label={'姓名'}
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
            rules={[
              {
                message: '请输入正确的手机号!',
                pattern: pattern.mobile,
              },
            ]}
          />
        </ProForm.Group>
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
          width="sm"
          label={'密码'}
          placeholder={'为空时将使用默认密码'}
        />
      </ProForm.Group>
      <ProForm.Group>
        <ProFormText name="nickName" label={'昵称'} width="sm" />
        <ProFormRadio.Group
          name="gender"
          label="性别"
          options={[
            {
              label: '男',
              value: 1,
            },
            {
              label: '女',
              value: 2,
            },
          ]}
        />
      </ProForm.Group>
      <ProFormText
        name="email"
        label={'邮箱'}
        rules={[
          {
            message: '请输入正确的邮箱!',
            pattern: pattern.email,
          },
        ]}
      />
    </ModalForm>
  );
};

export default InitForm;
