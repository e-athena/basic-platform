import {defineConfig} from '@umijs/max';

export default defineConfig({
  // 打包后输入目录
  outputPath: '../wwwroot',
  history: {
    type: 'hash',
  },

  define: {
    API_URL: 'http://e-cms.zhengjinfan.cn',
    RTC_URL: 'http://e-cms.zhengjinfan.cn',
    SSO_URL: 'http://basic.zhengjinfan.cn/#/user/login-redirect',
  },
});
