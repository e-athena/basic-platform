import pattern from '@/utils/pattern';
import { generateTextImage, submitHandle } from '@/utils/utils';
import {
  ProFormText,
  ModalForm,
  ProForm,
  ProFormSelect,
  ProFormRadio,
  FormInstance,
  ProFormCascader,
} from '@ant-design/pro-components';
import React from 'react';
import { update, create } from '../service';
import { roleList } from '@/services/ant-design-pro/system/role';
import { positionList } from '@/services/ant-design-pro/system/position';
import { orgCascader } from '@/services/ant-design-pro/system/org';

type CreateOrUpdateFormProps = {
  onCancel: () => void;
  onSuccess: () => void;
  open: boolean;
  values?: API.UserDetailInfo;
};

const CreateOrUpdateForm: React.FC<CreateOrUpdateFormProps> = (props) => {
  const [organizationId, setOrganizationId] = React.useState<string | undefined>();
  const formRef = React.useRef<FormInstance>();
  return (
    <ModalForm
      width={592}
      title={props.values?.id === undefined ? '创建新用户' : '更新用户'}
      open={props.open}
      formRef={formRef}
      modalProps={{
        onCancel: () => {
          props.onCancel();
        },
        bodyStyle: { padding: '32px 40px 48px' },
        destroyOnClose: true,
      }}
      onFinish={async (values: API.UpdateUserRequest) => {
        if (values.organizationId !== undefined) {
          if (values.organizationId.length !== 0) {
            values.organizationId = values.organizationId[values.organizationId.length - 1];
          } else {
            delete values.organizationId;
          }
        }
        const isUpdate = props.values !== undefined;
        if (isUpdate) {
          values.id = props.values!.id!;
        }
        values.avatar = generateTextImage(values.realName!, 400, 400);
        let succeed;
        if (isUpdate) {
          succeed = await submitHandle(update, values);
        } else {
          succeed = await submitHandle(create, values as API.CreateUserRequest);
        }
        if (succeed) {
          props.onSuccess();
        }
      }}
      initialValues={{
        ...props.values,
        organizationId:
          props.values?.organizationPath === undefined ||
          props.values?.organizationPath === '' ||
          props.values?.organizationPath === null
            ? []
            : props.values?.organizationPath.split(','),
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
      </ProForm.Group>
      <ProForm.Group>
        <ProFormText name="NickName" label={'昵称'} width="sm" />
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
      <ProForm.Group>
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
        <ProFormText
          name="email"
          label={'邮箱'}
          width="sm"
          rules={[
            {
              message: '请输入正确的邮箱!',
              pattern: pattern.email,
            },
          ]}
        />
      </ProForm.Group>
      <ProFormCascader
        name="organizationId"
        label="组织/部门"
        fieldProps={{
          showSearch: true,
          changeOnSelect: true,
          onChange: (values: string[] | any) => {
            formRef.current?.setFieldsValue({
              positionId: undefined,
            });
            if (values) {
              setOrganizationId(values[values.length - 1]);
            }
          },
        }}
        request={async () => {
          const { data } = await orgCascader();
          return (data || []) as any;
        }}
        rules={[
          {
            required: true,
            message: '请选择',
          },
        ]}
      />
      <ProFormSelect
        name="positionId"
        label="职位"
        showSearch
        placeholder="请先选择组织/部门"
        params={{
          organizationId: organizationId || props.values?.organizationId,
        }}
        request={async (params: { organizationId?: string }) => {
          const { data } = await positionList(params.organizationId);
          return data || [];
        }}
        rules={[
          {
            required: true,
            message: '请选择',
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
    </ModalForm>
  );
};

export default CreateOrUpdateForm;
