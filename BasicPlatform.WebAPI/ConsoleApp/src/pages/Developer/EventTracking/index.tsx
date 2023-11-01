import { query, queryDecompositionTreeGraph } from './service';
import { ApartmentOutlined, SearchOutlined } from '@ant-design/icons';
import type { ActionType, ProColumns } from '@ant-design/pro-components';
import { PageContainer } from '@ant-design/pro-components';
import { useModel, useLocation, Access, useMatch, Outlet, history } from '@umijs/max';
import { Button } from 'antd';
import React, { useRef, useState } from 'react';
import permission from '@/utils/permission';
import { canAccessible, hasMenuPermission, hasPermission, queryDetail } from '@/utils/utils';
import ViewDrawer from './components/ViewDrawer';
import ProTablePlus from '@/components/ProTablePlus';
import { G6TreeGraphData } from '@ant-design/graphs';
import IconStatus from '@/components/IconStatus';

const TableList: React.FC = () => {
  const [viewModalOpen, handleViewModalOpen] = useState<boolean>(false);

  const actionRef = useRef<ActionType>();
  const [currentRow, setCurrentRow] = useState<G6TreeGraphData>();
  const { initialState } = useModel('@@initialState');
  const { getResource } = useModel('resource');
  const location = useLocation();
  const resource = getResource(location.pathname);
  const showOption: boolean = hasPermission([permission.eventTracking.getAsync], resource);

  const childPath = '/developer/event-tracking/config';
  /**需要重写的Column */
  const defaultColumns: ProColumns<API.EventTrackingListItem>[] = ([
    {
      title: '事件名称',
      dataIndex: 'eventName',
      width: 180,
      ellipsis: true,
    },
    // {
    //   title: '追踪ID',
    //   dataIndex: 'traceId',
    //   width: 280,
    //   ellipsis: true,
    // },
    {
      title: '事件类型名称',
      dataIndex: 'eventTypeFullName',
      ellipsis: true,
    },
    {
      title: '状态',
      dataIndex: 'hasError',
      width: 85,
      align: 'center',
      render: (_, entity) => { 
        return <IconStatus status={!entity.hasError} />;
      }
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
      width: 95,
      render(_, entity) {
        return [
          <Access
            key={'edit'}
            accessible={canAccessible(permission.eventTracking.getAsync, resource)}
          >
            <Button
              size="small"
              icon={<SearchOutlined />}
              onClick={async (e) => {
                e.stopPropagation();
                const data = await queryDetail(queryDecompositionTreeGraph, entity.traceId);
                if (data) {
                  setCurrentRow(data);
                  handleViewModalOpen(true);
                }
              }}
            >
              查看
            </Button>
          </Access>,
        ];
      },
    },
  ]);
  const match = useMatch({ path: childPath });
  if (match) {
    return <Outlet />;
  }

  return (
    <PageContainer
      header={{
        title: resource?.name,
        children: resource?.description,
      }}
    >
      <ProTablePlus<API.EventTrackingListItem, API.EventTrackingPagingParams>
        actionRef={actionRef}
        defaultColumns={defaultColumns}
        query={query}
        moduleName={'EventTracking'}
        onlyUseLocalColumns
        toolBarRender={() => [
          <Access
            key={'config'}
            accessible={hasMenuPermission(childPath, initialState?.apiResources)}
          >
            <Button
              type="primary"
              onClick={() => {
                history.push(childPath);
              }}
              icon={<ApartmentOutlined />}
            >
              配置
            </Button>
          </Access>,
        ]}
      />
      {currentRow && (
        <ViewDrawer
          onClose={() => {
            handleViewModalOpen(false);
            setCurrentRow(undefined);
          }}
          open={viewModalOpen}
          dataSource={currentRow}
        />
      )}
    </PageContainer>
  );
};

export default TableList;
