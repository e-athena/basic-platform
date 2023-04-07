import { AlertFilled, AlertOutlined, QuestionCircleOutlined } from '@ant-design/icons';
import { SelectLang as UmiSelectLang, useModel } from '@umijs/max';
import { Switch } from 'antd';
import React from 'react';

export type SiderTheme = 'light' | 'dark';

export const SelectLang = () => {
  return (
    <UmiSelectLang
      style={{
        padding: 4,
      }}
    />
  );
};

export const Question = () => {
  return (
    <div
      style={{
        display: 'flex',
        height: 26,
      }}
      onClick={() => {
        window.open('https://pro.ant.design/docs/getting-started');
      }}
    >
      <QuestionCircleOutlined />
    </div>
  );
};

export const NavTheme = () => {
  const { initialState, setInitialState } = useModel('@@initialState');
  return (
    <div
      style={{
        display: 'flex',
        padding: 0
      }}
    >
      <Switch
        checkedChildren={<AlertFilled />}
        unCheckedChildren={<AlertOutlined />}
        checked={initialState?.customNavTheme !== 'light'}
        onChange={(checked) => {
          const theme = checked ? 'realDark' : 'light';
          initialState?.setCustomNavTheme?.(theme);
          setInitialState({
            ...initialState,
            customNavTheme: theme,
          });
        }}
      />
    </div>
  );
};
