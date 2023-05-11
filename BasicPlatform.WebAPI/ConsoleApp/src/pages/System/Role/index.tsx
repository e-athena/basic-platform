import { query, detail, statusChange, assignUsers } from './service';
import {
  PlusOutlined,
  FormOutlined,
  SafetyOutlined,
  MoreOutlined,
  ShareAltOutlined,
  UserAddOutlined,
} from '@ant-design/icons';
import type { ActionType, ProColumns } from '@ant-design/pro-components';
import { PageContainer } from '@ant-design/pro-components';
import { FormattedMessage, useModel, useLocation, Access } from '@umijs/max';
import { Button, Dropdown, message, Modal, Switch } from 'antd';
import React, { useRef, useState } from 'react';
import IconStatus from '@/components/IconStatus';
import permission from '@/utils/permission';
import { canAccessible, hasPermission, queryDetail, submitHandle } from '@/utils/utils';
import CreateOrUpdateForm from './components/CreateOrUpdateForm';
import ProTablePlus from '@/components/ProTablePlus';
import { ItemType } from 'antd/lib/menu/hooks/useItems';
import AuthorizationForm from './components/AuthorizationForm';
import DataPermissionForm from './components/DataPermissionForm';
import UserModal from '@/components/UserModal';

const TableList: React.FC = () => {
  const [createOrUpdateModalOpen, handleCreateOrUpdateModalOpen] = useState<boolean>(false);
  const [resourceModalOpen, handleResourceModalOpen] = useState<boolean>(false);
  const [userModalOpen, handleUserModalOpen] = useState<boolean>(false);
  const [permissionModalOpen, handlePermissionModalOpen] = useState<boolean>(false);

  const actionRef = useRef<ActionType>();
  const [currentRow, setCurrentRow] = useState<API.RoleDetailItem>();
  const { getResource } = useModel('resource');
  const location = useLocation();
  const resource = getResource(location.pathname);
  const showOption: boolean = hasPermission(
    [
      permission.role.putAsync,
      permission.role.assignResourcesAsync,
      permission.role.assignDataPermissionsAsync,
      permission.role.assignUsersAsync,
    ],
    resource,
  );
  const showMoreOption: boolean = hasPermission(
    [
      permission.role.assignResourcesAsync,
      permission.role.assignDataPermissionsAsync,
      permission.role.assignUsersAsync,
    ],
    resource,
  );

  /**需要重写的Column */
  const [defaultColumns] = useState<ProColumns<API.RoleListItem>[]>([
    {
      // 数据访问范围
      title: '数据访问范围',
      dataIndex: 'dataScope',
      width: 120,
      hideInSearch: true,
      sorter: true,
      valueEnum: {
        0: {
          text: '全部',
          status: 'Default',
        },
        1: {
          text: '本人',
          status: 'Success',
        },
        2: {
          text: '本部门',
          status: 'Warning',
        },
        3: {
          text: '本部门及下属',
          status: 'Error',
        },
        4: {
          text: '自定义',
          status: 'Processing',
        },
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
        if (canAccessible(permission.role.statusChangeAsync, resource)) {
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
      width: 95,
      render(_, entity) {
        const moreItems: ItemType[] = [];
        if (canAccessible(permission.role.assignResourcesAsync, resource)) {
          moreItems.push({
            key: 'resource',
            icon: <ShareAltOutlined />,
            label: '分配资源',
          });
        }
        if (canAccessible(permission.role.assignResourcesAsync, resource)) {
          moreItems.push({
            key: 'permission',
            icon: <SafetyOutlined />,
            label: '分配权限',
          });
        }
        if (canAccessible(permission.role.assignUsersAsync, resource)) {
          moreItems.push({
            key: 'user',
            icon: <UserAddOutlined />,
            label: '分配用户',
          });
        }
        return [
          <Access key={'edit'} accessible={canAccessible(permission.role.putAsync, resource)}>
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
                  if (key === 'permission') {
                    setCurrentRow(entity as any);
                    handlePermissionModalOpen(true);
                    return;
                  }
                  if (key === 'user') {
                    const data = await queryDetail(detail, entity.id!);
                    if (data) {
                      setCurrentRow(data);
                      handleUserModalOpen(true);
                    }
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
      <ProTablePlus<API.RoleListItem, API.RolePagingParams>
        actionRef={actionRef}
        defaultColumns={defaultColumns}
        query={query}
        moduleName={'Role'}
        toolBarRender={() => [
          <Access key={'add'} accessible={canAccessible(permission.role.postAsync, resource)}>
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
          roleResources={currentRow!.resources}
          roleId={currentRow!.id!}
        />
      )}
      {userModalOpen && (
        <UserModal
          title={`${currentRow?.name} - 分配用户`}
          onCancel={() => {
            handleUserModalOpen(false);
          }}
          onOk={async (keys: string[]) => {
            const succeed = await submitHandle(assignUsers, { id: currentRow!.id!, userIds: keys });
            if (succeed) {
              handleUserModalOpen(false);
            }
          }}
          mode={'multiple'}
          open={userModalOpen}
          defaultSelectedKeys={currentRow!.users?.map((p) => p.value) || []}
        />
      )}
      {permissionModalOpen && (
        <DataPermissionForm
          title={`${currentRow?.name} - 分配权限`}
          onCancel={() => {
            handlePermissionModalOpen(false);
          }}
          onSuccess={() => {
            handlePermissionModalOpen(false);
          }}
          open={permissionModalOpen}
          roleId={currentRow!.id!}
        />
      )}
    </PageContainer>
  );
};

export default TableList;
