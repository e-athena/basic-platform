import { submitHandle } from '@/utils/utils';
import {
  ModalForm,
  ProDescriptions
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
  const [codes, setCodes] = React.useState<string[]>([]);
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
      onFinish={async () => {
        const succeed = await submitHandle(assignResources, {
          id: props.values!.id!,
          resourceCodes: codes,
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
      <Authorization
        resourceCodes={[...props.resourceCodeInfo.roleResourceCodes, ...props.resourceCodeInfo.userResourceCodes]}
        disabledResourceCodes={props.resourceCodeInfo.roleResourceCodes}
        onChange={(_, currentCodes) => {
          setCodes(currentCodes || []);
        }} />
    </ModalForm>
  );
};

export default CreateOrUpdateForm;