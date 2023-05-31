import { query, detail, queryResourceCodeInfo, statusChange, resetPassword } from './service';
import {
  PlusOutlined,
  FormOutlined,
  SafetyOutlined,
  MoreOutlined,
  ReloadOutlined,
  ShareAltOutlined,
} from '@ant-design/icons';
import { ActionType, ProCard, ProColumns } from '@ant-design/pro-components';
import { PageContainer } from '@ant-design/pro-components';
import { FormattedMessage, useModel, useLocation, Access } from '@umijs/max';
import { Button, Dropdown, message, Modal, Switch, Typography } from 'antd';
import React, { useRef, useState } from 'react';
// import IconStatus from '@/components/IconStatus';
import permission from '@/utils/permission';
import { canAccessible, hasPermission, queryDetail } from '@/utils/utils';
import CreateOrUpdateForm from './components/CreateOrUpdateForm';
import AuthorizationForm from './components/AuthorizationForm';
import OrganizationTree from '@/components/OrganizationTree';
import { useSize } from 'ahooks';
import ProTablePlus from '@/components/ProTablePlus';
import { ItemType } from 'antd/lib/menu/hooks/useItems';
import DataPermissionForm from './components/DataPermissionForm';
const { Paragraph } = Typography;

const TableList: React.FC = () => {
  const [createOrUpdateModalOpen, handleCreateOrUpdateModalOpen] = useState<boolean>(false);
  const [authorizationModalOpen, handleAuthorizationModalOpen] = useState<boolean>(false);
  const [permissionModalOpen, handlePermissionModalOpen] = useState<boolean>(false);

  // 获取card的宽度，用于计算表格的宽度
  const tableSize = useSize(document.getElementsByClassName('ant-pro-card-body')[0]);

  const actionRef = useRef<ActionType>();
  const [currentRow, setCurrentRow] = useState<API.UserDetailInfo>();
  const [currentResourceCodeRow, setCurrentResourceCodeRow] = useState<API.UserResourceCodeInfo>({
    roleResources: [],
    userResources: [],
  });
  const { getResource } = useModel('resource');
  const location = useLocation();
  const resource = getResource(location.pathname);
  const showOption: boolean = hasPermission(
    [
      permission.user.putAsync,
      permission.user.assignResourcesAsync,
      permission.user.assignDataPermissionsAsync,
      permission.user.resetPasswordAsync,
    ],
    resource,
  );
  const showMoreOption: boolean = hasPermission(
    [
      permission.user.assignResourcesAsync,
      permission.user.assignDataPermissionsAsync,
      permission.user.resetPasswordAsync,
    ],
    resource,
  );

  /**需要重写的Column */
  const [defaultColumns] = useState<ProColumns<API.UserListItem>[]>([
    {
      title: '头像',
      dataIndex: 'avatar',
      hideInSearch: true,
      width: 50,
      fixed: 'left',
      align: 'center',
      valueType: 'avatar',
    },
    {
      title: '性别',
      dataIndex: 'gender',
      hideInSearch: true,
      ellipsis: true,
      width: 80,
      sorter: true,
      filters: true,
      valueEnum: {
        0: {
          status: 'Default',
          text: '保密',
        },
        1: {
          status: 'Success',
          text: '男',
        },
        2: {
          status: 'Error',
          text: '女',
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
      render(dom, entity) {
        if (canAccessible(permission.user.statusChangeAsync, resource)) {
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
                  content: `确定${statusName}{${entity.realName}}吗？`,
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
        return dom;
      },
    },
    {
      title: '更新人',
      dataIndex: 'updatedUserName',
      width: 100,
      hideInSearch: true,
      hideInTable: true,
    },
    {
      title: '更新时间',
      dataIndex: 'updatedOn',
      width: 170,
      hideInSearch: true,
      valueType: 'dateTime',
      sorter: true,
      hideInTable: true,
    },
    {
      title: '操作',
      dataIndex: 'option',
      valueType: 'option',
      hideInTable: !showOption,
      width: 95,
      render(_, entity) {
        const moreItems: ItemType[] = [];
        if (canAccessible(permission.user.assignResourcesAsync, resource)) {
          moreItems.push({
            key: 'auth',
            icon: <ShareAltOutlined />,
            label: '分配资源',
          });
        }
        if (canAccessible(permission.user.assignResourcesAsync, resource)) {
          moreItems.push({
            key: 'permission',
            icon: <SafetyOutlined />,
            label: '分配权限',
          });
        }
        if (canAccessible(permission.user.resetPasswordAsync, resource)) {
          moreItems.push({
            key: 'resetPassword',
            icon: <ReloadOutlined />,
            label: '重置密码',
          });
        }
        return [
          <Access key={'edit'} accessible={canAccessible(permission.user.putAsync, resource)}>
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
                  if (key === 'auth') {
                    const data = await queryDetail(queryResourceCodeInfo, entity.id);
                    if (data) {
                      setCurrentResourceCodeRow(data);
                      setCurrentRow(entity as API.UserDetailInfo);
                      handleAuthorizationModalOpen(true);
                    }
                    return;
                  }
                  if (key === 'permission') {
                    setCurrentRow(entity as any);
                    handlePermissionModalOpen(true);
                    return;
                  }
                  if (key === 'resetPassword') {
                    Modal.confirm({
                      title: '操作提示',
                      content: `确定重置{${entity.realName}}的密码吗？`,
                      onOk: async () => {
                        const hide = message.loading('正在重置', 0);
                        const res = await resetPassword(entity.id!);
                        hide();
                        if (res.success) {
                          Modal.success({
                            title: '重置成功，请复制新密码并保存好。',
                            content: (
                              <div style={{ marginTop: 50, marginLeft: 55 }}>
                                <Paragraph copyable style={{ fontSize: 28 }} type={'danger'}>
                                  {res.data!}
                                </Paragraph>
                              </div>
                            ),
                            okText: '我已复制',
                          });
                          return;
                        }
                        message.error(res.message);
                      },
                    });
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

  const [organizationId, setOrganizationId] = useState<string | null>(null);

  return (
    <PageContainer
      header={{
        title: resource?.name,
        children: resource?.description,
      }}
    >
      <ProCard split="vertical">
        <ProCard colSpan="270px">
          <OrganizationTree
            onSelect={(key) => {
              setOrganizationId(key);
            }}
          />
        </ProCard>
        <ProCard>
          <ProTablePlus<API.UserListItem, API.UserPagingParams>
            actionRef={actionRef}
            style={
              tableSize?.width
                ? {
                    maxWidth: tableSize?.width - 270 - 24,
                  }
                : {}
            }
            defaultColumns={defaultColumns}
            query={query}
            moduleName={'User'}
            params={{
              organizationId: organizationId,
            }}
            toolBarRender={() => [
              <Access key={'add'} accessible={canAccessible(permission.user.postAsync, resource)}>
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
        }}
        open={createOrUpdateModalOpen}
        values={currentRow}
      />
      <AuthorizationForm
        title={`${currentRow?.realName} - 分配资源`}
        onCancel={() => {
          handleAuthorizationModalOpen(false);
        }}
        onSuccess={() => {
          handleAuthorizationModalOpen(false);
        }}
        open={authorizationModalOpen}
        values={currentRow}
        resourceCodeInfo={currentResourceCodeRow}
      />
      {permissionModalOpen && (
        <DataPermissionForm
          title={`${currentRow?.realName} - 分配权限`}
          onCancel={() => {
            handlePermissionModalOpen(false);
          }}
          onSuccess={() => {
            handlePermissionModalOpen(false);
          }}
          open={permissionModalOpen}
          userId={currentRow!.id!}
        />
      )}
    </PageContainer>
  );
};

export default TableList;
