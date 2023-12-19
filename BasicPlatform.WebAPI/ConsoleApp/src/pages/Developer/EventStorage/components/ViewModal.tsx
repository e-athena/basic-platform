import React from 'react';
import { Modal } from 'antd';
import { ProDescriptions } from '@ant-design/pro-components';
import { querySelectList } from '@/services/ant-design-pro/system/user';

type ViewDrawerProps = {
  onClose: () => void;
  open: boolean;
  values: API.EventStorageListItem;
};

const ViewModal: React.FC<ViewDrawerProps> = (props) => {
  const [userList, setUserList] = React.useState<API.SelectInfo[]>([]);
  React.useEffect(() => {
    if (!props.open) {
      return;
    }
    if (userList.length > 0) {
      return;
    }
    querySelectList().then((res) => {
      setUserList(res.data || []);
    });
  }, []);
  return (
    <>
      <Modal
        width={'50%'}
        open={props.open}
        onCancel={() => {
          props.onClose();
        }}
        title={'详情'}
      >
        <ProDescriptions
          column={1}
          labelStyle={{ width: '120px' }}
          title={false}
          size={'small'}
          bordered
          dataSource={props.values || {}}
        >
          <ProDescriptions.Item
            dataIndex={'aggregateRootId'}
            label={'业务实体ID'}
            copyable
            valueType={'text'}
          />
          <ProDescriptions.Item
            dataIndex={'aggregateRootTypeName'}
            label={'业务实体名称'}
            valueType={'text'}
          />
          <ProDescriptions.Item
            dataIndex={'version'}
            label={'版本号'}
            valueType={'text'}
          />
          <ProDescriptions.Item
            dataIndex={'eventId'}
            label={'事件ID'}
            valueType={'text'}
          />
          <ProDescriptions.Item
            dataIndex={'eventName'}
            label={'事件名称'}
            valueType={'text'}
          />
          <ProDescriptions.Item
            dataIndex={'userId'}
            label={'操作人ID'}
            copyable
            valueType={'text'}
          />
          <ProDescriptions.Item
            dataIndex={'userId'}
            label={'操作人'}
            valueType={'text'}
            renderText={(text) => {
              return userList.find((item) => item.value === text)?.label;
            }}
          />
          <ProDescriptions.Item
            dataIndex={'createdOn'}
            label={'存储时间'}
            valueType={'dateTime'}
          />
          <ProDescriptions.Item
            dataIndex={'events'}
            label={'事件数据'}
            // valueType={'jsonCode'}
          />
        </ProDescriptions>
      </Modal>
    </>
  );
};

export default ViewModal;