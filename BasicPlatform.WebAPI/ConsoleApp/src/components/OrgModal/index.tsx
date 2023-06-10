import React, { useState } from 'react';
import { Modal } from 'antd';
import { ProCard } from '@ant-design/pro-components';
import OrganizationTree from '../OrganizationTree';
import TransferForm, { TransferFormProps, TransferOrgInfo } from './components/TransferForm';

type OrgModalProps = {
  onCancel?: () => void;
  onOk?: (keys: string[], rows: TransferOrgInfo[]) => void;
  open: boolean;
} & Partial<TransferFormProps>;

const OrgModal: React.FC<OrgModalProps> = (props) => {
  const [organizationId, setOrganizationId] = useState<string | null>(null);
  const [selectedKeys, setSelectedKeys] = useState<string[]>([]);
  const [selectedRows, setSelectedRows] = useState<TransferOrgInfo[]>([]);

  return (
    <>
      <Modal
        open={props.open}
        title={'选择组织/机构'}
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
              maxHeight={500}
              onSelect={(key) => {
                setOrganizationId(key);
              }}
            />
          </ProCard>
          <ProCard>
            <TransferForm
              mode={props.mode}
              onChange={(keys: string[], rows: TransferOrgInfo[]) => {
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

export default OrgModal;
