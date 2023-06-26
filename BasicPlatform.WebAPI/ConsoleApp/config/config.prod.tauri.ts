import { defineConfig } from '@umijs/max';

export default defineConfig({
  outputPath: 'dist',
  history: {
    type: 'hash',
  },
  define: {
    API_URL: 'https://dev.gateway.gzwjz.com/bpc',
    RTC_URL: 'http://basic.zhengjinfan.cn',
  },
});
