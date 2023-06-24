import Footer from '@/components/Footer';
import { Question, NavTheme, TenantInfo } from '@/components/RightContent';
import { LinkOutlined } from '@ant-design/icons';
import { MenuDataItem, Settings as LayoutSettings, PageLoading } from '@ant-design/pro-components';
import { SettingDrawer } from '@ant-design/pro-components';
import type { RunTimeLayoutConfig } from '@umijs/max';
import { history, Link } from '@umijs/max';
import defaultSettings from '../config/defaultSettings';
import { errorConfig } from './requestErrorConfig';
import {
  currentUser as queryCurrentUser,
  queryUserResources,
  queryApplicationMenuResources,
  queryApplicationDataPermissionResources,
  queryExternalPages,
  addUserAccessRecord,
  queryAppConfig,
} from './services/ant-design-pro/api';
import React from 'react';
import { AvatarDropdown, AvatarName } from './components/RightContent/AvatarDropdown';
import fixMenuItemIcon from './components/FixMenuItemIcon';
import { recursionMenu } from './utils/menu';
const isDev = process.env.NODE_ENV === 'development';
// const loginPath = '/user/login';
import { fetchSignalRConnectionNotice } from './signalr/connection';
import { HubConnection } from '@microsoft/signalr';
import { getNavTheme, setNavTheme } from '@/utils/navTheme';

/**
 * @see  https://umijs.org/zh-CN/plugins/plugin-initial-state
 * */
export async function getInitialState(): Promise<{
  customNavTheme?: 'realDark' | 'light' | undefined;
  setCustomNavTheme?: (theme: string) => void;
  settings?: Partial<LayoutSettings>;
  currentUser?: API.CurrentUser;
  loading?: boolean;
  fetchUserInfo?: () => Promise<API.CurrentUser | undefined>;
  fetchMenuData?: () => Promise<MenuDataItem[]>;
  fetchApiResources?: () => Promise<API.ResourceInfo[]>;
  apiResources?: API.ResourceInfo[];
  fetchApplicationResources?: () => Promise<API.ApplicationMenuResourceInfo[]>;
  fetchApplicationDataPermissionResources?: () => Promise<
    API.ApplicationDataPermissionResourceInfo[]
  >;
  fetchExternalPages?: () => Promise<API.ExternalPage[]>;
  externalPages?: API.ExternalPage[];
  noticeConnectionHub?: HubConnection;
  fetchSignalRConnectionNotice?: (token: string) => Promise<HubConnection>;
  // eventConnectionHub?: HubConnection;
  // fetchSignalRConnectionEvent?: (token: string) => Promise<HubConnection>;
}> {
  const fetchUserInfo = async () => {
    try {
      const msg = await queryCurrentUser({
        skipErrorHandler: true,
      });
      return msg.data;
    } catch (error) {
      history.push(LOGIN_PATH);
    }
    return undefined;
  };
  const fetchApiResources = async () => {
    const res = await queryUserResources();
    return res.data || [];
  };
  const fetchApplicationResources = async () => {
    const res = await queryApplicationMenuResources();
    return res.data || [];
  };
  const fetchApplicationDataPermissionResources = async () => {
    const res = await queryApplicationDataPermissionResources();
    return res.data || [];
  };
  const fetchExternalPages = async () => {
    const res = await queryExternalPages();
    return res.data || [];
  };
  const fetchMenuData = async () => {
    const data = await fetchApiResources();
    return recursionMenu(data);
  };
  // 如果不是登录页面，执行
  const { location } = history;
  if (location.pathname !== LOGIN_PATH) {
    const currentUser = await fetchUserInfo();
    const apiResources = await fetchApiResources();
    const externalPages = await fetchExternalPages();
    // SignalR
    const noticeConnectionHub = await fetchSignalRConnectionNotice();
    // const eventConnectionHub = await fetchSignalRConnectionEvent(token);
    return {
      fetchMenuData,
      fetchUserInfo,
      fetchApiResources,
      fetchApplicationResources,
      fetchApplicationDataPermissionResources,
      fetchExternalPages,
      noticeConnectionHub,
      fetchSignalRConnectionNotice,
      // eventConnectionHub,
      // fetchSignalRConnectionEvent,
      currentUser,
      apiResources,
      externalPages,
      settings: defaultSettings as Partial<LayoutSettings>,
      customNavTheme: getNavTheme(),
      setCustomNavTheme: setNavTheme,
    };
  }
  return {
    fetchMenuData,
    fetchUserInfo,
    fetchApiResources,
    fetchExternalPages,
    settings: defaultSettings as Partial<LayoutSettings>,
    customNavTheme: getNavTheme(),
    setCustomNavTheme: setNavTheme,
  };
}

// ProLayout 支持的api https://procomponents.ant.design/components/layout
export const layout: RunTimeLayoutConfig = ({ initialState, setInitialState }) => {
  // console.log(initialState?.settings);
  // console.log(initialState?.customNavTheme);
  // @ts-ignore
  // window.__INJECTED_QIANKUN_MASTER_NAV_THEME__ = theme;
  return {
    actionsRender: () => [
      <TenantInfo key={'tenant'} />,
      <NavTheme key={'theme'} />,
      <Question key="doc" />,
      // <SelectLang key="SelectLang" />
    ],
    avatarProps: {
      src: initialState?.currentUser?.avatar,
      title: <AvatarName />,
      render: (_, avatarChildren) => {
        return <AvatarDropdown menu={true}>{avatarChildren}</AvatarDropdown>;
      },
    },
    menuDataRender(menuData) {
      return fixMenuItemIcon(menuData);
    },
    menu: {
      locale: false,
      params: initialState?.currentUser,
      request: async () => {
        let basicMenus: API.ResourceInfo[];
        if (initialState?.apiResources === undefined) {
          basicMenus = (await initialState?.fetchApiResources?.()) || [];
          setInitialState({
            ...initialState,
            apiResources: basicMenus,
          });
        } else {
          basicMenus = initialState?.apiResources;
        }
        // 读取外部页面列表
        let externalPages: API.ExternalPage[] = [];
        if (initialState?.externalPages === undefined) {
          externalPages = (await initialState?.fetchExternalPages?.()) || [];
        } else {
          externalPages = initialState?.externalPages;
        }
        // 排序
        const parentExternalPages = externalPages
          .filter((p) => p.parentId === null)
          .sort((a, b) => a.sort - b.sort);
        const externalMenus = [];
        for (let i = 0; i < parentExternalPages.length; i += 1) {
          const page = parentExternalPages[i];
          let info: API.ResourceInfo = {
            path: page.type === 2 ? `/external/${encodeURIComponent(page.path)}` : page.path,
            name: page.name,
            icon: page.icon,
            footerRender: false,
            isAuth: true,
            isVisible: true,
            sort: page.sort,
            code: page.id,
          };
          if (page.layout !== 'default') {
            info = {
              ...info,
              layout: page.layout,
            };
          }
          const children = externalPages
            .filter((p) => p.parentId === page.id)
            .sort((a, b) => a.sort - b.sort);
          if (children.length > 0) {
            info.children = [];
            for (let j = 0; j < children.length; j += 1) {
              const child = children[j];
              let childInfo: API.ResourceInfo = {
                path: child.type === 2 ? `/external/${encodeURIComponent(child.path)}` : child.path,
                name: child.name,
                icon: child.icon,
                footerRender: false,
                isAuth: true,
                isVisible: true,
                sort: child.sort,
                code: child.id,
              };
              if (child.layout !== 'default') {
                childInfo = {
                  ...childInfo,
                  layout: child.layout,
                };
              }
              info.children.push(childInfo);
            }
          }
          externalMenus.push(info);
        }
        return recursionMenu([...basicMenus, ...externalMenus]);
      },
    },
    waterMarkProps: {
      content: initialState?.currentUser?.realName,
    },
    footerRender: () => <Footer />,
    onPageChange: () => {
      const { location } = history;
      // console.log('location', location);
      // 如果没有登录，重定向到 login
      if (!initialState?.currentUser && location.pathname !== LOGIN_PATH) {
        history.push(LOGIN_PATH);
        return;
      }
      if (location.pathname !== '/') {
        // 添加访问记录
        addUserAccessRecord({ accessUrl: location.pathname });
      }
    },
    layoutBgImgList: [
      {
        src: 'https://mdn.alipayobjects.com/yuyan_qk0oxh/afts/img/D2LWSqNny4sAAAAAAAAAAAAAFl94AQBr',
        left: 85,
        bottom: 100,
        height: '303px',
      },
      {
        src: 'https://mdn.alipayobjects.com/yuyan_qk0oxh/afts/img/C2TWRpJpiC0AAAAAAAAAAAAAFl94AQBr',
        bottom: -68,
        right: -45,
        height: '303px',
      },
      {
        src: 'https://mdn.alipayobjects.com/yuyan_qk0oxh/afts/img/F6vSTbj8KpYAAAAAAAAAAAAAFl94AQBr',
        bottom: 0,
        left: 0,
        width: '331px',
      },
    ],
    links: isDev
      ? [
        <Link key="openapi" to="/umi/plugin/openapi" target="_blank">
          <LinkOutlined />
          <span>OpenAPI 文档</span>
        </Link>,
      ]
      : [],
    menuHeaderRender: undefined,
    // 自定义 403 页面
    // unAccessible: <div>unAccessible</div>,
    // 增加一个 loading 的状态
    childrenRender: (children) => {
      if (initialState?.loading) return <PageLoading />;
      return (
        <>
          {children}
          <SettingDrawer
            disableUrlParams
            enableDarkTheme
            settings={initialState?.settings}
            onSettingChange={(settings) => {
              setInitialState((preInitialState) => ({
                ...preInitialState,
                settings,
              }));
            }}
          />
        </>
      );
    },
    ...initialState?.settings,
    // 根据操作系统设置自动切换主题
    navTheme:
      initialState?.customNavTheme === undefined
        ? window.matchMedia('(prefers-color-scheme: dark)').matches
          ? 'realDark'
          : 'light'
        : initialState?.customNavTheme,
  };
};

/**
 * @name request 配置，可以配置错误处理
 * 它基于 axios 和 ahooks 的 useRequest 提供了一套统一的网络请求和错误处理方案。
 * @doc https://umijs.org/docs/max/request#配置
 */
export const request = {
  ...errorConfig,
};

export const qiankun = async () => {
  const res = await queryAppConfig();
  const config = res.data || {};
  return {
    ...config,
  };
  // const res = await queryApps();
  // const apps = res.success ? res.data || [] : [];
  // console.log(apps);
  // return {
  // 注册子应用信息
  // apps,
  // apps: [
  //   {
  //     name: 'xdxd',
  //     entry: '//localhost:5119',
  //     credentials: true
  //   },
  // ],
  // routes: [
  //   {
  //     path: '/app/xdxd/*',
  //     microApp: 'xdxd',
  //     microAppProps: {
  //       autoCaptureError: true,
  //       className: 'micro-app',
  //     },
  //   },
  // ],
  // lifeCycles: {
  //   // 所有子应用在挂载完成时，打印 props 信息
  //   async afterMount(props: any) {
  //     console.log(props);
  //   },
  // },
  // };
};
