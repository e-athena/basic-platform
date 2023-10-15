import { AlertFilled, AlertOutlined, QuestionCircleOutlined } from '@ant-design/icons';
import { SelectLang as UmiSelectLang, useModel } from '@umijs/max';
import { useLocalStorageState } from 'ahooks';
import { Switch, Tag, message } from 'antd';
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

export const TenantInfo = () => {
  const [tenantInfo] = useLocalStorageState<TenantInfo | undefined>(APP_TENANT_INFO_KEY);
  if (!tenantInfo) {
    return null;
  }
  return (
    <Tag color="purple">{tenantInfo.name}</Tag>
  );
}

export const NavTheme = () => {
  const { initialState, setInitialState } = useModel('@@initialState');
  // @ts-ignore
  window.__INJECTED_QIANKUN_MASTER_NAV_THEME__ = initialState?.customNavTheme;
  let checked = false;
  const systemNavTheme = window.matchMedia('(prefers-color-scheme: dark)').matches
    ? 'realDark'
    : 'light';
  if (initialState?.customNavTheme === undefined) {
    checked = systemNavTheme !== 'light';
  } else {
    checked = initialState?.customNavTheme !== 'light';
  }
  return (
    <div
      style={{
        display: 'flex',
        padding: 0,
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
          if (window.location.href.includes('/app/')) {
            message.success('切换完成，页面将重新加载...');
            // @ts-ignore
            window.__INJECTED_QIANKUN_MASTER_NAV_THEME__ = theme;
            setTimeout(() => {
              window.location.reload();
            }, 1000);
          }
        }}
      />
    </div>
  );
};
