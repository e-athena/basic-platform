import React from 'react';
import * as AllIcons from '@ant-design/icons';
import { MenuDataItem } from '@ant-design/pro-components';

// FIX从接口获取菜单时icon为string类型
const fixMenuItemIcon = (menus: MenuDataItem[], iconType = 'Outlined'): MenuDataItem[] => {
  menus.forEach((item) => {
    const { icon, children } = item;
    if (typeof icon === 'string') {
      let fixIconName = icon.slice(0, 1).toLocaleUpperCase() + icon.slice(1) + iconType;
      // @ts-ignore
      item.icon = React.createElement(AllIcons[fixIconName] || AllIcons[icon]);
    }
    if (children && children.length > 0) {
      item.children = fixMenuItemIcon(children)
    }
  });
  return menus;
};

export default fixMenuItemIcon;