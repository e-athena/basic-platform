<template>
  <ProContainer>
    <Breadcrumb :items="['系统模块', '用户管理']" />
    <ProTable
      :query="query"
      :columns="columns"
      module-name="User"
      show-descriptions
      detail-column-name="realName"
      :stripe="false"
    >
      <template #tool-bar>
        <a-button type="primary">
          <template #icon>
            <icon-plus />
          </template>
          创建
        </a-button>
        <a-upload action="/">
          <template #upload-button>
            <a-button>导入</a-button>
          </template>
        </a-upload>
        <a-button>
          <template #icon>
            <icon-download />
          </template>
          下载
        </a-button>
      </template>
    </ProTable>
  </ProContainer>
</template>

<script lang="ts" setup>
  import { h } from 'vue';
  import ProTable from '@/components/pro-table/index.vue';
  import ProContainer from '@/components/pro-container/index.vue';
  import { ProTableColumnData } from '@/components/pro-table/typing';
  import { Avatar, Button, Space } from '@arco-design/web-vue';
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
    {
      title: '操作',
      width: 125,
      fixed: 'right',
      align: 'center',
      dataIndex: 'option',
      render: ({ record }) => {
        return h(Space, { size: 'mini' }, [
          h(Button, { type: 'text', size: 'small' }, '编辑'),
        ]);
      },
    },
  ];
</script>

<script lang="ts">
  export default {
    name: 'UserTable',
  };
</script>

<style scoped lang="less"></style>
