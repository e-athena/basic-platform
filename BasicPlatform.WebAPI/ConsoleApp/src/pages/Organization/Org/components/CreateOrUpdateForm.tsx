import { submitHandle } from '@/utils/utils';
import {
  ProFormText,
  ProFormTextArea,
  ModalForm,
  ProFormTreeSelect,
  ProFormSelect,
  ProFormSwitch,
  ProFormInstance
} from '@ant-design/pro-components';
import React, { useState } from 'react';
import { update, create } from '../service';
import { roleList } from '@/services/ant-design-pro/system/role'
import { orgTreeSelectForSelf } from '@/services/ant-design-pro/system/org'
import { Button } from 'antd';
import UserModal from '@/components/UserModal';
import { TransferUserInfo } from '@/components/UserModal/components/TransferForm';
import { SelectOutlined } from '@ant-design/icons';

type CreateOrUpdateFormProps = {
  onCancel: () => void;
  onSuccess: () => void;
  open: boolean;
  values?: API.OrgDetailItem;
};

const CreateOrUpdateForm: React.FC<CreateOrUpdateFormProps> = (props) => {
  const [userModalOpen, setUserModalOpen] = useState<boolean>(false);
  const [leaderId, setLeaderId] = useState<string | undefined>();
  const formRef = React.useRef<ProFormInstance>();
  return <>
    <ModalForm
      width={600}
      formRef={formRef}
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
        values.leaderId = leaderId;
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
      <ProFormText
        name="leaderName"
        label={'负责人'}
        placeholder={'请选择'}
        tooltip={'每个组织/部门只能有一个负责人'}
        width="md"
        fieldProps={{
          style: {
            backgroundColor: '#ffffff'
          }
        }}
        addonAfter={
          <Button
            onClick={() => {
              setUserModalOpen(true);
            }}
            icon={<SelectOutlined />}
          >选择</Button>}
        disabled
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
      <UserModal
        open={userModalOpen}
        onCancel={() => {
          setUserModalOpen(false);
        }}
        onOk={(keys: string[], rows: TransferUserInfo[]) => {
          setLeaderId(keys.length > 0 ? keys[0] : undefined);
          formRef.current?.setFieldsValue({
            leaderName: rows.length > 0 ? rows[0].realName : undefined,
          });
          setUserModalOpen(false);
        }}
        defaultSelectedKeys={leaderId ? [leaderId] : (props.values?.leaderId ? [props.values?.leaderId] : [])}
      />
    </ModalForm >
  </>
};

export default CreateOrUpdateForm;