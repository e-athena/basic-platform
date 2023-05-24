import { useModel, history } from '@umijs/max';

export default () => {
  const { initialState } = useModel('@@initialState');

  const getResource = (pathname?: string): API.ResourceInfo | null => {
    const path = pathname || history.location.pathname;
    let resource: API.ResourceInfo | null = null;
    if (initialState?.apiResources) {
      const list = initialState.apiResources[0].children || [];
      // 从子级中读取对应的菜单信息
      for (let i = 0; i < list.length; i++) {
        const module = list[i];
        const item = module.children?.find((p) => p.path === path);
        if (item) {
          resource = item;
          break;
        }
      }
    }
    return resource;
  };

  return { getResource };
};
