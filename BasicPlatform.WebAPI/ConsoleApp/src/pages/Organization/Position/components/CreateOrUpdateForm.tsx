import { submitHandle } from '@/utils/utils';
import {
  ProFormText,
  ProFormTextArea,
  ModalForm,
  ProForm,
  ProFormTreeSelect,
  ProFormSwitch,
  ProFormDigit,
} from '@ant-design/pro-components';
import React from 'react';
import { update, create } from '../service';
import { orgTreeSelect } from '@/services/ant-design-pro/system/org';

type CreateOrUpdateFormProps = {
  onCancel: () => void;
  onSuccess: () => void;
  open: boolean;
  values?: API.PositionDetailItem;
};

const CreateOrUpdateForm: React.FC<CreateOrUpdateFormProps> = (props) => {
  return (
    <ModalForm
      width={600}
      title={props.values === undefined ? '创建职位' : '更新职位'}
      open={props.open}
      modalProps={{
        onCancel: () => {
          props.onCancel();
        },
        bodyStyle: { padding: '32px 40px 48px' },
        destroyOnClose: true,
      }}
      onFinish={async (values: API.UpdatePositionItem) => {
        const isUpdate = props.values !== undefined;
        let succeed;
        if (isUpdate) {
          values.id = props.values!.id!;
          succeed = await submitHandle(update, values);
        } else {
          values.status = values.status ? 1 : 2;
          succeed = await submitHandle(create, values as API.CreatePositionItem);
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
      <ProForm.Group>
        <ProFormTreeSelect
          name="organizationId"
          label="部门"
          fieldProps={{
            showSearch: true,
          }}
          width="sm"
          tooltip={'为空时为通用职位'}
          request={async () => {
            const { data } = await orgTreeSelect();
            return data || [];
          }}
          placeholder={'请选择'}
        />
        <ProFormText
          name="name"
          label={'名称'}
          width="sm"
          rules={[
            {
              required: true,
              message: '请输入职位名称',
            },
          ]}
        />
      </ProForm.Group>
      <ProFormDigit name="sort" label={'排序'} min={0} />
      <ProFormTextArea name="remarks" label={'描述'} placeholder={'请输入'} />
      {props.values?.id === undefined && <ProFormSwitch name="status" label="启用" />}
    </ModalForm>
  );
};

export default CreateOrUpdateForm;
