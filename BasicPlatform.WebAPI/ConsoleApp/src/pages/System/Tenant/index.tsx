import { query, detail, statusChange, syncStructure, querySuperAdmin } from './service';
import { PlusOutlined, FormOutlined, CopyOutlined, ShareAltOutlined, MoreOutlined, SyncOutlined, UserOutlined } from '@ant-design/icons';
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
import CreateOrUpdateSuperAdminForm from './components/CreateOrUpdateSuperAdminForm';
import { setItem } from '@/utils/storage';

const TableList: React.FC = () => {
  const [createOrUpdateModalOpen, handleCreateOrUpdateModalOpen] = useState<boolean>(false);
  const [resourceModalOpen, handleResourceModalOpen] = useState<boolean>(false);
  const [initModalOpen, handleInitModalOpen] = useState<boolean>(false);

  const actionRef = useRef<ActionType>();
  const [currentRow, setCurrentRow] = useState<API.TenantDetailItem>();
  const [currentSuperAdmin, setCurrentSuperAdmin] = useState<API.TenantSuperAdminItem>();
  const { getResource } = useModel('resource');
  const location = useLocation();
  const resource = getResource(location.pathname);
  const showOption: boolean = hasPermission([
    permission.tenant.postAsync,
    permission.tenant.putAsync,
    permission.tenant.assignResourcesAsync,
    permission.tenant.createSuperAdminAsync
  ], resource);
  const showMoreOption: boolean = hasPermission(
    [
      permission.tenant.postAsync,
      permission.tenant.assignResourcesAsync,
      permission.tenant.createSuperAdminAsync
    ],
    resource,
  );

  /**需要重写的Column */
  const [defaultColumns] = useState<ProColumns<API.TenantListItem>[]>([
    {
      dataIndex: 'expiredTime',
      render(_, entity) {

        // 过期时间
        // 1. 如果为null，显示永久有效且显示绿色
        // 2. 已过期，显示已过期，显示格式为已过期多少天，显示灰色
        // 3. 未过期，显示未过期，显示格式为: 剩余多少天，如果小于30天，显示红色，如果小于24小时，显示剩余多少小时
        const expiredTime = entity.expiredTime;
        const now = new Date();
        const expired = new Date(expiredTime);
        const diff = expired.getTime() - now.getTime();
        const days = Math.floor(diff / (24 * 3600 * 1000));
        const hours = Math.floor((diff % (24 * 3600 * 1000)) / (3600 * 1000));
        if (expiredTime === null) {
          return <span style={{ color: 'green' }}>永久有效</span>
        }
        if (days < 0) {
          return <span style={{ color: 'red' }}>已过期{Math.abs(days)}天</span>
        }
        if (days === 0 && hours < 24) {
          return <span style={{ color: 'red' }}>剩余{hours}小时</span>
        }
        if (days < 30) {
          return <span style={{ color: 'red' }}>剩余{days}天</span>
        }
        return <span>剩余{days}天</span>
      }
    },
    {
      dataIndex: 'code',
      render(_, entity) {
        return <Tooltip placement={'top'} title={'点击登录'}>
          <Button
            shape="circle"
            type={'link'}
            onClick={() => {
              // 如果已过期
              if (entity.expiredTime !== null && new Date(entity.expiredTime) < new Date()) {
                // 提示已过期
                message.error('租户已过期');
                return;
              }
              // // 如果未初始化
              // if (!entity.isInitDatabase) {
              //   // 提示未创建超级管理员
              //   message.error('请先创建超级管理员');
              //   return;
              // }
              setItem(APP_TENANT_INFO_KEY, JSON.stringify({
                code: entity.code,
                name: entity.name,
              }));
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
        if (canAccessible(permission.tenant.createSuperAdminAsync, resource)) {
          moreItems.push({
            key: 'super-admin',
            icon: <UserOutlined />,
            label: '设置管理员',
            title: '创建或编辑超级管理员'
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
                  if (key === 'super-admin') {
                    domEvent.stopPropagation();
                    const res = await querySuperAdmin(entity.code);
                    if (res.success && res.data) {
                      const d = { ...res.data, code: entity.code }
                      setCurrentSuperAdmin(d);
                    } else {
                      setCurrentSuperAdmin({
                        userName: entity.contactPhoneNumber,
                        realName: entity.contactName,
                        nickName: entity.contactName,
                        phoneNumber: entity.contactPhoneNumber,
                        code: entity.code,
                      });
                    }
                    handleInitModalOpen(true);
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
      {initModalOpen && currentSuperAdmin !== undefined && (
        <CreateOrUpdateSuperAdminForm
          onCancel={() => {
            handleInitModalOpen(false);
            setCurrentSuperAdmin(undefined);
          }}
          onSuccess={() => {
            handleInitModalOpen(false);
            setCurrentSuperAdmin(undefined);
            if (actionRef.current) {
              actionRef.current.reload();
            }
          }}
          open={initModalOpen}
          values={currentSuperAdmin} />
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
