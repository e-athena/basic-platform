import {
  ProFormText,
  ProFormTextArea,
  StepsForm,
} from '@ant-design/pro-components';
import { Modal } from 'antd';
import React from 'react';
import Authorization from './Authorization';
import { create } from '../service';
import { submitHandle } from '@/utils/utils';

type CreateFormProps = {
  onCancel: () => void;
  onSuccess: () => void;
  open: boolean;
};

const CreateForm: React.FC<CreateFormProps> = (props) => {
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
            title={'创建角色'}
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
      onFinish={async (values: API.CreateRoleItem) => {
        values.resourceCodes = codes;
        const succeed = await submitHandle(create, values);
        if (succeed) {
          props.onSuccess();
        }
      }}
    >
      <StepsForm.StepForm
        title={'基本信息'}
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
      <StepsForm.StepForm
        initialValues={{
          target: '0',
          template: '0',
        }}
        title={'配置权限'}
      >
        <Authorization
          onChange={(codes) => {
            setCodes(codes);
          }} />
      </StepsForm.StepForm>
    </StepsForm>
  );
};

export default CreateForm;
