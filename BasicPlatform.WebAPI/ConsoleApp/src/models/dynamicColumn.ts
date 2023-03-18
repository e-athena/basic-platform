import { queryColumns, updateUserCustomColumns } from '@/services/ant-design-pro/api';
import { ProColumns } from '@ant-design/pro-components';
import { useEffect, useState } from 'react';
import { cloneDeep } from 'lodash';

/** 动态列 */
export default () => {
  const [historyColumnData, setHistoryColumnData] = useState<API.TableColumnItem[]>([]);
  const [columnData, setColumnData] = useState<API.TableColumnItem[]>([]);
  const [columnLoading, setColumnLoading] = useState<boolean>(true);
  const [tableWidth, setTableWidth] = useState<number | undefined>();
  const [columns, setColumns] = useState<ProColumns<any>[]>([]);
  const [modelName, setModelName] = useState<string>();

  useEffect(() => {
    const fetch = async () => {
      setColumnLoading(false);
      let list = columnData;
      if (list.length === 0) {
        const res = await queryColumns(modelName!);
        list = res.success ? res.data! : [];
        const option = columns.find(x => x.dataIndex === 'option');
        if (option !== undefined && list.find(x => x.dataIndex === 'option') === undefined) {
          list.push({
            title: '操作',
            dataIndex: 'option',
            fixed: 'right',
            width: option.width,
            show: true,
            sort: 999,
            required: true
          } as API.TableColumnItem);
        }
        setColumnData(list);
        setHistoryColumnData(cloneDeep(list));
      }
      const result: ProColumns<any>[] = [];
      // 按sort排序
      list.sort((a, b) => a.sort - b.sort);
      for (let i = 0; i < list.length; i++) {
        const item = list[i];
        const find = columns.find(x => x.dataIndex === item.dataIndex);
        if (find !== undefined) {
          find.hideInTable = !item.show;
          find.width = item.width || find.width;
          find.fixed = (item.fixed !== 'left' && item.fixed !== 'right') ? undefined : item.fixed;
          find.index = item.sort || i;
          // 以下属性如果为ture时则不覆盖
          find.sorter = find.sorter ? find.sorter : item.sorter;
          find.filters = find.filters ? find.filters : item.filters;
          find.ellipsis = find.ellipsis ? find.ellipsis : item.ellipsis;
          find.valueEnum = find.valueEnum ? find.valueEnum : item.valueEnum;
          result.push(find);
          continue;
        }
        result.push({
          ...item,
          hideInTable: !item.show,
          ellipsis: item.ellipsis || true,
          index: item.sort || i,
        } as ProColumns<API.UserListItem>);
      }
      // 计算宽度
      let width: number = 0;
      for (let i = 0; i < list.length; i++) {
        const item = list[i];
        if (!item.show) {
          continue;
        }
        width += item.width || 200;
      }
      setTableWidth(width);
      setColumns(result);
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
          if (find.show !== item.show) {
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
          columns: list.filter(p => p.dataIndex !== 'option').map(x => ({
            dataIndex: x.dataIndex,
            show: x.show,
            width: x.width,
            fixed: x.fixed,
            sort: x.sort
          }))
        });
      }
    }
    if (columnLoading && columns.length > 0 && modelName !== undefined) {
      fetch();
    }
  }, [columnLoading, columns, modelName]);

  const setDefaultColumns = (data: ProColumns<any>[]) => {
    if (columns.length === 0) {
      setColumns(data);
    }
  }
  const setDefaultModelName = (modelName: string) => {
    if (modelName !== undefined) {
      setModelName(modelName);
    }
  }

  return {
    /**远程的动态列数据 */
    columnData,
    setColumnData,
    /**列数据加载状态 */
    columnLoading,
    setColumnLoading,
    /**表格宽度 */
    tableWidth,
    /**表格列数据 */
    columns,
    /**设置自定义的列，用于实现自定义渲染等功能 */
    setDefaultColumns,
    /**设置模型名称，用于获取列数据 */
    setDefaultModelName
  };
};