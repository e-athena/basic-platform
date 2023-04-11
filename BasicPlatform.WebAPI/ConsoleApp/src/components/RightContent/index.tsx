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
        window.open('https://gitee.com/e-athena/basic-platform');
      }}
    >
      <QuestionCircleOutlined />
    </div>
  );
};

export const NavTheme = () => {
  const { initialState, setInitialState } = useModel('@@initialState');
  let checked = false;
  const systemNavTheme = window.matchMedia('(prefers-color-scheme: dark)').matches ? 'realDark' : 'light';
  if (initialState?.customNavTheme === undefined) {
    checked = systemNavTheme !== 'light';
  } else {
    checked = initialState?.customNavTheme !== 'light';
  }
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
        checked={checked}
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
