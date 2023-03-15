import { query, queryColumns, detail, queryResourceCodeInfo, statusChange } from './service';
import { PlusOutlined, FormOutlined, SafetyOutlined, SettingOutlined, SearchOutlined } from '@ant-design/icons';
import { ActionType, ProCard, ProColumns, ProDescriptionsItemProps } from '@ant-design/pro-components';
import {
  PageContainer,
  ProDescriptions,
  ProTable,
} from '@ant-design/pro-components';
import { FormattedMessage, useModel, useLocation, Access } from '@umijs/max';
import { Button, Drawer, message, Modal, Switch, Tooltip } from 'antd';
import React, { useEffect, useRef, useState } from 'react';
// import IconStatus from '@/components/IconStatus';
import permission from '@/utils/permission';
import { canAccessible, getSorter, getFilter, hasPermission } from '@/utils/utils';
import CreateOrUpdateForm from './components/CreateOrUpdateForm';
import AuthorizationForm from './components/AuthorizationForm';
import OrganizationTree from '@/components/OrganizationTree';
import EditTableColumnForm from '@/components/EditTableColumnForm';
import AdvancedSearch from '@/components/AdvancedSearch';
import { useSize } from 'ahooks';
import { FilterGroupItem } from '@/components/AdvancedSearch/components/RulerItem';

const TableList: React.FC = () => {
  const [createOrUpdateModalOpen, handleCreateOrUpdateModalOpen] = useState<boolean>(false);
  const [authorizationModalOpen, handleAuthorizationModalOpen] = useState<boolean>(false);
  const [editTableColumnModalOpen, handleEditTableColumnModalOpen] = useState<boolean>(false);
  const [advancedSearchModalOpen, handleAdvancedSearchModalOpen] = useState<boolean>(false);
  const [showDetail, setShowDetail] = useState<boolean>(false);

  const tableRef = useRef(null);
  const tableSize = useSize(tableRef);

  const actionRef = useRef<ActionType>();
  const [currentRow, setCurrentRow] = useState<API.UserDetailInfo>();
  const [currentResourceCodeRow, setCurrentResourceCodeRow] = useState<API.UserResourceCodeInfo>({
    roleResources: [],
    userResources: []
  });
  const { getResource } = useModel('resource');
  const location = useLocation();
  const resource = getResource(location.pathname);
  const hideInTable: boolean = !hasPermission([
    permission.user.putAsync,
    permission.user.assignResourcesAsync
  ], resource);

  const [columnData, setColumnData] = useState<API.TableColumnItem[]>([]);
  const [columnLoading, setColumnLoading] = useState<boolean>(true);
  const [tableWidth, setTableWidth] = useState<number | undefined>();
  const [columns, setColumns] = useState<ProColumns<API.UserListItem>[]>([
    {
      title: '头像',
      dataIndex: 'avatar',
      hideInSearch: true,
      width: 50,
      fixed: 'left',
      align: 'center',
      valueType: 'avatar'
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
      }
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
          return <Switch
            checkedChildren="启用"
            unCheckedChildren="禁用"
            checked={entity.status === 1}
            onClick={async () => {
              const statusName = entity.status === 1 ? '禁用' : '启用';
              Modal.confirm({
                title: '操作提示',
                content: `确定${statusName}{${entity.realName}}吗？`,
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
        return dom; // <IconStatus status={entity.status === 1} />;
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
      sorter: true,
      hideInTable: true,
    },
    {
      title: '操作',
      dataIndex: 'option',
      valueType: 'option',
      hideInTable: hideInTable,
      width: 130,
      render(_, entity) {
        return [
          <Access key={'edit'} accessible={canAccessible(permission.user.putAsync, resource)}>
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
          </Access>,
          <Access key={'auth'} accessible={canAccessible(permission.user.assignResourcesAsync, resource)}>
            <Button
              shape="circle"
              type={'link'}
              icon={<SafetyOutlined />}
              onClick={async () => {
                const hide = message.loading('正在查询');
                const res = await queryResourceCodeInfo(entity.id);
                hide();
                if (res.success) {
                  setCurrentResourceCodeRow(res.data!)
                  setCurrentRow(entity as API.UserDetailInfo);
                  handleAuthorizationModalOpen(true);
                  return;
                }
                message.error(res.message);
              }}>
              授权
            </Button>
          </Access >,
        ];
      },
    },
  ]);

  useEffect(() => {
    const fetch = async () => {
      setColumnLoading(false);
      let list = columnData;
      if (list.length === 0) {
        const res = await queryColumns();
        list = res.success ? res.data! : [];
        const option = columns.find(x => x.dataIndex === 'option');
        if (option !== undefined && list.find(x => x.dataIndex === 'option') === undefined) {
          list.push({
            title: '操作',
            dataIndex: 'option',
            fixed: 'right',
            width: option.width,
            show: true,
            sort: 999,
            required: true
          } as API.TableColumnItem);
        }
        setColumnData(list);
      }
      const result: ProColumns<API.UserListItem>[] = [];
      // 按sort排序
      list.sort((a, b) => a.sort - b.sort);
      for (let i = 0; i < list.length; i++) {
        const item = list[i];
        const find = columns.find(x => x.dataIndex === item.dataIndex);
        if (find !== undefined) {
          find.hideInTable = !item.show;
          find.width = item.width || find.width;
          find.fixed = (item.fixed !== 'left' && item.fixed !== 'right') ? undefined : item.fixed;
          result.push(find);
          continue;
        }
        result.push({
          ...item,
          hideInTable: !item.show,
          ellipsis: item.ellipsis || true,
        } as ProColumns<API.UserListItem>);
      }
      // 计算宽度
      let width: number = 0;
      for (let i = 0; i < list.length; i++) {
        const item = list[i];
        if (!item.show) {
          continue;
        }
        width += item.width || 200;
      }
      setTableWidth(width);
      setColumns(result);
    }
    if (columnLoading) {
      fetch();
    }
  }, [columnLoading]);

  // let columns: ProColumns<API.UserListItem>[] = [
  //   {
  //     title: '头像',
  //     dataIndex: 'avatar',
  //     hideInSearch: true,
  //     width: 50,
  //     align: 'center',
  //     valueType: 'avatar'
  //   },
  //   {
  //     title: '登录名',
  //     dataIndex: 'userName',
  //     hideInSearch: true,
  //     width: 100,
  //     ellipsis: true,
  //   },
  //   {
  //     title: '姓名',
  //     dataIndex: 'realName',
  //     hideInSearch: true,
  //     ellipsis: true,
  //     width: 100,
  //   },
  //   {
  //     title: '性别',
  //     dataIndex: 'gender',
  //     hideInSearch: true,
  //     ellipsis: true,
  //     width: 80,
  //     align: 'center',
  //     sorter: true,
  //     filters: true,
  //     valueEnum: {
  //       0: {
  //         status: 'Default',
  //         text: '保密',
  //       },
  //       1: {
  //         status: 'Success',
  //         text: '男',
  //       },
  //       2: {
  //         status: 'Error',
  //         text: '女',
  //       },
  //     }
  //   },
  //   {
  //     title: '手机号',
  //     dataIndex: 'phoneNumber',
  //     width: 110,
  //     hideInSearch: true,
  //   },
  //   {
  //     title: '组织',
  //     dataIndex: 'organizationName',
  //     hideInSearch: true,
  //     ellipsis: true,
  //   },
  //   {
  //     title: '职位',
  //     dataIndex: 'positionName',
  //     hideInSearch: true,
  //     ellipsis: true,
  //   },
  //   {
  //     title: '状态',
  //     dataIndex: 'status',
  //     width: 90,
  //     hideInSearch: true,
  //     align: 'center',
  //     sorter: true,
  //     render(_, entity) {
  //       if (canAccessible(permission.user.statusChangeAsync, resource)) {
  //         return <Switch
  //           checkedChildren="启用"
  //           unCheckedChildren="禁用"
  //           checked={entity.status === 1}
  //           onClick={async () => {
  //             const statusName = entity.status === 1 ? '禁用' : '启用';
  //             Modal.confirm({
  //               title: '操作提示',
  //               content: `确定${statusName}{${entity.realName}}吗？`,
  //               onOk: async () => {
  //                 const hide = message.loading(`正在${statusName}`);
  //                 const res = await statusChange(entity.id!);
  //                 hide();
  //                 if (res.success) {
  //                   actionRef.current?.reload();
  //                   return;
  //                 }
  //                 message.error(res.message);
  //               }
  //             });
  //           }}
  //         />
  //       }
  //       return <IconStatus status={entity.status === 1} />;
  //     },
  //   },
  //   {
  //     title: '更新人',
  //     dataIndex: 'updatedUserName',
  //     width: 100,
  //     hideInSearch: true,
  //     hideInTable: true
  //   },
  //   {
  //     title: '更新时间',
  //     dataIndex: 'updatedOn',
  //     width: 170,
  //     hideInSearch: true,
  //     valueType: 'dateTime',
  //     sorter: true,
  //     hideInTable: true
  //   },
  //   {
  //     title: '操作',
  //     dataIndex: 'option',
  //     valueType: 'option',
  //     hideInTable: hideInTable,
  //     render(_, entity) {
  //       return [
  //         <Access key={'edit'} accessible={canAccessible(permission.user.putAsync, resource)}>
  //           <Button
  //             shape="circle"
  //             type={'link'}
  //             icon={<FormOutlined />}
  //             onClick={async () => {
  //               const hide = message.loading('正在查询');
  //               const res = await detail(entity.id);
  //               hide();
  //               if (res.success) {
  //                 setCurrentRow(res.data);
  //                 handleCreateOrUpdateModalOpen(true);
  //                 return;
  //               }
  //               message.error(res.message);
  //             }}>
  //             编辑
  //           </Button>
  //         </Access>,
  //         <Access key={'auth'} accessible={canAccessible(permission.user.assignResourcesAsync, resource)}>
  //           <Button
  //             shape="circle"
  //             type={'link'}
  //             icon={<SafetyOutlined />}
  //             onClick={async () => {
  //               const hide = message.loading('正在查询');
  //               const res = await queryResourceCodeInfo(entity.id);
  //               hide();
  //               if (res.success) {
  //                 setCurrentResourceCodeRow(res.data!)
  //                 setCurrentRow(entity as API.UserDetailInfo);
  //                 handleAuthorizationModalOpen(true);
  //                 return;
  //               }
  //               message.error(res.message);
  //             }}>
  //             授权
  //           </Button>
  //         </Access >,
  //       ];
  //     },
  //   },
  // ];

  const [parentId, setParentId] = useState<string | null>(null);
  const [advancedSearchFilter, setAdvancedSearchFilter] = useState<FilterGroupItem[]>([]);

  return (
    <PageContainer header={{
      title: resource?.name,
      children: resource?.description
    }}>
      <ProCard split="vertical">
        <ProCard colSpan="270px">
          <OrganizationTree
            onSelect={(key) => {
              setParentId(key);
            }} />
        </ProCard>
        <ProCard>
          <div ref={tableRef}>
            <ProTable<API.UserListItem, API.UserPagingParams>
              headerTitle={'查询表格'}
              actionRef={actionRef}
              style={{ maxWidth: tableSize?.width }}
              rowKey="id"
              search={false}
              options={{
                search: {
                  placeholder: '关健字搜索',
                },
                setting: false
              }}
              toolBarRender={() => [
                <Access key={'add'} accessible={canAccessible(permission.user.postAsync, resource)}>
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
                <Tooltip
                  key={'advancedSearch'}
                  title={'自定义查询'}
                >
                  <Button
                    type={'link'}
                    style={{ color: '#1f1f1f' }}
                    onClick={() => {
                      handleAdvancedSearchModalOpen(true);
                    }}
                    icon={<SearchOutlined />}
                  />
                </Tooltip>,
                <Tooltip
                  key={'editTableColumn'}
                  title={'自定义表格'}
                >
                  <Button
                    type={'link'}
                    style={{ color: '#1f1f1f' }}
                    onClick={() => {
                      handleEditTableColumnModalOpen(true);
                    }}
                    icon={<SettingOutlined />}
                  />
                </Tooltip>,
              ]}
              params={{
                organizationId: parentId,
                filterGroups: advancedSearchFilter
              }}
              request={async (params, sorter, filter) => {
                const res = await query({ ...params, ...getSorter(sorter, 'a'), ...getFilter(filter) });
                return {
                  data: res.data?.items || [],
                  success: res.success,
                  total: res.data?.totalItems || 0,
                }
              }}
              scroll={{ x: tableWidth || 1000 }}
              columns={columnLoading ? [] : columns}
            />
          </div>
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
        }}
        open={createOrUpdateModalOpen}
        values={currentRow}
      />
      <AuthorizationForm
        onCancel={() => {
          handleAuthorizationModalOpen(false);
          if (!showDetail) {
            setCurrentRow(undefined);
          }
        }}
        onSuccess={() => {
          handleAuthorizationModalOpen(false);
          if (!showDetail) {
            setCurrentRow(undefined);
          }
        }}
        open={authorizationModalOpen}
        values={currentRow}
        resourceCodeInfo={currentResourceCodeRow}
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
        {currentRow?.userName && (
          <ProDescriptions<API.UserListItem>
            column={2}
            title={currentRow?.userName}
            request={async () => ({
              data: currentRow || {},
            })}
            params={{
              id: currentRow?.id,
            }}
            columns={columns as ProDescriptionsItemProps<API.UserListItem>[]}
          />
        )}
      </Drawer>
      <EditTableColumnForm
        data={columnData}
        open={editTableColumnModalOpen}
        onCancel={() => {
          handleEditTableColumnModalOpen(false);
        }}
        onOk={(d) => {
          setColumnData(d);
          setColumnLoading(true);
          handleEditTableColumnModalOpen(false);
        }} />

      <AdvancedSearch
        data={columnData}
        open={advancedSearchModalOpen}
        historyFilters={advancedSearchFilter}
        onCancel={() => {
          handleAdvancedSearchModalOpen(false);
        }}
        onOk={(d) => {
          console.log(d);
          setAdvancedSearchFilter(d);
          handleAdvancedSearchModalOpen(false);
        }} />
    </PageContainer>
  );
};

export default TableList;
