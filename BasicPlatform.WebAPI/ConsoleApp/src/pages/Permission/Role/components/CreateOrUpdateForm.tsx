import { submitHandle } from '@/utils/utils';
import { ProFormText, ProFormTextArea, ModalForm, ProForm } from '@ant-design/pro-components';
import React from 'react';
import { update, create } from '../service';
import Authorization from '@/components/Authorization';

type CreateOrUpdateFormProps = {
  onCancel: () => void;
  onSuccess: () => void;
  open: boolean;
  values?: API.RoleDetailItem;
};

const CreateOrUpdateForm: React.FC<CreateOrUpdateFormProps> = (props) => {
  const [resources, setResources] = React.useState<ResourceModel[]>([]);
  return (
    <ModalForm
      width={860}
      title={props.values === undefined ? '创建角色' : '更新角色'}
      open={props.open}
      modalProps={{
        onCancel: () => {
          props.onCancel();
        },
        bodyStyle: { padding: '32px 40px 8px' },
        destroyOnClose: true,
      }}
      onFinish={async (values: API.UpdateRoleItem) => {
        const isUpdate = props.values !== undefined;
        values.resources = resources;
        if (isUpdate) {
          values.id = props.values!.id!;
        }
        const req = isUpdate ? update : create;
        const succeed = await submitHandle(req, values);
        if (succeed) {
          props.onSuccess();
        }
      }}
      initialValues={{
        name: props.values?.name,
        remarks: props.values?.remarks,
      }}
    >
      <ProForm.Group>
        <ProFormText
          name="name"
          label={'名称'}
          width="md"
          rules={[
            {
              required: true,
              message: '请输入角色名称',
            },
          ]}
        />
        <ProFormTextArea name="remarks" width="md" label={'描述'} placeholder={'请输入'} />
      </ProForm.Group>
      <ProForm.Item label={'资源授权'}>
        <Authorization
          resources={props.values?.resources}
          onChange={(codes) => {
            setResources(codes);
          }}
        />
      </ProForm.Item>
    </ModalForm>
  );
};

export default CreateOrUpdateForm;
