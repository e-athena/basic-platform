import { query, detail, remove } from './service';
import { DeleteOutlined, FormOutlined, PlusOutlined, SearchOutlined } from '@ant-design/icons';
import type { ActionType, ProColumns } from '@ant-design/pro-components';
import { PageContainer } from '@ant-design/pro-components';
import { useModel, useLocation, Access } from '@umijs/max';
import { Button, Popconfirm } from 'antd';
import React, { useRef, useState } from 'react';
import permission from '@/utils/permission';
import { canAccessible, hasPermission, queryDetail, submitHandle } from '@/utils/utils';
import ViewDrawer from './components/ViewDrawer';
import CreateOrUpdateForm from './components/CreateOrUpdateForm';
import ProTablePlus from '@/components/ProTablePlus';

const TableList: React.FC = () => {
  const [createOrUpdateModalOpen, handleCreateOrUpdateModalOpen] = useState<boolean>(false);
  const [viewModalOpen, handleViewModalOpen] = useState<boolean>(false);

  const actionRef = useRef<ActionType>();
  const [currentRow, setCurrentRow] = useState<API.EventTrackingConfigDetailItem>();
  const { getResource } = useModel('resource');
  const location = useLocation();
  const resource = getResource(location.pathname);
  const showOption: boolean = hasPermission([
    permission.eventTrackingConfig.saveAsync,
    permission.eventTrackingConfig.getAsync,
    permission.eventTrackingConfig.deleteAsync,
  ], resource);

  /**需要重写的Column */
  const [defaultColumns] = useState<ProColumns<API.EventTrackingConfigListItem>[]>([
    {
      title: '事件名称',
      dataIndex: 'eventName',
      width: 180,
      ellipsis: true,
    },
    {
      title: '事件类型名称',
      dataIndex: 'eventTypeFullName',
      ellipsis: true,
    },
    {
      title: '创建时间',
      dataIndex: 'createdOn',
      ellipsis: true,
      width: 175,
      valueType: 'dateTime',
    },
    {
      title: '操作',
      dataIndex: 'option',
      valueType: 'option',
      hideInTable: !showOption,
      width: 115,
      render(_, entity) {
        return [
          <Access
            key={'edit'}
            accessible={canAccessible(permission.eventTrackingConfig.saveAsync, resource)}
          >
            <Button
              size="small"
              icon={<FormOutlined />}
              onClick={async (e) => {
                e.stopPropagation();
                const data = await queryDetail(detail, entity.id!);
                if (data) {
                  setCurrentRow(data);
                  handleCreateOrUpdateModalOpen(true);
                }
              }}
            />
          </Access>,
          <Access
            key={'view'}
            accessible={canAccessible(permission.eventTrackingConfig.getAsync, resource)}
          >
            <Button
              size="small"
              icon={<SearchOutlined />}
              onClick={async (e) => {
                e.stopPropagation();
                const data = await queryDetail(detail, entity.id!);
                if (data) {
                  setCurrentRow(data);
                  handleViewModalOpen(true);
                }
              }}
            />
          </Access>,
          <Access
            key={'view'}
            accessible={canAccessible(permission.eventTrackingConfig.deleteAsync, resource)}
          >
            <Popconfirm
              placement="topRight"
              title="删除确认"
              description="删除后不可恢复，确定要删除吗?"
              onConfirm={async (e) => {
                e?.stopPropagation();
                const succeed = await submitHandle(remove, entity.id!, '删除');
                if (succeed) {
                  actionRef.current?.reload();
                }
              }}
              okText="确定"
              cancelText="取消"
            >
              <Button
                size="small"
                danger
                icon={<DeleteOutlined />}
              />
            </Popconfirm>
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
      <ProTablePlus<API.EventTrackingConfigListItem, API.EventTrackingConfigPagingParams>
        actionRef={actionRef}
        defaultColumns={defaultColumns}
        query={query}
        moduleName={'EventTrackingConfig'}
        onlyUseLocalColumns
        toolBarRender={() => [
          <Access
            key={'add'}
            accessible={canAccessible(permission.eventTrackingConfig.saveAsync, resource)}
          >
            <Button
              type="primary"
              onClick={() => {
                setCurrentRow(undefined);
                handleCreateOrUpdateModalOpen(true);
              }}
              icon={<PlusOutlined />}
            >
              创建
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
          actionRef.current?.reload();
        }}
        open={createOrUpdateModalOpen}
        values={currentRow}
      />
      {currentRow && (
        <ViewDrawer
          onClose={() => {
            handleViewModalOpen(false);
            setCurrentRow(undefined);
          }}
          open={viewModalOpen}
          values={currentRow}
        />
      )}
    </PageContainer>
  );
};

export default TableList;
