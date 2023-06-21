import { query, detail, statusChange } from './service';
import { PlusOutlined, FormOutlined, CopyOutlined, ShareAltOutlined, MoreOutlined } from '@ant-design/icons';
import type { ActionType, ProColumns } from '@ant-design/pro-components';
import { PageContainer } from '@ant-design/pro-components';
import { FormattedMessage, useModel, useLocation, Access } from '@umijs/max';
import { Button, Dropdown, Modal, Switch, message } from 'antd';
import React, { useRef, useState } from 'react';
import permission from '@/utils/permission';
import { canAccessible, hasPermission, queryDetail } from '@/utils/utils';
import CreateOrUpdateForm from './components/CreateOrUpdateForm';
import ProTablePlus from '@/components/ProTablePlus';
import IconStatus from '@/components/IconStatus';
import { ItemType } from 'antd/es/menu/hooks/useItems';
import AuthorizationForm from './components/AuthorizationForm';

const TableList: React.FC = () => {
  const [createOrUpdateModalOpen, handleCreateOrUpdateModalOpen] = useState<boolean>(false);
  const [resourceModalOpen, handleResourceModalOpen] = useState<boolean>(false);

  const actionRef = useRef<ActionType>();
  const [currentRow, setCurrentRow] = useState<API.TenantDetailItem>();
  const { getResource } = useModel('resource');
  const location = useLocation();
  const resource = getResource(location.pathname);
  const showOption: boolean = hasPermission([
    permission.tenant.postAsync,
    permission.tenant.putAsync,
    permission.tenant.assignResourcesAsync
  ], resource);
  const showMoreOption: boolean = hasPermission(
    [
      permission.tenant.postAsync,
      permission.tenant.assignResourcesAsync
    ],
    resource,
  );

  /**需要重写的Column */
  const [defaultColumns] = useState<ProColumns<API.TenantListItem>[]>([
    {
      title: '状态',
      dataIndex: 'status',
      width: 90,
      hideInSearch: true,
      align: 'center',
      sorter: true,
      render(_, entity) {
        if (canAccessible(permission.tenant.statusChangeAsync, resource)) {
          return (
            <Switch
              checkedChildren="启用"
              unCheckedChildren="禁用"
              checked={entity.status === 1}
              onClick={async (_, e) => {
                e.stopPropagation();
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
                  },
                });
              }}
            />
          );
        }
        return <IconStatus status={entity.status === 1} />;
      },
    },
    {
      title: '操作',
      dataIndex: 'option',
      valueType: 'option',
      hideInTable: !showOption,
      width: 125,
      render(_, entity) {
        const moreItems: ItemType[] = [];
        if (canAccessible(permission.tenant.postAsync, resource)) {
          moreItems.push({
            key: 'copy',
            icon: <CopyOutlined />,
            label: '复制新建',
          });
        }
        if (canAccessible(permission.tenant.assignResourcesAsync, resource)) {
          moreItems.push({
            key: 'resource',
            icon: <ShareAltOutlined />,
            label: '分配资源',
          });
        }
        return [
          <Access
            key={'edit'}
            accessible={canAccessible(permission.tenant.putAsync, resource)}
          >
            <Button
              shape="circle"
              type={'link'}
              icon={<FormOutlined />}
              onClick={async (e) => {
                e.stopPropagation();
                const data = await queryDetail(detail, entity.id);
                if (data) {
                  setCurrentRow(data);
                  handleCreateOrUpdateModalOpen(true);
                }
              }}
            >
              编辑
            </Button>
          </Access>,
          <Access key={'more'} accessible={showMoreOption}>
            <Dropdown
              menu={{
                items: moreItems,
                onClick: async ({ key, domEvent }) => {
                  domEvent.stopPropagation();
                  if (key === 'resource') {
                    const data = await queryDetail(detail, entity.id!);
                    if (data) {
                      setCurrentRow(data);
                      handleResourceModalOpen(true);
                    }
                    return;
                  }
                  if (key === 'copy') {
                    domEvent.stopPropagation();
                    const data = await queryDetail(detail, entity.id!);
                    if (data) {
                      data.id = undefined;
                      setCurrentRow(data);
                      handleCreateOrUpdateModalOpen(true);
                    }
                    return;
                  }
                },
              }}
              placement="bottom"
            >
              <Button
                shape="circle"
                type={'link'}
                icon={<MoreOutlined />}
                onClick={(e) => {
                  e.stopPropagation();
                }}
              />
            </Dropdown>
          </Access>,
        ];
      },
    },
  ]);

  return (
    <PageContainer
      header={{
        title: resource?.name,
        children: resource?.description,
      }}
    >
      <ProTablePlus<API.TenantListItem, API.TenantPagingParams>
        actionRef={actionRef}
        defaultColumns={defaultColumns}
        query={query}
        moduleName={'Tenant'}
        showDescriptions
        toolBarRender={() => [
          <Access
            key={'add'}
            accessible={canAccessible(permission.tenant.postAsync, resource)}
          >
            <Button
              type="primary"
              onClick={() => {
                setCurrentRow(undefined);
                handleCreateOrUpdateModalOpen(true);
              }}
              icon={<PlusOutlined />}
            >
              <FormattedMessage id="pages.searchTable.new" defaultMessage="New" />
            </Button>
          </Access>,
        ]}
      />
      {resourceModalOpen && (
        <AuthorizationForm
          title={`${currentRow?.name} - 分配资源`}
          onCancel={() => {
            handleResourceModalOpen(false);
          }}
          onSuccess={() => {
            handleResourceModalOpen(false);
          }}
          open={resourceModalOpen}
          tenantResources={currentRow!.resources}
          tenantId={currentRow!.id!}
        />
      )}
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
        }}
        open={createOrUpdateModalOpen}
        values={currentRow}
      />
    </PageContainer>
  );
};

export default TableList;
