<template>
  <div class="container">
    <Breadcrumb :items="['系统模块', '用户管理']" />
    <ProTable
      :query="query"
      :columns="columns"
      module-name="User"
      show-descriptions
      detail-column-name="realName"
    />
  </div>
</template>

<script lang="ts" setup>
  import ProTable from '@/components/pro-table/index.vue';
  import { h } from 'vue';
  import { ProTableColumnData } from '@/components/pro-table/typing';
  import { Avatar } from '@arco-design/web-vue';
  import { query } from './service';

  const columns: ProTableColumnData[] = [
    {
      title: '头像',
      dataIndex: 'avatar',
      render: ({ record }) => {
        return h(Avatar, {
          imageUrl: record.avatar,
          size: 16,
        });
      },
    },
  ];
</script>

<script lang="ts">
  export default {
    name: 'SearchTable',
  };
</script>

<style scoped lang="less">
  .container {
    padding: 0 20px 20px 20px;
  }
  :deep(.arco-table-th) {
    &:last-child {
      .arco-table-th-item-title {
        margin-left: 16px;
      }
    }
  }
</style>
