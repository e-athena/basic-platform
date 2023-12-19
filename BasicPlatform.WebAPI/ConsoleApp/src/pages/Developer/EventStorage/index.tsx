import { detail, query } from './service';
import { SearchOutlined } from '@ant-design/icons';
import type { ActionType, ProColumns } from '@ant-design/pro-components';
import { PageContainer } from '@ant-design/pro-components';
import { useModel, useLocation, Access } from '@umijs/max';
import { Button } from 'antd';
import React, { useRef, useState } from 'react';
import permission from '@/utils/permission';
import { canAccessible, hasPermission } from '@/utils/utils';
import ViewModal from './components/ViewModal';
import ProTablePlus from '@/components/ProTablePlus';

const TableList: React.FC = () => {
  const [viewModalOpen, handleViewModalOpen] = useState<boolean>(false);

  const actionRef = useRef<ActionType>();
  const [currentRow, setCurrentRow] = useState<API.EventStorageListItem>();
  const { getResource } = useModel('resource');
  const location = useLocation();
  const resource = getResource(location.pathname);
  const showOption: boolean = hasPermission([permission.eventStorage.getAsync], resource);

  /**需要重写的Column */
  const defaultColumns: ProColumns<API.EventStorageListItem>[] = ([
    {
      title: '事件名称',
      dataIndex: 'eventName',
      // width: 180,
      ellipsis: true,
    },
    {
      title: '版本号',
      dataIndex: 'version',
      width: 110,
      sorter: true,
    },
    {
      title: '业务实体名称',
      dataIndex: 'aggregateRootTypeName',
      ellipsis: true,
    },
    // 操作人
    {
      title: '操作人',
      dataIndex: 'userId',
      ellipsis: true,
      width: 120,
    },
    {
      title: '存储时间',
      dataIndex: 'createdOn',
      ellipsis: true,
      width: 175,
      sorter: true,
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
            accessible={canAccessible(permission.eventStorage.getAsync, resource)}
          >
            <Button
              size="small"
              icon={<SearchOutlined />}
              onClick={async (e) => {
                e.stopPropagation();
                console.log(entity);
                const rsp = await detail(entity.sequence);
                if (rsp.success) {
                  entity.events = rsp.data;
                  setCurrentRow(entity);
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

  return (
    <PageContainer
      header={{
        title: resource?.name,
        children: resource?.description,
      }}
    >
      <ProTablePlus<API.EventStorageListItem, API.EventStoragePagingParams>
        actionRef={actionRef}
        defaultColumns={defaultColumns}
        query={query}
        requestBeforeTransform={async (params) => {
          if (!params?.keyword) {
            return Promise.reject('请输入业务ID或用户ID');
          }
          params.id = params.keyword;
          return Promise.resolve(params);
        }}
        searchPlaceholder='请输入业务ID或用户ID'
        moduleName={'EventStorage'}
        onlyUseLocalColumns
      />
      {currentRow && (
        <ViewModal
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
