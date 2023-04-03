import { query } from './service';
import type { ActionType, ProColumns } from '@ant-design/pro-components';
import { PageContainer, ProTable } from '@ant-design/pro-components';
import { useModel, useLocation } from '@umijs/max';
import React, { useRef } from 'react';
import { getSorter } from '@/utils/utils';

const TableList: React.FC = () => {
  const actionRef = useRef<ActionType>();
  const { getResource } = useModel('resource');
  const location = useLocation();
  const resource = getResource(location.pathname);

  const columns: ProColumns<API.UserAccessRecordListItem>[] = [
    {
      title: '姓名',
      dataIndex: 'userRealName',
      hideInSearch: true,
      width: 120,
    },
    {
      title: '地址(路由)',
      dataIndex: 'accessUrl',
      hideInSearch: true,
      ellipsis: true,
    },
    {
      title: 'IP地址',
      dataIndex: 'accessIp',
      width: 140,
      hideInSearch: true,
    },
    {
      title: '物理地址',
      dataIndex: 'accessPhysicalAddress',
      width: 180,
      hideInSearch: true,
      ellipsis: true,
    },
    {
      title: '访问时间',
      dataIndex: 'accessTime',
      width: 175,
      hideInSearch: true,
      valueType: 'dateTime',
      sorter: true,
    },
    {
      title: '浏览器',
      dataIndex: 'browser',
      width: 120,
      hideInSearch: true,
      // align: 'center',
      ellipsis: true,
    },
    {
      title: '操作系统',
      dataIndex: 'os',
      hideInSearch: true,
      // align: 'center',
      width: 140,
      ellipsis: true,
    },
    {
      title: '设备',
      dataIndex: 'device',
      hideInSearch: true,
      // align: 'center',
      width: 90,
      ellipsis: true,
    },
  ];

  return (
    <PageContainer
      header={{
        title: resource?.name,
        children: resource?.description,
      }}
    >
      <ProTable<API.UserAccessRecordListItem>
        headerTitle={'查询表格'}
        actionRef={actionRef}
        rowKey="id"
        search={false}
        options={{
          search: {
            placeholder: '关健字搜索',
          },
          setting: false,
          fullScreen: true,
        }}
        request={async (params, sorter) => {
          const res = await query({ ...params, ...getSorter(sorter, 'a') });
          return {
            data: res.data?.items || [],
            success: res.success,
            total: res.data?.totalItems || 0,
          };
        }}
        columns={columns}
      />
    </PageContainer>
  );
};

export default TableList;
