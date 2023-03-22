import React, { useState } from 'react';
import { Modal } from 'antd';
import { ProCard } from '@ant-design/pro-components';
import OrganizationTree from '../OrganizationTree';
import TransferForm, { TransferFormProps, TransferUserInfo } from './components/TransferForm';

type UserModalProps = {
  onCancel?: () => void;
  onOk?: (keys: string[], rows: TransferUserInfo[]) => void;
  open: boolean;
} & Partial<TransferFormProps>;

const App: React.FC<UserModalProps> = (props) => {
  const [organizationId, setOrganizationId] = useState<string | null>(null);
  const [selectedKeys, setSelectedKeys] = useState<string[]>([]);
  const [selectedRows, setSelectedRows] = useState<TransferUserInfo[]>([]);

  return (
    <>
      <Modal
        open={props.open}
        title={'选择用户'}
        onOk={() => {
          if (props.onOk) {
            props.onOk(selectedKeys, selectedRows);
          }
        }}
        width={1000}
        bodyStyle={{ paddingTop: 20, paddingBottom: 10, minHeight: 500 }}
        onCancel={props.onCancel}
      >
        <ProCard split="vertical">
          <ProCard colSpan="250px">
            <OrganizationTree
              onSelect={(key) => {
                setOrganizationId(key);
              }}
            />
          </ProCard>
          <ProCard>
            <TransferForm
              mode={'single'}
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

export default App;
