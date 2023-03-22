import { query, detail, statusChange } from './service';
import { PlusOutlined, FormOutlined } from '@ant-design/icons';
import { ActionType, ProCard, ProColumns } from '@ant-design/pro-components';
import {
  PageContainer,
} from '@ant-design/pro-components';
import { FormattedMessage, useModel, useLocation, Access } from '@umijs/max';
import { Button, message, Modal, Switch } from 'antd';
import React, { useRef, useState } from 'react';
import IconStatus from '@/components/IconStatus';
import permission from '@/utils/permission';
import { canAccessible, hasPermission } from '@/utils/utils';
import CreateOrUpdateForm from './components/CreateOrUpdateForm';
import OrganizationTree, { OrganizationTreeInstance } from '@/components/OrganizationTree';
import { useSize } from 'ahooks';
import ProTablePlus from '@/components/ProTablePlus';


const TableList: React.FC = () => {
  const [createOrUpdateModalOpen, handleCreateOrUpdateModalOpen] = useState<boolean>(false);

  const actionRef = useRef<ActionType>();
  // 获取card的宽度，用于计算表格的宽度
  const tableSize = useSize(document.getElementsByClassName('ant-pro-card-body')[0]);
  const [currentRow, setCurrentRow] = useState<API.OrgListItem>();
  // const [selectedRowsState, setSelectedRows] = useState<API.OrgListItem[]>([]);
  const { getResource } = useModel('resource');
  const location = useLocation();
  const resource = getResource(location.pathname);
  const hideInTable: boolean = !hasPermission([
    permission.organization.postAsync,
    permission.organization.putAsync
  ], resource);

  const [defaultColumns] = useState<ProColumns<API.OrgListItem>[]>([
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
                  const hide = message.loading(`正在${statusName}`, 0);
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
      title: '操作',
      dataIndex: 'option',
      valueType: 'option',
      width: 160,
      hideInTable: hideInTable,
      render(_, entity) {
        return [
          <Access key={'edit'} accessible={canAccessible(permission.organization.putAsync, resource)}>
            <Button
              shape="circle"
              type={'link'}
              icon={<FormOutlined />}
              onClick={async () => {
                const hide = message.loading('正在查询', 0);
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
  ]);
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
          <ProTablePlus<API.OrgListItem, API.OrgPagingParams>
            actionRef={actionRef}
            style={tableSize?.width ? {
              maxWidth: tableSize?.width - 270 - 24
            } : {}}
            defaultColumns={defaultColumns}
            query={query}
            moduleName={'Organization'}
            params={{
              parentId: organizationId,
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
          />
        </ProCard>
      </ProCard>
      <CreateOrUpdateForm
        onCancel={() => {
          handleCreateOrUpdateModalOpen(false);
          setCurrentRow(undefined);
        }}
        onSuccess={() => {
          handleCreateOrUpdateModalOpen(false);
          setCurrentRow(undefined);
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
    </PageContainer>
  );
};

export default TableList;
