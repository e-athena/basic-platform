import { MenuDataItem } from '@ant-design/pro-components';

/**
 * 递归处理菜单
 * @param menu
 * @returns
 */
export const recursionMenu = (menu: API.ResourceInfo[]): MenuDataItem[] => {
  return menu.map((item) => {
    if (item.children) {
      return {
        ...item,
        path: item.path.toLowerCase(),
        name: item.name,
        icon: item.icon,
        hideInMenu: !item.isVisible,
        children: recursionMenu(item.children),
      };
    }
    return {
      ...item,
      icon: item.icon,
      path: item.path.toLowerCase(),
      name: item.name,
      hideInMenu: !item.isVisible,
    };
  });
};
