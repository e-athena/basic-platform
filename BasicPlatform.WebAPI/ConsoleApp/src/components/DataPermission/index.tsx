import { ProColumns, ProTable } from '@ant-design/pro-components';
import { Badge, Button, Checkbox, Radio, Segmented, Space, Tooltip } from 'antd';
import React, { useEffect, useState } from 'react';
import OrgModal from '@/components/OrgModal';
import QueryPolicyModal from './components/QueryPolicyModal';

type DataPermissionProps = {
  data: API.DataPermissionGroup[];
  onChange: (data: API.DataPermissionGroup[]) => void;
};

const DataPermission: React.FC<DataPermissionProps> = (props) => {
  const [dataSources, setDataSources] = useState<API.DataPermissionGroup[]>([]);
  const [segmentedOptions, setSegmentedOptioins] = useState<string[]>([]);
  const [currentSegmented, setCurrentSegmented] = useState<string>('');
  const [orgModalOpen, setOrgModalOpen] = useState<boolean>(false);
  const [queryPolicyOpen, setQueryPolicyOpen] = useState<boolean>(false);
  const [dataScopeCustoms, setDataScopeCustoms] = useState<string[]>([]);
  const [currentResourceKey, setCurrentResourceKey] = useState<string>('');
  const [currentRow, setCurrentRow] = useState<API.DataPermission>();
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    if (loading && props.data.length > 0) {
      setLoading(false);
      setDataSources(props.data);
      setSegmentedOptioins(props.data.map((item) => item.displayName!));
      setCurrentSegmented(props.data[0].displayName!);
    }
  }, [loading, props.data])

  useEffect(() => {
    props.onChange(dataSources);
  }, [dataSources]);

  const columns: ProColumns<API.DataPermission>[] = [
    {
      title: '启用',
      dataIndex: 'enabled',
      hideInSearch: true,
      width: 70,
      tooltip: '选中后，该模块的数据访问范围才会生效',
      align: 'center',
      render(_, entity) {
        return (
          <Checkbox
            checked={entity.enabled}
            disabled={entity.disableChecked && entity.enabled}
            onChange={(e) => {
              // 更新dataSources的启用状态
              const index = dataSources.findIndex((item) => item.displayName === currentSegmented);
              const items = dataSources[index].items;
              const itemIndex = items.findIndex((item) => item.resourceKey === entity.resourceKey);
              items[itemIndex].enabled = e.target.checked;
              setDataSources([...dataSources]);
            }}
          />
        );
      },
    },
    {
      title: '模块名称',
      dataIndex: 'displayName',
      hideInSearch: true,
      ellipsis: true,
      width: 190,
    },
    {
      title: '数据访问范围',
      dataIndex: 'dataScope',
      hideInSearch: true,
      render(_, entity) {
        const defaultDom = (
          <Radio.Group
            onChange={(e) => {
              // 更新dataSources的访问范围
              const index = dataSources.findIndex((item) => item.displayName === currentSegmented);
              const items = dataSources[index].items;
              const itemIndex = items.findIndex((item) => item.resourceKey === entity.resourceKey);
              items[itemIndex].dataScope = e.target.value;
              setDataSources([...dataSources]);
            }}
            value={entity.dataScope}
          >
            <Radio value={0}>
              <Tooltip title={'不受查询限制，能查询所有的数据。'} placement={'top'}>
                全部
              </Tooltip>
            </Radio>
            <Radio value={1}>
              <Tooltip title={'只能查询本人创建的数据。'} placement={'top'}>
                本人
              </Tooltip>
            </Radio>
            <Radio value={2}>
              <Tooltip
                title={'只能查询本人所在的部门成员创建的数据，离开本部门后将查询不到。'}
                placement={'top'}
              >
                本部门
              </Tooltip>
            </Radio>
            <Radio value={3}>
              <Tooltip
                title={'只能查询本人所在的部门成员及下级部门创建的数据，离开本部门后将查询不到。'}
                placement={'top'}
              >
                本部门及下属部门
              </Tooltip>
            </Radio>
            <Radio value={4}>
              <Tooltip title={'可查询指定组织/部门成员创建的数据。'} placement={'top'}>
                {entity.dataScope === 4 && entity.dataScopeCustoms.length > 0 ? (
                  <Badge count={entity.dataScopeCustoms.length}>自定义</Badge>
                ) : (
                  <span>自定义</span>
                )}
              </Tooltip>
            </Radio>
          </Radio.Group>
        );
        if (entity.dataScope !== 4) {
          return defaultDom;
        }
        return (
          <Space>
            {defaultDom}
            <Button
              size={'small'}
              onClick={() => {
                setCurrentResourceKey(entity.resourceKey);
                setDataScopeCustoms(entity.dataScopeCustoms || []);
                setOrgModalOpen(true);
              }}
            >
              选择组织
            </Button>
          </Space>
        );
      },
    },
    {
      title: '查询策略',
      dataIndex: 'displayName',
      hideInSearch: true,
      ellipsis: true,
      width: 100,
      tooltip: '针对模块列表字段的查询策略',
      align: 'center',
      render(_, entity) {
        const defaultDom = <Button size={'small'} type={'dashed'} onClick={() => {
          setCurrentRow(entity);
          setQueryPolicyOpen(true);
        }}>配置</Button>;
        if (entity.queryFilterGroups.length === 0) {
          return defaultDom;
        }
        return (<Badge count={entity.queryFilterGroups.length}>
          {defaultDom}
        </Badge>)
      }
    },
  ];
  return (
    <>
      {segmentedOptions.length > 1 && (
        <div style={{ padding: '20px 0' }}>
          <Segmented
            block
            options={segmentedOptions}
            value={currentSegmented}
            onChange={(value) => {
              setCurrentSegmented(value.toString());
            }}
          />
        </div>
      )}
      <ProTable<API.DataPermission>
        loading={loading}
        headerTitle={'权限列表'}
        rowKey="resourceKey"
        search={false}
        toolBarRender={false}
        request={async (params) => {
          if (dataSources.length > 0 && currentSegmented !== '') {
            const items = dataSources.find((item) => item.displayName === params.group)?.items || [];
            return {
              data: items,
              success: true,
              total: 0,
            };
          }
          return {
            data: [],
            success: true,
            total: 0,
          };
        }}
        params={{
          group: currentSegmented,
        }}
        columns={columns}
        scroll={{ x: 730, y: 400 }}
        rowSelection={false}
        pagination={false}
      />

      <OrgModal
        mode={'multiple'}
        open={orgModalOpen}
        onCancel={() => {
          setOrgModalOpen(false);
        }}
        onOk={(keys: string[]) => {
          // 更新dataSources的访问范围
          const index = dataSources.findIndex((item) => item.displayName === currentSegmented);
          const items = dataSources[index].items;
          const itemIndex = items.findIndex((item) => item.resourceKey === currentResourceKey);
          items[itemIndex].dataScope = 4;
          items[itemIndex].dataScopeCustoms = keys;
          setDataSources([...dataSources]);
          setOrgModalOpen(false);
        }}
        defaultSelectedKeys={dataScopeCustoms || []}
      />
      {queryPolicyOpen && currentRow && (
        <QueryPolicyModal
          title={`${currentRow?.displayName} - 查询策略配置`}
          onCancel={() => {
            setCurrentRow(undefined);
            setQueryPolicyOpen(false);
          }}
          historyFilters={[...(currentRow?.queryFilterGroups || [])]}
          onOk={(rows: FilterGroupItem[]) => {
            // 将数据更新到queryFilterGroups
            const index = dataSources.findIndex((item) => item.displayName === currentSegmented);
            const items = dataSources[index].items;
            const itemIndex = items.findIndex((item) => item.resourceKey === currentRow?.resourceKey);
            items[itemIndex].queryFilterGroups = rows;
            setDataSources([...dataSources]);
            setCurrentRow(undefined);
            setQueryPolicyOpen(false);
          }}
          open={queryPolicyOpen}
          resourceKey={currentRow?.resourceKey || ''}
          data={currentRow?.properties || []}
        />)}
    </>
  );
};

export default DataPermission;
