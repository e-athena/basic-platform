import { query, sync, reinitialize } from './service';
import { FormOutlined, SyncOutlined, RedoOutlined } from '@ant-design/icons';
import type { ActionType, ProColumns, ProDescriptionsItemProps } from '@ant-design/pro-components';
import { PageContainer, ProDescriptions, ProTable } from '@ant-design/pro-components';
import { Button, Drawer, message, Tag } from 'antd';
import React, { useRef, useState } from 'react';
import IconStatus from '@/components/IconStatus';
import FixIcon from '@/components/FixIcon';
import { useLocation, useModel, Access } from '@umijs/max';
import permission from '@/utils/permission';
import { canAccessible } from '@/utils/utils';

const TableList: React.FC = () => {
  const [showDetail, setShowDetail] = useState<boolean>(false);
  const [expandedRowKeys, setExpandedRowKeys] = useState<string[]>([]);

  const actionRef = useRef<ActionType>();
  const [currentRow, setCurrentRow] = useState<API.ResourceInfo>();
  const { getResource } = useModel('resource');
  const location = useLocation();
  const resource = getResource(location.pathname);

  const columns: ProColumns<API.ResourceInfo>[] = [
    {
      title: '名称',
      dataIndex: 'name',
      hideInSearch: true,
      width: 150,
    },
    {
      title: '路由',
      dataIndex: 'path',
      hideInSearch: true,
      width: 150,
    },
    {
      title: '代码',
      dataIndex: 'code',
      width: 220,
      hideInSearch: true,
    },
    {
      title: '显示',
      dataIndex: 'success',
      width: 70,
      hideInSearch: true,
      align: 'center',
      render(_, entity) {
        return <IconStatus status={entity.isVisible} />;
      },
    },
    {
      title: '排序',
      dataIndex: 'sort',
      width: 70,
      hideInSearch: true,
      align: 'center',
    },
    {
      title: '图标',
      dataIndex: 'icon',
      width: 70,
      hideInSearch: true,
      align: 'center',
      render(_, entity) {
        // @ts-ignore
        return <FixIcon name={entity.icon} />
      },
    },
    {
      title: '功能',
      dataIndex: 'functions',
      hideInSearch: true,
      render(_, entity) {
        return entity.functions?.map((item) => (<Tag style={{
          marginTop: 5
        }} key={item.value}>{item.label}</Tag>));
      }
    },
    {
      title: '操作',
      dataIndex: 'option',
      valueType: 'option',
      fixed: 'right',
      width: 70,
      render(_, entity) {
        return [
          <Button
            key={'view'}
            shape="circle"
            type={'link'}
            icon={<FormOutlined />}
            onClick={() => {
              setCurrentRow(entity);
              setShowDetail(true);
            }}
          >
            查看
          </Button>,
        ];
      },
    }
  ];

  return (
    <PageContainer header={{
      title: resource?.name,
      children: resource?.description
    }}>
      <ProTable<API.ResourceInfo, API.PageParams>
        headerTitle={'资源列表'}
        actionRef={actionRef}
        rowKey="code"
        search={false}
        toolBarRender={() => [
          <Access key={'sync'} accessible={canAccessible(permission.resource.syncAsync, resource)}>
            <Button
              type="primary"
              onClick={async () => {
                const hide = message.loading('正在同步');
                const res = await sync();
                hide();
                if (res.success) {
                  message.success('同步成功');
                  actionRef.current?.reloadAndRest?.();
                  return;
                }
                message.error('同步失败，请重试');
              }}
              icon={<SyncOutlined />}
            >
              同步资源
            </Button>
          </Access>,
          <Access key={'reset'} accessible={canAccessible(permission.resource.reinitializeAsync, resource)}>
            <Button
              type="primary"
              danger
              onClick={async () => {
                const hide = message.loading('正在重置');
                const res = await reinitialize();
                hide();
                if (res.success) {
                  message.success('重置成功');
                  actionRef.current?.reloadAndRest?.();
                  return;
                }
                message.error('重置失败，请重试');
              }}
              icon={<RedoOutlined />}
            >
              重置资源
            </Button>
          </Access>,
        ]}
        request={async () => {
          const res = await query();
          if (res.success) {
            setExpandedRowKeys(res.data!.map(item => item.code));
          }
          return {
            data: res.data || [],
            success: res.success,
            total: 0,
          }
        }}
        expandable={{
          expandedRowKeys: expandedRowKeys
        }}
        columns={columns}
        rowSelection={false}
        pagination={false}
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
        {currentRow?.code && (
          <ProDescriptions<API.ResourceInfo>
            column={1}
            title={currentRow?.name}
            request={async () => ({
              data: currentRow || {},
            })}
            params={{
              id: currentRow?.id,
            }}
            columns={columns as ProDescriptionsItemProps<API.ResourceInfo>[]}
          />
        )}
      </Drawer>
    </PageContainer>
  );
};

export default TableList;
