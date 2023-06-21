import { submitHandle } from '@/utils/utils';
import { ModalForm } from '@ant-design/pro-components';
import React from 'react';
import { assignResources } from '../service';
import Authorization from '@/components/ApplicationAuthorization';

type AuthorizationFormProps = {
  onCancel: () => void;
  onSuccess: () => void;
  open: boolean;
  tenantResources: ResourceModel[];
  tenantId: string;
  title?: string;
};

const AuthorizationForm: React.FC<AuthorizationFormProps> = (props) => {
  const [resources, setResources] = React.useState<ResourceModel[]>([]);
  return (
    <ModalForm
      width={960}
      title={props.title || '资源授权'}
      open={props.open}
      modalProps={{
        onCancel: () => {
          props.onCancel();
        },
        destroyOnClose: true,
      }}
      onFinish={async () => {
        const succeed = await submitHandle(assignResources, {
          id: props.tenantId,
          resources: resources,
        });
        if (succeed) {
          props.onSuccess();
        }
      }}
    >
      <Authorization
        resources={props.tenantResources}
        onChange={(codes) => {
          setResources(codes);
        }}
        height={500}
      />
    </ModalForm>
  );
};

export default AuthorizationForm;
