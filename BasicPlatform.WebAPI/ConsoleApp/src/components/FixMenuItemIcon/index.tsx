import React from 'react';
import { MenuDataItem } from '@ant-design/pro-components';
import FixIcon from '../FixIcon';

// FIX从接口获取菜单时icon为string类型
const fixMenuItemIcon = (menus: MenuDataItem[], iconType = 'Outlined'): MenuDataItem[] => {
  menus.forEach((item) => {
    const { icon, children } = item;
    if (typeof icon === 'string') {
      // @ts-ignore
      item.icon = <FixIcon name={icon} type={iconType} />;
    }
    if (children && children.length > 0) {
      item.children = fixMenuItemIcon(children);
    }
  });
  return menus;
};

export default fixMenuItemIcon;
