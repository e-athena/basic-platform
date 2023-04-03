import { defineConfig } from '@umijs/max';

export default defineConfig({
  // 打包后输入目录
  outputPath: '../wwwroot',
  history: {
    type: 'hash',
  },

  define: {
    API_URL: '',
    RTC_URL: '',
  },
});
