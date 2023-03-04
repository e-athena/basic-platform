import { query, detail } from './service';
import { PlusOutlined, FormOutlined } from '@ant-design/icons';
import type { ActionType, ProColumns, ProDescriptionsItemProps } from '@ant-design/pro-components';
import {
  PageContainer,
  ProDescriptions,
  ProTable,
} from '@ant-design/pro-components';
import { FormattedMessage, useIntl, useModel, useLocation, Access } from '@umijs/max';
import { Button, Drawer, message } from 'antd';
import React, { useRef, useState } from 'react';
import UpdateForm from './components/UpdateForm';
import CreateForm from './components/CreateForm';
import IconStatus from '@/components/IconStatus';
import permission from '@/utils/permission';
import { canAccessible } from '@/utils/utils';


const TableList: React.FC = () => {
  /**
   * @en-US Pop-up window of new window
   * @zh-CN 新建窗口的弹窗
   *  */
  const [createModalOpen, handleModalOpen] = useState<boolean>(false);
  /**
   * @en-US The pop-up window of the distribution update window
   * @zh-CN 分布更新窗口的弹窗
   * */
  const [updateModalOpen, handleUpdateModalOpen] = useState<boolean>(false);

  const [showDetail, setShowDetail] = useState<boolean>(false);

  const actionRef = useRef<ActionType>();
  const [currentRow, setCurrentRow] = useState<API.RoleDetailItem>();
  // const [selectedRowsState, setSelectedRows] = useState<API.RoleListItem[]>([]);
  const { getResource } = useModel('resource');
  const location = useLocation();
  const resource = getResource(location.pathname);

  /**
   * @en-US International configuration
   * @zh-CN 国际化配置
   * */
  const intl = useIntl();

  const columns: ProColumns<API.RoleListItem>[] = [
    {
      title: '名称',
      dataIndex: 'name',
      hideInSearch: true,
      width: 150,
    },
    {
      title: '状态',
      dataIndex: 'status',
      width: 90,
      hideInSearch: true,
      sorter: true,
      render(_, entity) {
        return <IconStatus status={entity.status === 1} />;
      },
    },
    {
      title: '备注',
      dataIndex: 'remarks',
      hideInSearch: true,
    },
    {
      title: '更新人',
      dataIndex: 'updatedUserName',
      width: 100,
      hideInSearch: true,
    },
    {
      title: '更新时间',
      dataIndex: 'updatedOn',
      width: 160,
      hideInSearch: true,
      valueType: 'dateTime',
      sorter: true,
    },
    {
      title: '操作',
      dataIndex: 'option',
      valueType: 'option',
      width: 95,
      render(_, entity) {
        return [
          <Button key={'view'}
            shape="circle"
            type={'link'}
            icon={<FormOutlined />}
            onClick={async () => {
              const hide = message.loading('正在查询');
              const res = await detail(entity.id);
              hide();
              if (res.success) {
                setCurrentRow(res.data);
                handleUpdateModalOpen(true);
                return;
              }
              message.error(res.message);
            }}>
            编辑
          </Button>,
        ];
      },
    },
    {
      title: '关键字',
      dataIndex: 'keyword',
      hideInTable: true,
      hideInForm: true,
      hideInDescriptions: true,
      hideInSearch: false,
      hideInSetting: true,
      fieldProps: {
        placeholder: '搜索关键字',
      },
    },
  ];

  return (
    <PageContainer header={{
      title: resource?.name,
      children: resource?.description
    }}>
      <ProTable<API.RoleListItem, API.RolePagingParams>
        headerTitle={intl.formatMessage({
          id: 'pages.searchTable.title',
          defaultMessage: 'Enquiry form',
        })}
        actionRef={actionRef}
        rowKey="id"
        search={{
          labelWidth: 120,
        }}
        toolBarRender={() => [
          <Access key={'add'} accessible={canAccessible(permission.role.postAsync, resource)}>
            <Button
              type="primary"
              onClick={() => {
                handleModalOpen(true);
              }}
              icon={<PlusOutlined />}
            >
              <FormattedMessage id="pages.searchTable.new" defaultMessage="New" />
            </Button>
          </Access>,
        ]}
        request={async (params) => {
          const res = await query(params);
          return {
            data: res.data?.items || [],
            success: res.success,
            total: res.data?.totalItems || 0,
          }
        }}
        columns={columns}
      // rowSelection={{
      //   onChange: (_, selectedRows) => {
      //     setSelectedRows(selectedRows);
      //   },
      // }}
      />
      <CreateForm
        onCancel={() => {
          handleModalOpen(false);
        }}
        onSuccess={() => {
          handleModalOpen(false);
          if (actionRef.current) {
            actionRef.current.reload();
          }
        }}
        open={createModalOpen}
      />
      <UpdateForm
        onCancel={() => {
          handleUpdateModalOpen(false);
          if (!showDetail) {
            setCurrentRow(undefined);
          }
        }}
        onSuccess={() => {
          handleUpdateModalOpen(false);
          setCurrentRow(undefined);
          if (actionRef.current) {
            actionRef.current.reload();
          }
        }}
        open={updateModalOpen}
        values={currentRow}
      />

      <Drawer
        width={600}
        open={showDetail}
        onClose={() => {
          setCurrentRow(undefined);
          setShowDetail(false);
        }}
        closable={false}
      >
        {currentRow?.name && (
          <ProDescriptions<API.RoleListItem>
            column={2}
            title={currentRow?.name}
            request={async () => ({
              data: currentRow || {},
            })}
            params={{
              id: currentRow?.name,
            }}
            columns={columns as ProDescriptionsItemProps<API.RoleListItem>[]}
          />
        )}
      </Drawer>
    </PageContainer>
  );
};

export default TableList;
