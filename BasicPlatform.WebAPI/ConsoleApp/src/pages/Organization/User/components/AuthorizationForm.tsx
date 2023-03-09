import { submitHandle } from '@/utils/utils';
import {
  ModalForm,
  ProDescriptions,
  ProFormDateTimePicker
} from '@ant-design/pro-components';
import React from 'react';
import { assignResources } from '../service';
import Authorization from '@/components/Authorization';
import { Alert } from 'antd';

type AuthorizationFormProps = {
  onCancel: () => void;
  onSuccess: () => void;
  open: boolean;
  values?: API.UserDetailInfo;
  resourceCodeInfo: API.UserResourceCodeInfo;
};

const CreateOrUpdateForm: React.FC<AuthorizationFormProps> = (props) => {
  const [resources, setResources] = React.useState<ResourceModel[]>([]);
  return (
    <ModalForm
      width={860}
      title={'资源授权'}
      open={props.open}
      modalProps={{
        onCancel: () => {
          props.onCancel();
        },
        bodyStyle: { padding: '32px 40px 48px' },
        destroyOnClose: true,
      }}
      onFinish={async (values) => {
        const succeed = await submitHandle(assignResources, {
          id: props.values!.id!,
          resources: resources,
          expireAt: values.expireAt,
        })
        if (succeed) {
          props.onSuccess();
        }
      }}
      initialValues={{
        ...props.values,
      }}
    >
      <Alert message="禁用的资源是角色自带的，用户已经拥有，其他可选的是用户可分配的额外资源权限。" type="warning" />
      <ProDescriptions
        style={{ marginTop: 24 }}
        columns={[{
          title: '登录名',
          dataIndex: 'userName'
        }, {
          title: '姓名',
          dataIndex: 'realName'
        }]
        } column={2} dataSource={props.values}
      />
      <ProFormDateTimePicker
        name="expireAt"
        label="有效期至"
        tooltip="资源授权的有效期，超过有效期将自动失效。"
        placeholder={'请选择'}
      />
      <Authorization
        resources={[...props.resourceCodeInfo.roleResources, ...props.resourceCodeInfo.userResources]}
        disabledResourceKeys={props.resourceCodeInfo.roleResources.map(r => r.key)}
        onChange={(_, currentCodes) => {
          setResources(currentCodes || []);
        }} />
    </ModalForm>
  );
};

export default CreateOrUpdateForm;