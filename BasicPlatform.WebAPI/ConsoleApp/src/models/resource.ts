import { useModel } from '@umijs/max';

export default () => {
  const { initialState } = useModel('@@initialState');

  const getResource = (pathname: string): (API.ResourceInfo | null) => {
    let resource: API.ResourceInfo | null = null;
    if (initialState?.apiResources) {
      // 从子级中读取对应的菜单信息
      for (let i = 0; i < initialState.apiResources.length; i++) {
        const module = initialState.apiResources[i];
        const item = module.children?.find(p => p.path === pathname);
        if (item) {
          resource = item;
          break;
        }
      }
    }
    return resource;
  }

  return { getResource };
};