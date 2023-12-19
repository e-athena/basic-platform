import {
  queryColumns,
  queryDetailColumns,
  updateUserCustomColumns,
} from '@/services/ant-design-pro/api';
import { getFilter, getSorter, queryDetail } from '@/utils/utils';
import {
  ParamsType,
  ProColumns,
  ProDescriptions,
  ProTable,
  ProTableProps,
} from '@ant-design/pro-components';
import { Collapse, Drawer, Space, Tooltip, Image, Button } from 'antd';
import { cloneDeep } from 'lodash';
import { useEffect, useState } from 'react';
import AdvancedSearch from '../AdvancedSearch';
import EditTableColumnForm from '../EditTableColumnForm';
import { ZoomInOutlined, InfoCircleOutlined } from '@ant-design/icons';
import { querySelectList } from '@/services/ant-design-pro/system/user';

type ProTablePlusProps<T, U, ValueType = 'text'> = {
  /** 只使用本地配置的列 */
  onlyUseLocalColumns?: boolean;
  /** 默认的Column */
  defaultColumns?: ProColumns<T, ValueType>[];
  /** 查询列表接口 */
  query: (params: U) => Promise<ApiPagingResponse<T>>;
  /** 查询详情接口 */
  queryDetail?: (id: string) => Promise<ApiResponse<any>>;
  /** 模块名称 */
  moduleName: string;
  /** 是否显示索引列 */
  showIndexColumn?: boolean;
  /** 是否显示详情 */
  showDescriptions?: boolean;
  /** 搜索框提示内容 */
  searchPlaceholder?: string;
  /** 表格的Y轴高度 */
  scrollY?: number;
  /** 详情列列名 */
  detailColumnName?: string;
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
  const [detailColumnData, setDetailColumnData] = useState<API.TableColumnItem[]>([]);
  const [userValueEnums, setUserValueEnums] = useState<Record<string, { text: string }>>();
  const [advancedSearchFilter, setAdvancedSearchFilter] = useState<FilterGroupItem[]>([]);
  const [currentRow, setCurrentRow] = useState<T>();
  const { query, searchPlaceholder } = props;

  // detailColumnData按group分组
  const detailColumnDataGroup = detailColumnData.reduce((prev, cur) => {
    const group = prev.find((x) => x.group === cur.group);
    if (group) {
      if (cur.groupDescription && !group.description) {
        group.description = cur.groupDescription;
      }
      group.columns.push(cur);
    } else {
      prev.push({
        description: cur.groupDescription,
        group: cur.group,
        columns: [cur],
      });
    }
    return prev;
  }, [] as { group: string; description?: string; columns: API.TableColumnItem[] }[]);

  useEffect(() => {
    const fetch = async () => {
      let list = columnData;
      const defaultColumns = props.defaultColumns || [];
      if (list.length === 0) {
        // 如果不是只使用本地配置的列，则从后台获取列配置
        if (!props.onlyUseLocalColumns) {
          const res = await queryColumns(props.moduleName);
          if (res.success) {
            list = res.data!?.columns;
            setRemoteModuleName(res.data?.moduleName);
          }
        } else {
          list = defaultColumns.map((x) => {
            return {
              ...x,
              hideInTable: x.hideInTable || false,
              sort: x.index || 999,
            } as API.TableColumnItem;
          });
        }
        const option = defaultColumns.find((x) => x.dataIndex === 'option');
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
      let userListValueEnums: Record<string, { text: string }> = {};
      if (userValueEnums === undefined) {
        try {
          const res = await querySelectList();
          if (res.success) {
            const data = res.data!.reduce((prev, cur) => {
              prev[cur.value] = { text: cur.label };
              return prev;
            }, {} as Record<string, { text: string }>);
            userListValueEnums = data;
            setUserValueEnums(data);
          }
        } catch (error) {
          console.log(error);
        }
      } else {
        userListValueEnums = userValueEnums;
      }
      const result: ProColumns<T, ValueType>[] = [];
      // 按sort排序
      list.sort((a, b) => a.sort - b.sort);
      for (let i = 0; i < list.length; i++) {
        const item = list[i];
        const find = defaultColumns.find((x) => x.dataIndex === item.dataIndex);
        if (find !== undefined) {
          find.title = find.title || item.title;
          find.tooltip = find.tooltip || item.tooltip;
          // find.hideInTable = find.dataIndex === 'option' ? find.hideInTable : item.hideInTable;
          find.hideInTable = find.hideInTable || item.hideInTable;
          find.hideInDescriptions = find.hideInDescriptions || item.hideInDescriptions;
          find.width = item.width || find.width;
          find.fixed = item.fixed !== 'left' && item.fixed !== 'right' ? undefined : item.fixed;
          find.index = item.sort || i;
          // 以下属性如果为ture时则不覆盖
          find.sorter = find.sorter ? find.sorter : item.sorter;
          find.filters = find.filters ? find.filters : item.filters;
          find.ellipsis = find.ellipsis ? find.ellipsis : item.ellipsis;
          find.valueEnum = find.valueEnum ? find.valueEnum : item.valueEnum;
          if (
            props.showDescriptions &&
            ((props.detailColumnName === undefined && i === 0) ||
              props.detailColumnName === item.dataIndex)
          ) {
            find.render = (dom, record) => {
              return (
                <Button
                  size={'small'}
                  type={'link'}
                  onClick={async () => {
                    if (props.queryDetail !== undefined) {
                      if (detailColumnData.length === 0) {
                        // 查询详情列
                        const res = await queryDetailColumns(props.moduleName);
                        setDetailColumnData(res.success ? res.data?.columns || [] : []);
                      }
                      const data = await queryDetail(props.queryDetail, record.id!);
                      if (data) {
                        setCurrentRow(data);
                      }
                      return;
                    }
                    setCurrentRow(record);
                  }}
                  icon={<ZoomInOutlined />}
                >
                  {dom}
                </Button>
              );
            };
          }
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
        if (item.dataIndex.includes('UserId') && item.hideInTable === false) {
          nItem.valueType = 'select';
          nItem.valueEnum = userListValueEnums;
          // 如果值在userListValueEnums中不存在，则显示-，否则显示值
          nItem.renderText = (value) => {
            return userListValueEnums[value] ? userListValueEnums[value].text : '-';
          };
        }
        if (
          props.showDescriptions &&
          ((props.detailColumnName === undefined && i === 0) ||
            props.detailColumnName === item.dataIndex)
        ) {
          nItem.render = (dom, record) => {
            return (
              <Button
                size={'small'}
                type={'link'}
                onClick={async () => {
                  if (props.queryDetail !== undefined) {
                    if (detailColumnData.length === 0) {
                      // 查询详情列
                      const res = await queryDetailColumns(props.moduleName);
                      setDetailColumnData(res.success ? res.data?.columns || [] : []);
                    }
                    const data = await queryDetail(props.queryDetail, record.id!);
                    if (data) {
                      setCurrentRow(data);
                    }
                    return;
                  }
                  setCurrentRow(record);
                }}
                icon={<ZoomInOutlined />}
              >
                {dom}
              </Button>
            );
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
      for (let i = 0; i < defaultColumns.length; i++) {
        const item = defaultColumns[i];
        // const find = result.find((x) => x.dataIndex === item.dataIndex);
        // if (find === undefined) {
        //   result.splice(i, 0, item);
        // }
        const find = result.find((x) => x.dataIndex === item.dataIndex);
        if (find === undefined) {
          const nextIndex = i + 1;
          if (nextIndex < defaultColumns.length) {
            const index = result.findIndex(
              (x) => x.dataIndex === defaultColumns[nextIndex].dataIndex,
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
      if (isUpdate && remoteModuleName) {
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
      setColumnLoading(false);
    };
    if (columnLoading && props.defaultColumns) {
      fetch();
    }
  }, [columnLoading, props.defaultColumns]);

  /** 渲染详情项 */
  const renderProDescriptionsItem = (x: API.TableColumnItem) => {
    if (x.valueType === 'image-list') {
      return (
        <ProDescriptions.Item
          labelStyle={{ width: x.width || 100 }}
          key={x.dataIndex}
          dataIndex={x.dataIndex}
          label={x.title === '' ? false : x.title}
          tooltip={x.tooltip}
          renderText={(value) => {
            return (
              <Space>
                {(value || []).map((v: string) => {
                  return <Image width={40} key={v} src={`${CDN_ADDRESS}/${v}`} />;
                })}
              </Space>
            );
          }}
        />
      );
    }
    if (x.dataIndex.includes('UserId')) {
      return (
        <ProDescriptions.Item
          key={x.dataIndex}
          dataIndex={x.dataIndex}
          label={x.title}
          tooltip={x.tooltip}
          request={async () => {
            const res = await querySelectList();
            return res.data || [];
          }}
        />
      );
    }
    return (
      <ProDescriptions.Item
        key={x.dataIndex}
        dataIndex={x.dataIndex}
        label={x.title}
        valueType={x.valueType}
        valueEnum={x.valueEnum}
        tooltip={x.tooltip}
        renderText={(value) => {
          if (x.propertyType === 'boolean') {
            return value ? '是' : '否';
          }
          return value;
        }}
      />
    );
  };

  return (
    <>
      <ProTable<T, U, ValueType>
        headerTitle={'查询表格'}
        rowKey="id"
        search={false}
        options={{
          search: {
            placeholder: searchPlaceholder || '关健字搜索',
          },
          setting: false,
          fullScreen: true,
        }}
        {...props}
        toolBarRender={(action, rows) => [
          ...(props.toolBarRender ? props.toolBarRender(action, rows) : []),
          ...[
            <AdvancedSearch
              key={'advancedSearch'}
              data={columnData.filter((d) => d.dataIndex !== 'option')}
              historyFilters={advancedSearchFilter}
              onSearch={(d) => {
                setAdvancedSearchFilter(d);
              }}
            />,
            <EditTableColumnForm
              key={'editTableColumn'}
              data={columnData.filter((d) => !d.tableIgnore)}
              onOk={(list) => {
                setColumnData(list);
                setColumnLoading(true);
              }}
            />,
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
        scroll={{
          x: tableWidth || 1000,
          y: props.scrollY || window.innerHeight - 352,
        }}
        columns={columnLoading ? [] : columns}
      />
      {detailColumnData.length === 0 ? (
        <Drawer
          width={'50%'}
          open={!!currentRow}
          onClose={() => {
            setCurrentRow(undefined);
          }}
          title={'详情'}
        >
          {currentRow !== undefined && (
            <ProDescriptions
              column={2}
              title={false}
              size={'small'}
              bordered
              dataSource={currentRow || {}}
            >
              {(detailColumnData.length !== 0 ? detailColumnData : columnData)
                .filter((x) => !x.hideInDescriptions && x.dataIndex !== 'option')
                .sort((x) => x.sort)
                .map((x) => renderProDescriptionsItem(x) as unknown as React.ReactNode)}
            </ProDescriptions>
          )}
        </Drawer>
      ) : (
        <Drawer
          width={'50%'}
          open={!!currentRow}
          onClose={() => {
            setCurrentRow(undefined);
          }}
          title={'详情'}
        >
          <Collapse defaultActiveKey={detailColumnDataGroup.map((x) => x.group)}>
            {detailColumnDataGroup.map((x) => {
              return (
                <>
                  <Collapse.Panel
                    header={x.group === 'default' ? '基础信息' : x.group}
                    key={x.group}
                    extra={
                      x.description ? (
                        <Tooltip title={x.description}>
                          <InfoCircleOutlined />
                        </Tooltip>
                      ) : undefined
                    }
                  >
                    <ProDescriptions
                      column={2}
                      title={false}
                      size={'small'}
                      bordered
                      dataSource={currentRow || {}}
                    >
                      {x.columns
                        .filter((x) => !x.hideInDescriptions && x.dataIndex !== 'option')
                        .sort((x) => x.sort)
                        .map((x) => renderProDescriptionsItem(x) as unknown as React.ReactNode)}
                    </ProDescriptions>
                  </Collapse.Panel>
                </>
              );
            })}
          </Collapse>
        </Drawer>
      )}
    </>
  );
}

export default ProTablePlus;
