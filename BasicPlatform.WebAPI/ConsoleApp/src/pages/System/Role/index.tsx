import { query, detail, statusChange } from './service';
import { PlusOutlined, FormOutlined } from '@ant-design/icons';
import type { ActionType, ProColumns, ProDescriptionsItemProps } from '@ant-design/pro-components';
import {
  PageContainer,
  ProDescriptions,
  ProTable,
} from '@ant-design/pro-components';
import { FormattedMessage, useModel, useLocation, Access } from '@umijs/max';
import { Button, Drawer, message, Modal, Switch } from 'antd';
import React, { useRef, useState } from 'react';
import IconStatus from '@/components/IconStatus';
import permission from '@/utils/permission';
import { canAccessible, getSorter, hasPermission } from '@/utils/utils';
import CreateOrUpdateForm from './components/CreateOrUpdateForm';


const TableList: React.FC = () => {
  const [createOrUpdateModalOpen, handleCreateOrUpdateModalOpen] = useState<boolean>(false);
  const [showDetail, setShowDetail] = useState<boolean>(false);

  const actionRef = useRef<ActionType>();
  const [currentRow, setCurrentRow] = useState<API.RoleDetailItem>();
  // const [selectedRowsState, setSelectedRows] = useState<API.RoleListItem[]>([]);
  const { getResource } = useModel('resource');
  const location = useLocation();
  const resource = getResource(location.pathname);
  const hideInTable: boolean = !hasPermission([
    permission.role.putAsync
  ], resource);

  const columns: ProColumns<API.RoleListItem>[] = [
    {
      title: '名称',
      dataIndex: 'name',
      hideInSearch: true,
      width: 150,
    },
    {
      title: '备注',
      dataIndex: 'remarks',
      hideInSearch: true,
    },
    {
      title: '状态',
      dataIndex: 'status',
      width: 90,
      hideInSearch: true,
      sorter: true,
      render(_, entity) {
        if (canAccessible(permission.role.statusChangeAsync, resource)) {
          return <Switch
            checkedChildren="启用"
            unCheckedChildren="禁用"
            checked={entity.status === 1}
            onClick={async () => {
              const statusName = entity.status === 1 ? '禁用' : '启用';
              Modal.confirm({
                title: '操作提示',
                content: `确定${statusName}{${entity.name}}吗？`,
                onOk: async () => {
                  const hide = message.loading(`正在${statusName}`);
                  const res = await statusChange(entity.id!);
                  hide();
                  if (res.success) {
                    actionRef.current?.reload();
                    return;
                  }
                  message.error(res.message);
                }
              });
            }}
          />
        }
        return <IconStatus status={entity.status === 1} />;
      },
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
      width: 170,
      hideInSearch: true,
      valueType: 'dateTime',
      sorter: true,
    },
    {
      title: '操作',
      dataIndex: 'option',
      valueType: 'option',
      hideInTable: hideInTable,
      width: 95,
      render(_, entity) {
        return [
          <Access key={'edit'} accessible={canAccessible(permission.role.putAsync, resource)}>
            <Button
              shape="circle"
              type={'link'}
              icon={<FormOutlined />}
              onClick={async () => {
                const hide = message.loading('正在查询');
                const res = await detail(entity.id);
                hide();
                if (res.success) {
                  setCurrentRow(res.data);
                  handleCreateOrUpdateModalOpen(true);
                  return;
                }
                message.error(res.message);
              }}>
              编辑
            </Button>
          </Access>
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
        headerTitle={'查询表格'}
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
                handleCreateOrUpdateModalOpen(true);
              }}
              icon={<PlusOutlined />}
            >
              <FormattedMessage id="pages.searchTable.new" defaultMessage="New" />
            </Button>
          </Access>,
        ]}
        request={async (params, sorter) => {
          const res = await query({ ...params, ...getSorter(sorter, 'a') });
          return {
            data: res.data?.items || [],
            success: res.success,
            total: res.data?.totalItems || 0,
          }
        }}
        columns={columns}
      />
      <CreateOrUpdateForm
        onCancel={() => {
          handleCreateOrUpdateModalOpen(false);
          if (!showDetail) {
            setCurrentRow(undefined);
          }
        }}
        onSuccess={() => {
          handleCreateOrUpdateModalOpen(false);
          if (!showDetail) {
            setCurrentRow(undefined);
          }
          if (actionRef.current) {
            actionRef.current.reload();
          }
        }}
        open={createOrUpdateModalOpen}
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