import { defineConfig } from '@umijs/max';

export default defineConfig({
  define: {
    API_URL: 'http://localhost:5152',
    RTC_URL: 'http://localhost:5152',
    SSO_URL: 'http://localhost:5079/#/user/login-redirect',
  },
});
