import { ProLayoutProps } from '@ant-design/pro-components';

/**
 * @name
 */
const Settings: ProLayoutProps & {
  pwa?: boolean;
  logo?: string;
} = {
  // navTheme: 'light',
  // // 拂晓蓝
  // colorPrimary: '#1890ff',
  // layout: 'mix',
  // contentWidth: 'Fluid',
  // fixedHeader: false,
  // fixSiderbar: true,
  colorWeak: false,
  title: 'Athena Pro',
  // pwa: true,
  // // logo: 'https://gw.alipayobjects.com/zos/rmsportal/KDpgvguMpGfqaHPjicRK.svg',
  // logo: 'https://cdn.gzwjz.com/FmzrX15jYA03KMVfbgMJnk-P6WGl.png',
  // iconfontUrl: '',
  // token: {
  //   // 参见ts声明，demo 见文档，通过token 修改样式
  //   //https://procomponents.ant.design/components/layout#%E9%80%9A%E8%BF%87-token-%E4%BF%AE%E6%94%B9%E6%A0%B7%E5%BC%8F
  // },

  navTheme: 'light',
  colorPrimary: '#1890ff',
  layout: 'mix',
  contentWidth: 'Fluid',
  fixedHeader: true,
  fixSiderbar: true,
  pwa: true,
  logo: 'https://cdn.gzwjz.com/FmzrX15jYA03KMVfbgMJnk-P6WGl.png',
  token: {},
  siderMenuType: 'sub',
  splitMenus: true,
};

export default Settings;
