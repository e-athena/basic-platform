import { submitHandle } from '@/utils/utils';
import { ModalForm } from '@ant-design/pro-components';
import React from 'react';
import { assignResources } from '../service';
import Authorization from '@/components/Authorization';

type AuthorizationFormProps = {
  onCancel: () => void;
  onSuccess: () => void;
  open: boolean;
  roleResources: ResourceModel[];
  roleId: string;
  title?: string;
};

const AuthorizationForm: React.FC<AuthorizationFormProps> = (props) => {
  const [resources, setResources] = React.useState<ResourceModel[]>([]);
  return (
    <ModalForm
      width={860}
      title={props.title || '资源授权'}
      open={props.open}
      modalProps={{
        onCancel: () => {
          props.onCancel();
        },
        // bodyStyle: { padding: '32px 40px 48px' },
        bodyStyle: { padding: '10px 0' },
        destroyOnClose: true,
      }}
      onFinish={async () => {
        const succeed = await submitHandle(assignResources, {
          id: props.roleId,
          resources: resources,
        });
        if (succeed) {
          props.onSuccess();
        }
      }}
    >
      <Authorization
        resources={props.roleResources}
        onChange={(codes) => {
          setResources(codes);
        }}
        height={500}
      />
    </ModalForm>
  );
};

export default AuthorizationForm;
