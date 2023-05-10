/**
 * @see https://umijs.org/zh-CN/plugins/plugin-access
 * */
export default function access(
  initialState:
    | {
        currentUser?: API.CurrentUser;
        apiResources?: API.ResourceInfo[];
      }
    | undefined,
) {
  const { currentUser, apiResources } = initialState ?? {};
  return {
    canAdmin: currentUser && currentUser.userName === 'root',
    routeFilter: (route: RouteInfo) => {
      if (currentUser?.userName === 'root') {
        return true;
      }
      // 是否有权限访问
      if (apiResources && apiResources.length > 0) {
        let hasPermission = false;
        const resource = apiResources[0];
        // 从子级中读取对应的菜单信息
        for (let i = 0; i < (resource.children || []).length; i++) {
          const module = (resource.children || [])[i];
          const item = module.children?.find((p) => p.path === route.path);
          if (item) {
            hasPermission = true;
            break;
          }
        }
        return hasPermission;
      }
      return false;
    },
  };
}

type RouteInfo = {
  path: string;
  name: string;
};
