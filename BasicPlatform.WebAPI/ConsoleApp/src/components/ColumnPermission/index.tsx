import { ProColumns, ProTable } from '@ant-design/pro-components';
import { Button, Segmented } from 'antd';
import React, { useEffect, useState } from 'react';
import ColumnConfig, { ColumnProperty } from './components/ColumnConfig';

export type ColumnConfigGroup = {
  displayName: string;
  items: ColumnConfigType[]
}
export type ColumnConfigType = {
  displayName: string;
  groupKey: string;
  items: ColumnProperty[]
}

type ColumnPermissionProps = {
  data: ColumnConfigGroup[];
  onChange: (data: ColumnConfigGroup[]) => void;
  title: string;
};

const ColumnPermission: React.FC<ColumnPermissionProps> = (props) => {
  const [dataSources, setDataSources] = useState<ColumnConfigGroup[]>([]);
  const [segmentedOptions, setSegmentedOptioins] = useState<string[]>([]);
  const [currentSegmented, setCurrentSegmented] = useState<string>('');
  const [configOpen, setConfigOpen] = useState<boolean>(false);
  const [currentRow, setCurrentRow] = useState<ColumnConfigType>();
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    if (loading) {
      setLoading(false);
      if (props.data.length > 0) {
        setDataSources(props.data);
        setSegmentedOptioins(props.data.map((item) => item.displayName!));
        setCurrentSegmented(props.data[0].displayName!);
      }
    }
  }, [loading, props.data]);

  useEffect(() => {
    if (dataSources.length > 0) {
      props.onChange(dataSources);
    }
  }, [dataSources]);

  const columns: ProColumns<ColumnConfigType>[] = [
    {
      title: '序号',
      dataIndex: 'index',
      fixed: 'left',
      valueType: 'indexBorder',
      width: 75,
      align: 'center',
    },
    {
      title: '模块名称',
      dataIndex: 'displayName',
      hideInSearch: true,
      ellipsis: true,
      // width: 180,
    },
    {
      title: '授权列数',
      dataIndex: 'groupKey',
      hideInSearch: true,
      align: 'center',
      width: 90,
      render(_, entity) {
        return `${entity.items.filter(p => p.enabled).length}/${entity.items.length}`;
      }
    },
    {
      title: '脱敏列数',
      dataIndex: 'groupKey',
      hideInSearch: true,
      align: 'center',
      width: 90,
      render(_, entity) {
        // return entity.items.filter(p => p.isEnableDataMask).length;
        return `${entity.items.filter(p => p.isEnableDataMask).length}/${entity.items.length}`;
      }
    },
    {
      title: '操作',
      dataIndex: 'displayName',
      hideInSearch: true,
      ellipsis: true,
      width: 95,
      align: 'center',
      render(_, entity) {
        const defaultDom = (
          <Button
            size={'small'}
            type={'dashed'}
            onClick={() => {
              setCurrentRow(entity);
              setConfigOpen(true);
            }}
          >
            配置
          </Button>
        );
        return defaultDom;
        // if (entity.items.filter(p => p.isEnableDataMask).length === 0) {
        //   return defaultDom;
        // }
        // return <Badge count={entity.items.filter(p => p.isEnableDataMask).length}>{defaultDom}</Badge>;
      },
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
      <ProTable<ColumnConfigType>
        loading={loading}
        headerTitle={'权限列表'}
        rowKey="resourceKey"
        search={false}
        toolBarRender={false}
        request={async (params) => {
          if (dataSources.length > 0 && currentSegmented !== '') {
            const items =
              dataSources.find((item) => item.displayName === params.group)?.items || [];
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
      {configOpen && currentRow && (
        <ColumnConfig
          // title={`${currentRow?.displayName} - 数据列权限配置`}
          title={`${currentRow?.displayName} - ${props.title}`}
          onCancel={() => {
            setCurrentRow(undefined);
            setConfigOpen(false);
          }}
          onOk={(rows: ColumnProperty[]) => {
            // 更新到dataSources
            const index = dataSources.findIndex((item) => item.displayName === currentSegmented);
            const group = dataSources[index];
            const itemIndex = group.items.findIndex((item) => item.groupKey === currentRow?.groupKey);
            group.items[itemIndex].items = rows;
            setDataSources([...dataSources]);
            setCurrentRow(undefined);
            setConfigOpen(false);
          }}
          open={configOpen}
          data={currentRow}
        />
      )}
    </>
  );
};

export default ColumnPermission;
