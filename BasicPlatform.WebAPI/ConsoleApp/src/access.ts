import { hasMenuPermission } from "./utils/utils";

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
      return hasMenuPermission(route.path, apiResources ? apiResources[0]?.children : null);
    },
  };
}

type RouteInfo = {
  path: string;
  name: string;
};
