import { query, detail, remove } from './service';
import { PlusOutlined, FormOutlined, DeleteOutlined, ScissorOutlined } from '@ant-design/icons';
import {
  ActionType,
  ProCard,
  ProColumns,
  ProDescriptionsItemProps,
} from '@ant-design/pro-components';
import { PageContainer, ProDescriptions, ProTable } from '@ant-design/pro-components';
import { FormattedMessage, useModel, useLocation, Access } from '@umijs/max';
import { Button, Drawer, message, Modal, Space } from 'antd';
import React, { useRef, useState } from 'react';
import permission from '@/utils/permission';
import { canAccessible, hasPermission } from '@/utils/utils';
import CreateOrUpdateForm from './components/CreateOrUpdateForm';
import ExternalPageTree, { ExternalPageTreeInstance } from './components/ExternalPageTree';
import FixIcon from '@/components/FixIcon';

const TableList: React.FC = () => {
  const { initialState } = useModel('@@initialState');
  const isRoot = initialState?.currentUser?.userName === 'root';

  const [createOrUpdateModalOpen, handleCreateOrUpdateModalOpen] = useState<boolean>(false);
  const [showDetail, setShowDetail] = useState<boolean>(false);

  const actionRef = useRef<ActionType>();
  const externalPageActionRef = useRef<ExternalPageTreeInstance>();
  const [currentRow, setCurrentRow] = useState<API.ExternalPageListItem>();
  const { getResource } = useModel('resource');
  const location = useLocation();
  const resource = getResource(location.pathname);
  const showOption: boolean = hasPermission(
    [
      permission.externalPage.postAsync,
      permission.externalPage.putAsync
    ],
    resource,
  );

  const columns: ProColumns<API.ExternalPageListItem>[] = [
    {
      title: '名称',
      dataIndex: 'name',
      hideInSearch: true,
      ellipsis: true,
      width: 130,
    },
    {
      title: '地址',
      dataIndex: 'path',
      hideInSearch: true,
      ellipsis: true,
    },
    {
      title: '跳转方式',
      dataIndex: 'type',
      hideInSearch: true,
      width: 100,
      valueEnum: {
        1: {
          text: '外部链接',
          status: 'Error',
        },
        2: {
          text: '内部链接',
          status: 'Default',
        },
      },
    },
    {
      title: '布局',
      dataIndex: 'layout',
      hideInSearch: true,
      tooltip: '访问该页面时使用的布局',
      ellipsis: true,
      width: 80,
    },
    {
      title: '图标',
      dataIndex: 'icon',
      hideInSearch: true,
      ellipsis: true,
      width: 70,
      align: 'center',
      render(_, entity) {
        // @ts-ignore
        return <FixIcon key={'icon'} name={entity.icon} />;
      },
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
      hideInTable: true,
    },
    {
      title: '操作',
      dataIndex: 'option',
      valueType: 'option',
      width: 130,
      hideInTable: !showOption,
      render(_, entity) {
        return [
          <Access
            key={'edit'}
            accessible={canAccessible(permission.externalPage.putAsync, resource)}
          >
            <Button
              shape="circle"
              type={'link'}
              icon={<FormOutlined />}
              disabled={entity.isPublic && !isRoot}
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
              }}
            >
              编辑
            </Button>
          </Access>,
          <Access
            key={'delete'}
            accessible={canAccessible(permission.externalPage.deleteAsync, resource)}
          >
            <Button
              shape="circle"
              type={'link'}
              icon={<DeleteOutlined />}
              disabled={entity.isPublic && !isRoot}
              onClick={() => {
                Modal.confirm({
                  title: '操作提示',
                  content: `确定删除{${entity.name}}吗？`,
                  onOk: async () => {
                    const hide = message.loading(`正在删除`, 0);
                    const res = await remove(entity.id!);
                    hide();
                    if (res.success) {
                      actionRef.current?.reload();
                      externalPageActionRef.current?.reload();
                      return;
                    }
                    message.error(res.message);
                  },
                });
              }}
            >
              删除
            </Button>
          </Access>,
        ];
      },
    },
  ];
  const [parentId, setParentId] = useState<string | null>(null);

  return (
    <PageContainer
      header={{
        title: resource?.name,
        children: resource?.description,
      }}
    >
      <ProCard split="vertical">
        <ProCard colSpan="270px">
          <ExternalPageTree
            ref={externalPageActionRef}
            onSelect={(key) => {
              setParentId(key);
            }}
          />
        </ProCard>
        <ProCard>
          <ProTable<API.ExternalPageListItem, API.ExternalPagePagingParams>
            headerTitle={'查询表格'}
            actionRef={actionRef}
            rowKey="id"
            search={false}
            options={{
              search: {
                placeholder: '请输入名称',
              },
            }}
            toolBarRender={() => [
              <Access
                key={'add'}
                accessible={canAccessible(permission.externalPage.postAsync, resource)}
              >
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
              parentId: parentId,
            }}
            request={async (params: API.ExternalPagePagingParams) => {
              const res = await query(params);
              return {
                data: res.data?.items || [],
                // success: res.success,
                total: res.data?.totalPages || 0,
              };
            }}
            columns={columns}
            tableAlertRender={({ selectedRowKeys }) => {
              return (
                <>
                  <Space size={24}>
                    <span>已选择{selectedRowKeys.length}条记录</span>
                    <Button
                      icon={<ScissorOutlined />}
                      type={'link'}
                      onClick={() => {
                        console.log(selectedRowKeys);
                      }}
                    >
                      批量移动
                    </Button>
                  </Space>
                </>
              );
            }}
            rowSelection={{
              getCheckboxProps(record) {
                return {
                  disabled: record.parentId === null,
                };
              },
            }}
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
          if (externalPageActionRef.current) {
            externalPageActionRef.current.reload();
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
          <ProDescriptions<API.ExternalPageListItem>
            column={2}
            title={currentRow?.name}
            request={async () => ({
              data: currentRow || {},
            })}
            params={{
              id: currentRow?.name,
            }}
            columns={columns as ProDescriptionsItemProps<API.ExternalPageListItem>[]}
          />
        )}
      </Drawer>
    </PageContainer>
  );
};

export default TableList;
