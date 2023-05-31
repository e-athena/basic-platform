import { useLocation, Outlet, useModel, history, useOutlet } from '@umijs/max';
import { Button } from 'antd';
import Result from 'antd/es/result';
import React from 'react';

function BasicLayout() {
  const { initialState } = useModel('@@initialState');
  const location = useLocation();
  const resource = (initialState?.apiResources || []).find((p) => p.path === location.pathname);
  const outlet = useOutlet();

  if (outlet) {
    return <Outlet />;
  }

  // 模块下的第一个子路由
  const firstPath = resource?.children?.filter(c => c.isVisible)?.[0].path;
  // 检查是否有子路由
  if (firstPath) {
    // 跳转至第一个子路由
    history.push(firstPath);
    return;
  }

  return (
    <>
      <Result
        status="403"
        title="403"
        subTitle="对不起，您没有访问这个页面的权限。"
        extra={
          <Button
            type="primary"
            onClick={() => {
              history.push('/');
            }}
          >
            去主页
          </Button>
        }
      />
    </>
  );
}

export default BasicLayout;
