import React, { ReactNode } from 'react';
import * as AllIcons from '@ant-design/icons';

type FixIconProps = {
  name: string;
  type?: string;
};

const FixIcon = (props: FixIconProps): ReactNode => {
  const { name, type = 'Outlined' } = props;
  let fixIconName = name.slice(0, 1).toLocaleUpperCase() + name.slice(1) + type;
  // @ts-ignore
  return React.createElement(AllIcons[fixIconName] || AllIcons[name]);
};

export default FixIcon;
