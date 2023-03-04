import { submitHandle } from '@/utils/utils';
import {
  ProFormText,
  ProFormTextArea,
  StepsForm,
} from '@ant-design/pro-components';
import { Modal } from 'antd';
import React from 'react';
import { update } from '../service';
import Authorization from './Authorization';

type UpdateFormProps = {
  onCancel: () => void;
  onSuccess: () => void;
  open: boolean;
  values?: API.RoleDetailItem;
};

const UpdateForm: React.FC<UpdateFormProps> = (props) => {
  const [codes, setCodes] = React.useState<string[]>([]);
  return (
    <StepsForm
      stepsProps={{
        size: 'small',
      }}
      stepsFormRender={(dom, submitter) => {
        return (
          <Modal
            width={860}
            bodyStyle={{ padding: '32px 40px 48px' }}
            destroyOnClose
            title={'更新角色'}
            open={props.open}
            footer={submitter}
            onCancel={() => {
              props.onCancel();
            }}
          >
            {dom}
          </Modal>
        );
      }}
      onFinish={async (values: API.UpdateRoleItem) => {
        values.resourceCodes = codes;
        values.id = props.values!.id!;
        console.log(values);
        const succeed = await submitHandle(update, values);
        if (succeed) {
          props.onSuccess();
        }
      }}
    >
      <StepsForm.StepForm
        title={'基本信息'}
        initialValues={{
          name: props.values?.name,
          remarks: props.values?.remarks,
        }}
      >
        <ProFormText
          name="name"
          label={'角色名称'}
          width="md"
          rules={[
            {
              required: true,
              message: '请输入角色名称',
            },
          ]}
        />
        <ProFormTextArea
          name="remarks"
          width="md"
          label={'角色描述'}
          placeholder={'请输入'}
        />
      </StepsForm.StepForm>
      <StepsForm.StepForm title={'配置权限'}>
        <Authorization
          resourceCodes={props.values?.resourceCodes}
          onChange={(codes) => {
            setCodes(codes);
          }} />
      </StepsForm.StepForm>
    </StepsForm>
  );
};

export default UpdateForm;