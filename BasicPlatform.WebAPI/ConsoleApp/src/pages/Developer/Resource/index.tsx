import { query, sync } from './service';
import { SyncOutlined } from '@ant-design/icons';
import type { ActionType, ProColumns } from '@ant-design/pro-components';
import { PageContainer, ProTable } from '@ant-design/pro-components';
import { Button, message, Select, Tag, Tooltip } from 'antd';
import React, { useEffect, useRef, useState } from 'react';
import IconStatus from '@/components/IconStatus';
import FixIcon from '@/components/FixIcon';
import { useLocation, useModel, Access } from '@umijs/max';
import permission from '@/utils/permission';
import { canAccessible } from '@/utils/utils';
import { queryAppList } from '@/services/ant-design-pro/api';

const TableList: React.FC = () => {
  const [expandedRowKeys, setExpandedRowKeys] = useState<string[]>([]);

  const actionRef = useRef<ActionType>();
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
      width: 180,
      ellipsis: true,
    },
    {
      title: '代码',
      dataIndex: 'code',
      width: 210,
      hideInSearch: true,
      ellipsis: true,
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
        return <FixIcon name={entity.icon} />;
      },
    },
    {
      title: '功能',
      dataIndex: 'functions',
      hideInSearch: true,
      render(_, entity) {
        return entity.functions?.map((item) => {
          const tagDom = (
            <Tag
              style={{
                marginTop: 5,
              }}
              key={item.value}
            >
              {item.label}
            </Tag>
          );
          if (item.description) {
            return (
              <Tooltip key={item.value} placement={'top'} title={item.description}>
                {tagDom}
              </Tooltip>
            );
          }
          return tagDom;
        });
      },
    },
  ];

  const [apps, setApps] = useState<API.ApplicationListItem[]>([]);
  const [appLoading, setAppLoading] = useState<boolean>(true);
  const [subAppResourceUrl, setSubAppResourceUrl] = useState<string>();

  useEffect(() => {
    const fetch = async () => {
      setAppLoading(false);
      const res = await queryAppList();
      const data = res.success ? res.data || [] : [];
      setApps(data);
    };
    if (appLoading) {
      fetch();
    }
  }, [appLoading]);

  return (
    <PageContainer
      header={{
        title: resource?.name,
        children: resource?.description,
      }}
    >
      <ProTable<API.ResourceInfo>
        headerTitle={'资源列表'}
        actionRef={actionRef}
        rowKey="code"
        search={false}
        options={{
          reload: false,
          fullScreen: true,
        }}
        toolBarRender={() => [
          <Select
            key={'switchApp'}
            showSearch
            placeholder="选择应用切换"
            onChange={(value) => {
              if (value === undefined) {
                setSubAppResourceUrl(undefined);
                return;
              }
              const app = apps.find((p) => p.id === value);
              setSubAppResourceUrl(`${app?.apiUrl}${app?.menuResourceRoute}`);
            }}
            allowClear
            filterOption={(input, option) =>
              (option?.label ?? '').toLowerCase().includes(input.toLowerCase())
            }
            options={apps.map((p) => ({ label: p.name, value: p.id }))}
            style={{ width: 200, marginRight: 10 }}
          />,
          <Access key={'sync'} accessible={canAccessible(permission.resource.syncAsync, resource)}>
            <Button
              type="primary"
              onClick={async () => {
                const hide = message.loading('正在同步', 0);
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
        ]}
        request={async (params) => {
          const res = await query(params.subAppResourceUrl);
          if (res.success) {
            setExpandedRowKeys(res.data!.map((item) => item.code));
          } else {
            message.error('加载资源失败');
          }
          return {
            data: res.data || [],
            success: res.success,
            total: 0,
          };
        }}
        expandable={{
          expandedRowKeys: expandedRowKeys,
        }}
        columns={columns}
        rowSelection={false}
        pagination={false}
        params={{
          subAppResourceUrl,
        }}
      />
    </PageContainer>
  );
};

export default TableList;
