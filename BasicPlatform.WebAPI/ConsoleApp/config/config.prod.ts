import { defineConfig } from '@umijs/max';

export default defineConfig({
  // 打包后输入目录
  outputPath: '../wwwroot/w',
  publicPath: 'w/',
  history: {
    type: 'hash',
  },
  esbuildMinifyIIFE: true,
  define: {
    API_URL: '',
    RTC_URL: '',
  },
});
