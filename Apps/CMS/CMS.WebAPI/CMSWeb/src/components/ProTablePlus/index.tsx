import { queryColumns, updateUserCustomColumns } from '@/services/ant-design-pro/api';
import { getFilter, getSorter } from '@/utils/utils';
import {
  ParamsType,
  ProColumns,
  ProDescriptions,
  ProTable,
  ProTableProps,
} from '@ant-design/pro-components';
import { Drawer, Tooltip } from 'antd';
import { cloneDeep } from 'lodash';
import { useEffect, useState } from 'react';
import AdvancedSearch from '../AdvancedSearch';
import EditTableColumnForm from '../EditTableColumnForm';

type ProTablePlusProps<T, U, ValueType = 'text'> = {
  /** 重写的Column */
  defaultColumns?: ProColumns<T, ValueType>[];
  /** 查询接口 */
  query: (params: U) => Promise<ApiPagingResponse<T>>;
  /** 模块名称 */
  moduleName: string;
  /** 是否显示索引列 */
  showIndexColumn?: boolean;
  /** 是否显示详情 */
  showDescriptions?: boolean;
} & Partial<ProTableProps<T, U, ValueType>>;

/**
 * ProTable
 * @param props
 * @returns
 */
function ProTablePlus<T extends Record<string, any>, U extends ParamsType, ValueType = 'text'>(
  props: ProTablePlusProps<T, U, ValueType>,
) {
  const [historyColumnData, setHistoryColumnData] = useState<API.TableColumnItem[]>([]);
  const [columnData, setColumnData] = useState<API.TableColumnItem[]>([]);
  const [columnLoading, setColumnLoading] = useState<boolean>(true);
  const [tableWidth, setTableWidth] = useState<number | undefined>();
  const [columns, setColumns] = useState<ProColumns<T, ValueType>[]>(props.defaultColumns || []);
  const [remoteModuleName, setRemoteModuleName] = useState<string>();
  const showIndexColumn = props.showIndexColumn === undefined || props.showIndexColumn;

  useEffect(() => {
    const fetch = async () => {
      let list = columnData;
      if (list.length === 0) {
        const res = await queryColumns(props.moduleName);
        if (res.success) {
          list = res.data!?.columns;
          setRemoteModuleName(res.data?.moduleName);
        }
        const option = columns.find((x) => x.dataIndex === 'option');
        if (option !== undefined && list.find((x) => x.dataIndex === 'option') === undefined) {
          list.push({
            title: '操作',
            dataIndex: 'option',
            fixed: 'right',
            width: option.width,
            hideInTable: false,
            sort: 999,
            required: true,
          } as API.TableColumnItem);
        }
        setColumnData(list);
        setHistoryColumnData(cloneDeep(list));
      }
      setColumnLoading(false);
      const result: ProColumns<T, ValueType>[] = [];
      // 按sort排序
      list.sort((a, b) => a.sort - b.sort);
      for (let i = 0; i < list.length; i++) {
        const item = list[i];
        const find = columns.find((x) => x.dataIndex === item.dataIndex);
        if (find !== undefined) {
          find.title = find.title || item.title;
          find.hideInTable = find.dataIndex === 'option' ? find.hideInTable : item.hideInTable;
          find.width = item.width || find.width;
          find.fixed = item.fixed !== 'left' && item.fixed !== 'right' ? undefined : item.fixed;
          find.index = item.sort || i;
          // 以下属性如果为ture时则不覆盖
          find.sorter = find.sorter ? find.sorter : item.sorter;
          find.filters = find.filters ? find.filters : item.filters;
          find.ellipsis = find.ellipsis ? find.ellipsis : item.ellipsis;
          find.valueEnum = find.valueEnum ? find.valueEnum : item.valueEnum;
          result.push(find);
          continue;
        }
        const nItem: ProColumns<T, ValueType> = {
          ...item,
          hideInTable: item.hideInTable,
          ellipsis: item.ellipsis || true,
          index: item.sort || i,
        } as ProColumns<T, ValueType>;
        if (item.propertyType === 'boolean') {
          nItem.valueEnum = {
            false: { text: '否', status: 'Default' },
            true: { text: '是', status: 'Success' },
          };
        }
        result.push(nItem);
      }
      // 计算宽度
      let width: number = 0;
      for (let i = 0; i < list.length; i++) {
        const item = list[i];
        if (item.hideInTable) {
          continue;
        }
        width += item.width || 200;
      }
      setTableWidth(width);
      // 处理自定义的列
      for (let i = 0; i < (props.defaultColumns || []).length; i++) {
        const item = (props.defaultColumns || [])[i];
        const find = result.find((x) => x.dataIndex === item.dataIndex);
        if (find === undefined) {
          const nextIndex = i + 1;
          if (nextIndex < (props.defaultColumns || []).length) {
            const index = result.findIndex(
              (x) => x.dataIndex === (props.defaultColumns || [])[nextIndex].dataIndex,
            );
            if (index > -1) {
              result.splice(index, 0, item);
            }
          }
          continue;
        }
      }
      if (showIndexColumn) {
        setColumns([
          {
            title: '序号',
            dataIndex: 'index',
            fixed: 'left',
            valueType: 'indexBorder',
            width: 75,
            align: 'center',
          },
          ...result,
        ]);
      } else {
        setColumns(result);
      }
      // 检查数据是否有更新
      let isUpdate = false;
      if (list.length !== historyColumnData.length) {
        // 初始化都是0，所以为0时不处理
        if (historyColumnData.length !== 0) {
          isUpdate = true;
        }
      } else {
        for (let i = 0; i < list.length; i++) {
          const item = list[i];
          const find = historyColumnData[i];
          if (find.dataIndex !== item.dataIndex) {
            isUpdate = true;
            break;
          }
          if (find.hideInTable !== item.hideInTable) {
            isUpdate = true;
            break;
          }
          if (find.width !== item.width) {
            isUpdate = true;
            break;
          }
          if (find.fixed !== item.fixed && find.fixed !== undefined) {
            isUpdate = true;
            break;
          }
          if (find.sort !== item.sort) {
            isUpdate = true;
            break;
          }
        }
      }
      // 如果数据有更新，则更新列数据
      if (isUpdate) {
        // 更新列数据
        updateUserCustomColumns({
          moduleName: remoteModuleName,
          columns: list
            .filter((p) => p.dataIndex !== 'option')
            .map((x) => ({
              dataIndex: x.dataIndex,
              show: !x.hideInTable,
              width: x.width,
              fixed: x.fixed,
              sort: x.sort,
            })),
        });
      }
    };
    if (columnLoading && columns.length > 0) {
      fetch();
    }
  }, [columnLoading, columns]);

  const [advancedSearchFilter, setAdvancedSearchFilter] = useState<FilterGroupItem[]>([]);
  const [currentRow, setCurrentRow] = useState<T>();
  const { query } = props;
  return (
    <>
      <ProTable<T, U, ValueType>
        headerTitle={'查询表格'}
        rowKey="id"
        search={false}
        options={{
          search: {
            placeholder: '关健字搜索',
          },
          setting: false,
          fullScreen: true,
        }}
        onRow={(record) => {
          return {
            onClick: () => {
              if (props.showDescriptions) {
                setCurrentRow(record);
              }
            },
          };
        }}
        {...props}
        toolBarRender={(action, rows) => [
          ...(props.toolBarRender ? props.toolBarRender(action, rows) : []),
          ...[
            <Tooltip key={'advancedSearch'} title={'自定义查询'}>
              <AdvancedSearch
                data={columnData.filter((d) => d.dataIndex !== 'option')}
                historyFilters={advancedSearchFilter}
                onSearch={(d) => {
                  setAdvancedSearchFilter(d);
                }}
              />
            </Tooltip>,
            <Tooltip key={'editTableColumn'} title={'自定义表格'}>
              <EditTableColumnForm
                data={columnData}
                onOk={(list) => {
                  setColumnData(list);
                  setColumnLoading(true);
                }}
              />
            </Tooltip>,
          ],
        ]}
        // @ts-ignore
        params={{
          filterGroups: advancedSearchFilter,
          ...props.params,
        }}
        request={async (params, sorter, filter) => {
          const res = await query({ ...params, ...getSorter(sorter, 'a'), ...getFilter(filter) });
          return {
            data: res.data?.items || [],
            success: res.success,
            total: res.data?.totalItems || 0,
          };
        }}
        scroll={{ x: tableWidth || 1000 }}
        columns={columnLoading ? [] : columns}
      />
      <Drawer
        width={600}
        open={!!currentRow}
        onClose={() => {
          setCurrentRow(undefined);
        }}
        closable={false}
      >
        {currentRow !== undefined && (
          <ProDescriptions
            column={1}
            title={'详情'}
            dataSource={currentRow || {}}
            // @ts-ignore
            columns={columns}
          />
        )}
      </Drawer>
    </>
  );
}

export default ProTablePlus;
