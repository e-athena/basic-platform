import React, { useEffect, useState } from 'react';
import { PageLoading } from '@ant-design/pro-components';
import { history, useModel } from '@umijs/max';
import { parse } from 'querystring';
import { setToken } from '@/utils/token';
import { flushSync } from 'react-dom';
import { isQiankun } from '@/utils/utils';

const gotoSSOAuth = () => {
  const urlParams = parse(window.location.href.split('login-redirect?')[1], '&');
  const redirectUrl = encodeURIComponent(window.location.href.replace('&target=logout', ''));
  const url = `${SSO_URL}?clientId=${APPID}&redirectUrl=${redirectUrl}&target=${
    urlParams.target || ''
  }`;
  window.location.href = url;
};

const LoginRedirect: React.FC = () => {
  const query = parse(history.location.search.split('?')[1], '&');
  const [handling, setHandling] = useState<boolean>(true);
  const { initialState, setInitialState } = useModel('@@initialState');

  const fetchUserInfo = async () => {
    const userInfo = await initialState?.fetchUserInfo?.();
    if (userInfo) {
      flushSync(() => {
        setInitialState((s) => ({
          ...s,
          currentUser: userInfo,
        }));
      });
    }
  };
  useEffect(() => {
    const fetch = async () => {
      setToken(query.token as string);
      await fetchUserInfo();
      let redirect = (query?.redirect as string) || '/';
      if (isQiankun()) {
        redirect = '/';
      }
      history.push(redirect);
      setHandling(false);
    };
    if (query?.token && handling) {
      fetch();
    } else {
      gotoSSOAuth();
    }
  }, [query?.token]);

  return (
    <div>
      <PageLoading tip={'页面跳转中...'} />
    </div>
  );
};

export default LoginRedirect;
