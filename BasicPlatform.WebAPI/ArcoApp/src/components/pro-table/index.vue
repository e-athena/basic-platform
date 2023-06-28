<template>
  <a-card ref="fullscreenEl" class="general-card" title="查询表格">
    <a-row style="margin-bottom: 16px">
      <a-col :span="16">
        <a-space>
          <slot name="tool-bar" />
        </a-space>
      </a-col>
      <a-col :span="8" style="text-align: right">
        <a-space>
          <a-input-search
            :placeholder="props.searchPlaceholder || '关键字'"
            search-button
            allow-clear
            @search="handleSearch"
          />
          <a-tooltip content="自定义查询">
            <a-button>
              <template #icon>
                <icon-search />
              </template>
            </a-button>
          </a-tooltip>
          <a-tooltip content="列设置">
            <a-button>
              <template #icon>
                <icon-settings />
              </template>
            </a-button>
          </a-tooltip>
          <a-tooltip content="密度">
            <a-dropdown v-model="size" @select="handleSizeSelect">
              <a-button>
                <template #icon>
                  <icon-swap />
                </template>
              </a-button>
              <template #content>
                <a-doption :disabled="size === 'large'" value="large"
                  >默认</a-doption
                >
                <a-doption :disabled="size === 'medium'" value="medium"
                  >中等</a-doption
                >
                <a-doption :disabled="size === 'small'" value="small"
                  >紧凑</a-doption
                >
                <a-doption :disabled="size === 'mini'" value="mini"
                  >迷你</a-doption
                >
              </template>
            </a-dropdown>
          </a-tooltip>
          <a-tooltip :content="isFullscreen ? '退出全屏' : '全屏'">
            <a-button v-show="false" @click="handleFullscreenClick">
              <template #icon>
                <icon-fullscreen-exit v-if="isFullscreen" />
                <icon-fullscreen v-else />
              </template>
            </a-button>
          </a-tooltip>
        </a-space>
      </a-col>
    </a-row>
    <a-table
      :row-key="rowKey"
      :loading="loading"
      :pagination="pagination"
      :data="renderData"
      :bordered="false"
      :columns="columns"
      :size="size"
      stripe
      @page-change="onPageChange"
    />
    <a-drawer
      width="50%"
      :visible="currentRow !== undefined"
      unmount-on-close
      :footer="false"
      @cancel="handleDrawerCancel"
    >
      <template #title>详情</template>
      <div v-if="detailColumnData.length === 0">
        <a-descriptions :column="2" :data="normalDetailData" bordered />
      </div>
      <div v-else>
        <a-collapse :default-active-key="['1']">
          <a-collapse-item
            key="1"
            header="Beijing Toutiao Technology Co., Ltd."
          >
            <a-descriptions
              :column="2"
              :data="[
                {
                  label: 'Name',
                  value: 'Socrates',
                },
                {
                  label: 'Mobile',
                  value: '123-1234-1234',
                },
                {
                  label: 'Residence',
                  value: 'Beijing',
                },
                {
                  label: 'Hometown',
                  value: 'Beijing',
                },
                {
                  label: 'Address',
                  value: 'Yingdu Building, Zhichun Road, Beijing',
                },
              ]"
              bordered
            />
          </a-collapse-item>
        </a-collapse>
      </div>
    </a-drawer>
  </a-card>
</template>

<script setup lang="ts">
  import { ref, reactive, h } from 'vue';
  import useLoading from '@/hooks/loading';
  import {
    ApiPagingResponse,
    ApiResponse,
    FilterGroupItem,
    Pagination,
    PagingParams,
    SelectInfo,
    TableColumnItem,
  } from '@/types/global';
  import { Button, DescData } from '@arco-design/web-vue';
  import { queryDetail } from '@/utils/utils';
  import dayjs from 'dayjs';
  import { useFullscreen } from '@vueuse/core';
  import {
    queryColumns,
    queryDetailColumns,
    queryUserSelectList,
    updateUserCustomColumns,
  } from './service';
  import { ProTableColumnData } from './typing';

  type ProTableProps = {
    /** 只使用本地配置的列 */
    onlyUseLocalColumns?: boolean;
    /** 表格配置列 */
    columns?: ProTableColumnData[];
    /** 查询列表接口 */
    query: (params: PagingParams) => Promise<ApiPagingResponse<any>>;
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
    rowKey?: string;
  };

  const props = defineProps<ProTableProps>();
  const size = ref<'mini' | 'small' | 'medium' | 'large'>('large');
  const { loading, setLoading } = useLoading(true);
  const renderData = ref<any[]>([]);
  const historyColumnData = ref<TableColumnItem[]>([]);
  const columnData = ref<TableColumnItem[]>([]);
  const columnLoading = ref<boolean>(true);
  const tableWidth = ref<number | undefined>(0);
  const columns = ref<ProTableColumnData[]>(props.columns || []);
  const rowKey = ref<string>(props.rowKey || 'id');
  const remoteModuleName = ref<string>();
  const detailColumnData = ref<TableColumnItem[]>([]);
  const userSelectList = ref<SelectInfo[]>();
  const advancedSearchFilter = ref<FilterGroupItem[]>([]);
  const currentRow = ref<any>();
  const normalDetailData = ref<DescData[]>([]);
  const showIndexColumn =
    props.showIndexColumn === undefined || props.showIndexColumn;
  const fullscreenEl = ref<HTMLElement | null>(null);

  const { isFullscreen, toggle } = useFullscreen(fullscreenEl);
  const handleFullscreenClick = (e: MouseEvent) => {
    e.stopPropagation();
    toggle();
  };

  // detailColumnData按group分组
  const detailColumnDataGroup = detailColumnData.value.reduce((prev, cur) => {
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
  }, [] as { group: string; description?: string; columns: TableColumnItem[] }[]);

  const fetchColumns = async () => {
    let list = columnData.value;
    const defaultColumns = props.columns || [];
    if (list.length === 0) {
      // 如果不是只使用本地配置的列，则从后台获取列配置
      if (!props.onlyUseLocalColumns) {
        const res = await queryColumns(props.moduleName);
        if (res.success) {
          list = res.data.columns;
          remoteModuleName.value = res.data.moduleName;
        }
      } else {
        list = defaultColumns.map((x) => {
          return {
            ...x,
            // hideInTable: x.hideInTable || false,
            sort: x.index || 999,
          } as TableColumnItem;
        });
      }
      const option = defaultColumns.find((x) => x.dataIndex === 'option');
      if (
        option !== undefined &&
        list.find((x) => x.dataIndex === 'option') === undefined
      ) {
        list.push({
          title: '操作',
          dataIndex: 'option',
          fixed: 'right',
          width: option.width,
          hideInTable: false,
          sort: 999,
          required: true,
        } as TableColumnItem);
      }
      columnData.value = list;
      historyColumnData.value = [...list];
    }
    if (userSelectList.value === undefined) {
      try {
        const res = await queryUserSelectList();
        if (res.success) {
          userSelectList.value = res.data;
        }
      } catch (error) {
        // console.log(error);
      }
    }
    const result: ProTableColumnData[] = [];
    // 按sort排序
    list.sort((a, b) => a.sort - b.sort);
    for (let i = 0; i < list.length; i += 1) {
      const item = list[i];
      const find = defaultColumns.find((x) => x.dataIndex === item.dataIndex);
      const nItem =
        find === undefined
          ? ({
              ...item,
              hideInTable: item.hideInTable,
              ellipsis: item.ellipsis || true,
              index: item.sort || i,
            } as ProTableColumnData)
          : find;
      if (find !== undefined) {
        nItem.title = find.title || item.title;
        nItem.hideInTable = find.hideInTable || item.hideInTable;
        nItem.hideInDescriptions =
          nItem.hideInDescriptions || item.hideInDescriptions;
        nItem.width = item.width || find.width;
        nItem.fixed =
          item.fixed !== 'left' && item.fixed !== 'right'
            ? undefined
            : item.fixed;
        nItem.index = item.sort || i;
      }
      if (item.sorter) {
        nItem.sortable = {
          sortDirections: ['ascend', 'descend'],
        };
      }
      if (item.filters) {
        nItem.filterable = {
          filters: (item.enumOptions || []).map((p: SelectInfo) => ({
            text: p.label,
            value: p.value,
          })),
          filter: (value, record) => {
            return value.some((p) => {
              return (
                parseInt(p as string, 10) === record[item.dataIndex as string]
              );
            });
          },
          multiple: true,
        };
      }
      if (item.propertyType === 'enum') {
        nItem.render = ({ record }) => {
          const value = record[item.dataIndex as string];
          const option = item.enumOptions?.find(
            (x) => parseInt(x.value, 10) === value
          );
          return option?.label || '';
        };
      }
      if (item.propertyType === 'boolean') {
        nItem.render = ({ record }) => {
          const flag = record[item.dataIndex as string] as boolean;
          return flag ? '是' : '否';
        };
      }
      if (item.dataIndex.includes('UserId') && item.hideInTable === false) {
        nItem.render = ({ record }) => {
          const text = record[item.dataIndex as string];
          return (
            userSelectList.value?.find((x) => x.value === text)?.label || ''
          );
        };
      }
      // 如果是日期类型，则需要格式化
      if (item.valueType === 'dateTime' || item.valueType === 'date') {
        switch (item.valueType) {
          case 'dateTime':
            nItem.render = ({ record }) => {
              const text = record[item.dataIndex as string];
              return text ? dayjs(text).format('YYYY-MM-DD HH:mm:ss') : '';
            };
            break;
          case 'date':
            nItem.render = ({ record }) => {
              const text = record[item.dataIndex as string];
              return text ? dayjs(text).format('YYYY-MM-DD') : '';
            };
            break;
          default:
            break;
        }
      }
      if (
        props.showDescriptions &&
        ((props.detailColumnName === undefined && i === 0) ||
          props.detailColumnName === item.dataIndex)
      ) {
        nItem.render = ({ record }) => {
          return h(
            Button,
            {
              type: 'text',
              size: 'small',
              onClick: async (e) => {
                e.stopPropagation();
                if (props.queryDetail !== undefined) {
                  if (detailColumnData.value.length === 0) {
                    // 查询详情列
                    const res = await queryDetailColumns(props.moduleName);
                    detailColumnData.value = res.success
                      ? res.data.columns || []
                      : [];
                  }
                  const data = await queryDetail(props.queryDetail, record.id);
                  if (data) {
                    currentRow.value = data;
                  }
                  return;
                }
                const c: DescData[] = [];
                const c1 =
                  (detailColumnData.value.length !== 0
                    ? detailColumnData.value
                    : columnData.value
                  ).filter((x) => !x.hideInDescriptions) || [];

                for (let j = 0; j < c1.length; j += 1) {
                  const item = c1[j];
                  const value = record[item.dataIndex] as string;
                  c.push({
                    label: item.title,
                    value: value || '',
                  });
                }

                normalDetailData.value = c;
                currentRow.value = record;
              },
            },
            record[nItem.dataIndex as string]
          );
        };
      }
      result.push(nItem);
    }
    // 计算宽度
    let width = 0;
    for (let i = 0; i < list.length; i += 1) {
      const item = list[i];
      if (!item.hideInTable) {
        width += item.width || 200;
      }
    }
    tableWidth.value = width;
    // 处理自定义的列
    for (let i = 0; i < defaultColumns.length; i += 1) {
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
            (x) => x.dataIndex === defaultColumns[nextIndex].dataIndex
          );
          if (index > -1) {
            result.splice(index, 0, item);
          }
        }
      }
    }
    if (showIndexColumn) {
      columns.value = [
        {
          title: '序号',
          dataIndex: 'index',
          fixed: 'left',
          // valueType: 'indexBorder',
          width: 75,
          align: 'center',
        },
        ...result,
      ];
    } else {
      columns.value = result.filter((x) => x.hideInTable !== true);
    }

    // 检查数据是否有更新
    let isUpdate = false;
    if (list.length !== historyColumnData.value.length) {
      // 初始化都是0，所以为0时不处理
      if (historyColumnData.value.length !== 0) {
        isUpdate = true;
      }
    } else {
      for (let i = 0; i < list.length; i += 1) {
        const item = list[i];
        const find = historyColumnData.value[i];
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
    columnLoading.value = false;
  };

  if (columnLoading.value) {
    fetchColumns();
  }

  const basePagination: Pagination = {
    current: 1,
    pageSize: 20,
  };
  const pagination = reactive({
    ...basePagination,
  });
  /** 请求数据 */
  const fetchData = async (
    params: PagingParams = {
      current: 1,
      pageSize: 20,
    }
  ) => {
    setLoading(true);
    try {
      const { data } = await props.query({
        filterGroups: advancedSearchFilter.value,
        ...params,
      });
      renderData.value = data.items || [];
      pagination.current = params.current;
      pagination.total = data.totalItems;
    } catch (err) {
      // you can report use errorHandler or other
    } finally {
      setLoading(false);
    }
  };
  /** 分页变更 */
  const onPageChange = (current: number) => {
    fetchData({ ...basePagination, current });
  };

  const handleSearch = async (value: string) => {
    fetchData({
      ...pagination,
      keyword: value,
    });
  };

  // 选择表格大小
  const handleSizeSelect = (v: 'mini' | 'small' | 'medium' | 'large') => {
    size.value = v;
  };

  const handleDrawerCancel = () => {
    currentRow.value = undefined;
  };

  fetchData();
</script>

<script lang="ts">
  export default {
    name: 'ProTable',
  };
</script>

<style scoped lang="less"></style>

<style lang="less">
  .message-popover {
    .arco-popover-content {
      margin-top: 0;
    }
  }
</style>
