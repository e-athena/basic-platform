import { query, detail, statusChange } from './service';
import { PlusOutlined, FormOutlined, SafetyOutlined, MoreOutlined } from '@ant-design/icons';
import type { ActionType, ProColumns } from '@ant-design/pro-components';
import { PageContainer } from '@ant-design/pro-components';
import { FormattedMessage, useModel, useLocation, Access } from '@umijs/max';
import { Button, Dropdown, message, Modal, Switch } from 'antd';
import React, { useRef, useState } from 'react';
import IconStatus from '@/components/IconStatus';
import permission from '@/utils/permission';
import { canAccessible, hasPermission } from '@/utils/utils';
import CreateOrUpdateForm from './components/CreateOrUpdateForm';
import ProTablePlus from '@/components/ProTablePlus';
import { ItemType } from 'antd/lib/menu/hooks/useItems';
import AuthorizationForm from './components/AuthorizationForm';

const TableList: React.FC = () => {
  const [createOrUpdateModalOpen, handleCreateOrUpdateModalOpen] = useState<boolean>(false);
  const [authorizationModalOpen, handleAuthorizationModalOpen] = useState<boolean>(false);

  const actionRef = useRef<ActionType>();
  const [currentRow, setCurrentRow] = useState<API.RoleDetailItem>();
  const { getResource } = useModel('resource');
  const location = useLocation();
  const resource = getResource(location.pathname);
  const hideInTable: boolean = !hasPermission([permission.role.putAsync], resource);
  const showMoreOption: boolean = hasPermission(
    [permission.role.assignResourcesAsync],
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
      }
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
      hideInTable: hideInTable,
      width: 95,
      render(_, entity) {
        const moreItems: ItemType[] = [];
        if (canAccessible(permission.role.assignResourcesAsync, resource)) {
          moreItems.push({
            key: 'auth',
            icon: <SafetyOutlined />,
            label: '资源授权',
          });
        }
        return [
          <Access key={'edit'} accessible={canAccessible(permission.role.putAsync, resource)}>
            <Button
              shape="circle"
              type={'link'}
              icon={<FormOutlined />}
              onClick={async () => {
                const hide = message.loading('正在查询', 0);
                const res = await detail(entity.id);
                hide();
                if (res.success) {
                  setCurrentRow(res.data);
                  handleCreateOrUpdateModalOpen(true);
                  return;
                }
                message.error(res.message);
              }}
            >
              编辑
            </Button>
          </Access>,
          <Access key={'more'} accessible={showMoreOption}>
            <Dropdown
              menu={{
                items: moreItems,
                onClick: async ({ key }) => {
                  if (key === 'auth') {
                    const hide = message.loading('正在查询', 0);
                    const res = await detail(entity.id);
                    hide();
                    if (res.success) {
                      setCurrentRow(res.data);
                      handleAuthorizationModalOpen(true);
                      return;
                    }
                    message.error(res.message);
                  }
                },
              }}
              placement="bottom"
            >
              <Button shape="circle" type={'link'} icon={<MoreOutlined />} />
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
      {authorizationModalOpen && (<AuthorizationForm
        onCancel={() => {
          handleAuthorizationModalOpen(false);
        }}
        onSuccess={() => {
          handleAuthorizationModalOpen(false);
        }}
        open={authorizationModalOpen}
        roleResources={currentRow!.resources}
        roleId={currentRow!.id!}
      />)}
    </PageContainer>
  );
};

export default TableList;
