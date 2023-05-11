import React, { useState } from 'react';
import { Modal } from 'antd';
import { ProCard } from '@ant-design/pro-components';
import OrganizationTree from '../OrganizationTree';
import TransferForm, { TransferFormProps, TransferUserInfo } from './components/TransferForm';

type UserModalProps = {
  onCancel?: () => void;
  onOk?: (keys: string[], rows: TransferUserInfo[]) => void;
  open: boolean;
  title?: string;
} & Partial<TransferFormProps>;

const UserModal: React.FC<UserModalProps> = (props) => {
  const [organizationId, setOrganizationId] = useState<string | null>(null);
  const [selectedKeys, setSelectedKeys] = useState<string[]>([]);
  const [selectedRows, setSelectedRows] = useState<TransferUserInfo[]>([]);

  return (
    <>
      <Modal
        open={props.open}
        title={props.title || '选择用户'}
        onOk={() => {
          if (props.onOk) {
            props.onOk(selectedKeys, selectedRows);
          }
        }}
        destroyOnClose
        width={1000}
        bodyStyle={{ paddingTop: 20, paddingBottom: 10, minHeight: 500 }}
        onCancel={props.onCancel}
      >
        <ProCard split="vertical" bordered>
          <ProCard colSpan="250px">
            <OrganizationTree
              onSelect={(key) => {
                setOrganizationId(key);
              }}
            />
          </ProCard>
          <ProCard>
            <TransferForm
              mode={props.mode}
              onChange={(keys: string[], rows: TransferUserInfo[]) => {
                setSelectedKeys(keys);
                setSelectedRows(rows);
              }}
              organizationId={organizationId || undefined}
              defaultSelectedKeys={props.defaultSelectedKeys}
            />
          </ProCard>
        </ProCard>
      </Modal>
    </>
  );
};

export default UserModal;
