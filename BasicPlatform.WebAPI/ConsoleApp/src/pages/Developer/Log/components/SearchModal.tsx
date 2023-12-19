import React, { useState } from 'react';
import { ModalForm, ProFormDateRangePicker, ProFormSelect, ProFormText } from '@ant-design/pro-components';
import UserModal from '@/components/UserModal';
import { TransferUserInfo } from '@/components/UserModal/components/TransferForm';
import { Button, FormInstance } from 'antd';
import { SelectOutlined } from '@ant-design/icons';
import { } from '../'

type SearchModalProps = {
  onCancel: () => void;
  open: boolean;
  values: API.LogPagingParams;
  serviceList: API.SelectInfo[];
  onSearch: (values: API.LogPagingParams) => void;
};

const logLevelArray = ['Trace', 'Debug', 'Information', 'Warning', 'Error', 'Critical', 'None'];
const logLevelOptions = logLevelArray.map((item, index) => ({ value: index, label: item }));

const SearchModal: React.FC<SearchModalProps> = (props) => {
  const [userModalOpen, handleUserModalOpen] = useState<boolean>(false);
  const [searchFilter, setSearchFilter] = useState<API.LogPagingParams>(props.values);
  const formRef = React.useRef<FormInstance>();
  return (
    <>
      <ModalForm
        width={458}
        title={'查询'}
        open={props.open}
        formRef={formRef}
        modalProps={{
          onCancel: () => {
            props.onCancel();
          },
          bodyStyle: { padding: '32px 40px 48px' },
          destroyOnClose: true,
        }}
        onFinish={async (values: API.LogPagingParams) => {
          props.onSearch(values);
        }}
        initialValues={{
          ...props?.values,
        }}
      >
        <ProFormSelect
          name="serviceName"
          showSearch
          request={async () => {
            return props.serviceList;
          }}
          label="服务名"
          placeholder="请选择"
          rules={[{
            required: true,
            message: '请选择服务'
          }]}
        />
        <ProFormDateRangePicker
          name="dateRange"
          width={'md'}
          label="日期"
          placeholder={['开始日期', '结束日期']}
        />
        <ProFormSelect
          name="logLevel"
          label="日志等级"
          placeholder="请选择"
          options={logLevelOptions}
        />
        <ProFormText
          name="traceId"
          label={'追踪ID'}
          rules={[{
            min: 32,
            message: '最小长度32位'
          }, {
            max: 32,
            message: '最大长度32位'
          }]}
        />
        <ProFormText
          name="realName"
          label={'操作人'}
          placeholder={'请选择'}
          addonAfter={
            <Button
              onClick={() => {
                handleUserModalOpen(true);
              }}
              icon={<SelectOutlined />}
            >
              选择
            </Button>
          }
          disabled
        />
      </ModalForm>

      {userModalOpen && (
        <UserModal
          title={'选择用户'}
          onCancel={() => {
            handleUserModalOpen(false);
          }}
          onOk={async (keys: string[], rows: TransferUserInfo[]) => {
            let userId;
            let realName;
            if (keys.length !== 0) {
              userId = keys[0];
              realName = rows[0].realName;
            }
            formRef.current?.setFieldsValue({ realName: realName });
            setSearchFilter({ ...searchFilter, userId: userId, realName: realName });
            handleUserModalOpen(false);
          }}
          mode={'single'}
          open={userModalOpen}
          defaultSelectedKeys={searchFilter.userId !== undefined ? [searchFilter.userId] : []}
        />
      )}
    </>
  );
};

export default SearchModal;