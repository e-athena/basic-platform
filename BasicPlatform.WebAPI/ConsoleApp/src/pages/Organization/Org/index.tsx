import { query, detail, statusChange } from './service';
import { PlusOutlined, FormOutlined } from '@ant-design/icons';
import { ActionType, ProCard, ProColumns, ProDescriptionsItemProps } from '@ant-design/pro-components';
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
import { canAccessible, hasPermission } from '@/utils/utils';
import CreateOrUpdateForm from './components/CreateOrUpdateForm';
import OrganizationTree, { OrganizationTreeInstance } from '@/components/OrganizationTree';


const TableList: React.FC = () => {
  const [createOrUpdateModalOpen, handleCreateOrUpdateModalOpen] = useState<boolean>(false);
  const [showDetail, setShowDetail] = useState<boolean>(false);

  const actionRef = useRef<ActionType>();
  const [currentRow, setCurrentRow] = useState<API.OrgListItem>();
  // const [selectedRowsState, setSelectedRows] = useState<API.OrgListItem[]>([]);
  const { getResource } = useModel('resource');
  const location = useLocation();
  const resource = getResource(location.pathname);
  const hideInTable: boolean = !hasPermission([
    permission.organization.postAsync,
    permission.organization.putAsync
  ], resource);

  const columns: ProColumns<API.OrgListItem>[] = [
    {
      title: '名称',
      dataIndex: 'name',
      hideInSearch: true,
      width: 150,
    },
    {
      title: '负责人',
      dataIndex: 'leaderName',
      hideInSearch: true,
      ellipsis: true,
      width: 120,
    },
    {
      title: '备注',
      dataIndex: 'remarks',
      hideInSearch: true,
    },
    {
      title: '排序',
      dataIndex: 'sort',
      hideInSearch: true,
      align: 'center',
      sorter: true,
      width: 70,
    },
    {
      title: '状态',
      dataIndex: 'status',
      width: 90,
      hideInSearch: true,
      align: 'center',
      render(_, entity) {
        if (canAccessible(permission.organization.statusChangeAsync, resource)) {
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
      hideInTable: true
    },
    {
      title: '更新时间',
      dataIndex: 'updatedOn',
      width: 170,
      hideInSearch: true,
      valueType: 'dateTime',
      hideInTable: true
    },
    {
      title: '操作',
      dataIndex: 'option',
      valueType: 'option',
      width: 180,
      hideInTable: hideInTable,
      render(_, entity) {
        return [
          <Access key={'edit'} accessible={canAccessible(permission.organization.putAsync, resource)}>
            <Button
              shape="circle"
              type={'link'}
              icon={<FormOutlined />}
              onClick={async () => {
                const hide = message.loading('正在查询');
                const res = await detail(entity.id!);
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
          </Access>,
          <Access key={'create'} accessible={canAccessible(permission.organization.postAsync, resource)}>
            <Button
              shape="circle"
              type={'link'}
              icon={<FormOutlined />}
              onClick={() => {
                setCurrentRow({ parentId: entity.id });
                handleCreateOrUpdateModalOpen(true);
              }}>
              添加子组织
            </Button>
          </Access>,
        ];
      },
    },
  ];
  const [organizationId, setOrganizationId] = useState<string | null>(null);
  const orgActionRef = useRef<OrganizationTreeInstance>();

  return (
    <PageContainer header={{
      title: resource?.name,
      children: resource?.description
    }}>
      <ProCard split="vertical">
        <ProCard colSpan="270px">
          <OrganizationTree
            ref={orgActionRef}
            onSelect={(key) => {
              setOrganizationId(key);
            }}
          />
        </ProCard>
        <ProCard>
          <ProTable<API.OrgListItem, API.OrgPagingParams>
            headerTitle={'查询表格'}
            actionRef={actionRef}
            rowKey="id"
            search={false}
            options={{
              search: {
                placeholder: '请输入名称',
              }
            }}
            toolBarRender={() => [
              <Access key={'add'} accessible={canAccessible(permission.organization.postAsync, resource)}>
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
            params={{
              parentId: organizationId,
            }}
            request={async (params: API.OrgPagingParams) => {
              const res = await query(params);
              return {
                data: res.data?.items || [],
                // success: res.success,
                total: res.data?.totalPages || 0,
              }
            }}
            columns={columns}
          />
        </ProCard>
      </ProCard>
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
          if (orgActionRef.current) {
            orgActionRef.current.reload();
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
          <ProDescriptions<API.OrgListItem>
            column={2}
            title={currentRow?.name}
            request={async () => ({
              data: currentRow || {},
            })}
            params={{
              id: currentRow?.name,
            }}
            columns={columns as ProDescriptionsItemProps<API.OrgListItem>[]}
          />
        )}
      </Drawer>
    </PageContainer>
  );
};

export default TableList;