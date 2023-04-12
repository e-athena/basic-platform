/**
 * @name umi 的路由配置
 * @description 只支持 path,component,routes,redirect,wrappers,name,icon 的配置
 * @param path  path 只支持两种占位符配置，第一种是动态参数 :id 的形式，第二种是 * 通配符，通配符只能出现路由字符串的最后。
 * @param component 配置 location 和 path 匹配后用于渲染的 React 组件路径。可以是绝对路径，也可以是相对路径，如果是相对路径，会从 src/pages 开始找起。
 * @param routes 配置子路由，通常在需要为多个路径增加 layout 组件时使用。
 * @param redirect 配置路由跳转
 * @param wrappers 配置路由组件的包装组件，通过包装组件可以为当前的路由组件组合进更多的功能。 比如，可以用于路由级别的权限校验
 * @param name 配置路由的标题，默认读取国际化文件 menu.ts 中 menu.xxxx 的值，如配置 name 为 login，则读取 menu.ts 中 menu.login 的取值作为标题
 * @param icon 配置路由的图标，取值参考 https://ant.design/components/icon-cn， 注意去除风格后缀和大小写，如想要配置图标为 <StepBackwardOutlined /> 则取值应为 stepBackward 或 StepBackward，如想要配置图标为 <UserOutlined /> 则取值应为 user 或者 User
 * @doc https://umijs.org/docs/guides/routes
 */
export default [
  {
    path: '/user',
    layout: false,
    routes: [
      {
        name: 'login',
        path: '/user/login',
        component: './User/Login',
      },
    ],
  },
  {
    layout: false,
    name: '跳转中',
    path: '/user/login-redirect',
    component: './User/LoginRedirect',
  },
  {
    name: '个人中心',
    path: '/user/center',
    component: './User/Center',
  },
  {
    name: '个人设置',
    path: '/user/settings',
    component: './User/Settings',
  },
  {
    path: '/',
    redirect: '/dashboard',
  },
  {
    path: '/dashboard',
    name: '工作台',
    icon: 'home',
    routes: [
      {
        path: '/dashboard',
        redirect: '/dashboard/workbench',
      },
      {
        path: '/dashboard/workbench',
        name: '工作台',
        icon: 'smile',
        component: './Welcome',
      },
    ],
  },
  {
    path: '/admin',
    name: 'admin',
    icon: 'crown',
    access: 'canAdmin',
    routes: [
      {
        path: '/admin',
        redirect: '/admin/sub-page',
      },
      {
        path: '/admin/sub-page',
        name: 'sub-page',
        component: './Admin',
      },
    ],
  },
  {
    path: '/developer',
    name: '开发者中心',
    icon: 'crown',
    routes: [
      {
        path: '/developer',
        redirect: '/developer/resource',
      },
      {
        path: '/developer/resource',
        name: '资源管理',
        component: './Developer/Resource',
        access: 'routeFilter',
      },
      {
        path: '/developer/app',
        name: '应用管理',
        component: './Developer/Application',
        access: 'routeFilter',
      },
    ],
  },
  {
    path: '/system',
    name: '系统管理',
    icon: 'crown',
    routes: [
      {
        path: '/system',
        redirect: '/system/organization',
      },
      {
        path: '/system/organization',
        name: '组织架构',
        component: './System/Organization',
        access: 'routeFilter',
      },
      {
        path: '/system/position',
        name: '职位管理',
        component: './System/Position',
        access: 'routeFilter',
      },
      {
        path: '/system/role',
        name: '角色管理',
        component: './System/Role',
        access: 'routeFilter',
      },
      {
        path: '/system/user',
        name: '用户管理',
        component: './System/User',
        access: 'routeFilter',
      },
      {
        path: '/system/user-access-record',
        name: '员工访问记录',
        component: './System/UserAccessRecord',
        access: 'routeFilter',
      },
      {
        path: '/system/server-info',
        name: '服务器信息',
        component: './System/ServerInfo',
        access: 'routeFilter',
      },
      {
        path: '/system/external-page',
        name: '外部页面',
        component: './System/ExternalPage',
        access: 'routeFilter',
      },
    ],
  },
  {
    path: '/external/:id',
    component: './ExternalPages',
  },
  {
    path: '*',
    layout: false,
    component: './404',
  },
];
