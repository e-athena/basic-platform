<template>
  <div class="container">
    <slot></slot>
  </div>
</template>

<script lang="ts">
  export default {
    name: 'ProContainer',
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
