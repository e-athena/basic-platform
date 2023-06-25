import pattern from '@/utils/pattern';
import { generateTextImage, submitHandle } from '@/utils/utils';
import {
  ProFormText,
  ModalForm,
  ProForm,
  ProFormRadio,
  FormInstance,
} from '@ant-design/pro-components';
import React from 'react';
import { createSuperAdmin, updateSuperAdmin } from '../service';

type CreateOrUpdateSuperAdminFormProps = {
  onCancel: () => void;
  onSuccess: () => void;
  open: boolean;
  values: API.TenantSuperAdminItem;
};

const CreateOrUpdateSuperAdminForm: React.FC<CreateOrUpdateSuperAdminFormProps> = (props) => {
  const formRef = React.useRef<FormInstance>();
  return (
    <ModalForm
      width={592}
      title={props.values.id ? '更新租户管理员信息' : '创建租户管理员'}
      open={props.open}
      formRef={formRef}
      modalProps={{
        onCancel: () => {
          props.onCancel();
        },
        bodyStyle: { padding: '32px 40px 48px' },
        destroyOnClose: true,
      }}
      onFinish={async (values: API.UpdateTenantSuperAdminRequest) => {
        // 租户编码
        values.code = props.values.code;
        const isUpdate = props.values.id !== undefined;
        if (isUpdate) {
          values.id = props.values!.id!;
        }
        if (values.avatar === undefined) {
          values.avatar = generateTextImage(values.realName!, 400, 400);
        }
        let succeed;
        if (isUpdate) {
          succeed = await submitHandle(updateSuperAdmin, values);
        } else {
          succeed = await submitHandle(createSuperAdmin, values as API.CreateTenantSuperAdminRequest);
        }
        if (succeed) {
          props.onSuccess();
        }
      }}
      initialValues={{
        ...props.values
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

export default CreateOrUpdateSuperAdminForm;
