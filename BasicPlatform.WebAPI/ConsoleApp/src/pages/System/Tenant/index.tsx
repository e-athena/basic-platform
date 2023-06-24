import { query, detail, statusChange, syncStructure } from './service';
import { PlusOutlined, FormOutlined, CopyOutlined, ShareAltOutlined, MoreOutlined, DatabaseOutlined, SyncOutlined } from '@ant-design/icons';
import type { ActionType, ProColumns } from '@ant-design/pro-components';
import { PageContainer } from '@ant-design/pro-components';
import { FormattedMessage, useModel, useLocation, Access, history } from '@umijs/max';
import { Button, Dropdown, Modal, Switch, Tooltip, message } from 'antd';
import React, { useRef, useState } from 'react';
import permission from '@/utils/permission';
import { canAccessible, hasPermission, queryDetail, submitHandle } from '@/utils/utils';
import CreateOrUpdateForm from './components/CreateOrUpdateForm';
import ProTablePlus from '@/components/ProTablePlus';
import IconStatus from '@/components/IconStatus';
import { ItemType } from 'antd/es/menu/hooks/useItems';
import AuthorizationForm from './components/AuthorizationForm';
import InitForm from './components/InitForm';

const TableList: React.FC = () => {
  const [createOrUpdateModalOpen, handleCreateOrUpdateModalOpen] = useState<boolean>(false);
  const [resourceModalOpen, handleResourceModalOpen] = useState<boolean>(false);
  const [initModalOpen, handleInitModalOpen] = useState<boolean>(false);

  const actionRef = useRef<ActionType>();
  const [currentRow, setCurrentRow] = useState<API.TenantDetailItem>();
  const { getResource } = useModel('resource');
  const location = useLocation();
  const resource = getResource(location.pathname);
  const showOption: boolean = hasPermission([
    permission.tenant.postAsync,
    permission.tenant.putAsync,
    permission.tenant.assignResourcesAsync,
    permission.tenant.initAsync
  ], resource);
  const showMoreOption: boolean = hasPermission(
    [
      permission.tenant.postAsync,
      permission.tenant.assignResourcesAsync,
      permission.tenant.initAsync
    ],
    resource,
  );

  /**需要重写的Column */
  const [defaultColumns] = useState<ProColumns<API.TenantListItem>[]>([
    {
      dataIndex: 'code',
      render(_, entity) {
        return <Tooltip placement={'top'} title={'点击登录'}>
          <Button
            shape="circle"
            type={'link'}
            onClick={() => {
              history.push(`/user/login?t_code=${entity.code}`);
            }}>
            {entity.code}
          </Button>
        </Tooltip>
      },
    },
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
        if (canAccessible(permission.tenant.initAsync, resource)) {
          moreItems.push({
            key: 'init',
            icon: <DatabaseOutlined />,
            label: '初始化',
            title: '初始化数据库及创建超级管理员',
            disabled: entity.isInitDatabase
          });
        }
        if (canAccessible(permission.tenant.syncStructureAsync, resource)) {
          moreItems.push({
            key: 'sync',
            icon: <SyncOutlined />,
            label: '同步表结构',
            title: '同步主应用和所有子应用的数据库结构'
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
                  if (key === 'init') {
                    domEvent.stopPropagation();
                    const data = await queryDetail(detail, entity.id!);
                    if (data) {
                      setCurrentRow(data);
                      handleInitModalOpen(true);
                    }
                    return;
                  }
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
                  if (key === 'sync') {
                    domEvent.stopPropagation();
                    Modal.confirm({
                      title: '操作提示',
                      content: `确定要同步{${entity.name}}的数据库结构吗？`,
                      onOk: async () => {
                        const succeed = await submitHandle(syncStructure, entity.code, '同步');
                        if (succeed) {
                          actionRef.current?.reload();
                        }
                      }
                    });
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
      {initModalOpen && currentRow !== undefined && (
        <InitForm
          onCancel={() => {
            handleInitModalOpen(false);
            setCurrentRow(undefined);
          }}
          onSuccess={() => {
            handleInitModalOpen(false);
            setCurrentRow(undefined);
            if (actionRef.current) {
              actionRef.current.reload();
            }
          }}
          open={initModalOpen}
          values={currentRow} />
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
