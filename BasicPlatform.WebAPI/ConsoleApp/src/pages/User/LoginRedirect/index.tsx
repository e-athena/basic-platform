import React, { useEffect, useState } from 'react';
import { PageLoading } from '@ant-design/pro-components';
import { history } from '@umijs/max';
import { parse } from 'querystring';
import { getSessionCode } from '@/utils/token';
import { getAuthCode } from '@/services/ant-design-pro/api';

const LoginRedirect: React.FC = () => {
  const query = parse(history.location.search.split('?')[1], '&');
  const [handling, setHandling] = useState<boolean>(true);

  useEffect(() => {
    const fetch = async () => {
      // 读取sessionCode
      const sessionCode = getSessionCode();
      if (sessionCode) {
        const res = await getAuthCode({
          clientId: query?.clientId as string,
          sessionCode,
        });
        if (res.success) {
          const redirectUrl = query!.redirectUrl as string;
          if (redirectUrl !== undefined && redirectUrl?.includes('?')) {
            let url = redirectUrl.split("?")[0];
            let param = redirectUrl.split("?")[1];
            window.location.href = `${url}?authCode=${res.data}&sessionCode=${res.data}&source=sso&${param}`;
            return;
          }
          window.location.href = `${redirectUrl}?authCode=${res.data}&sessionCode=${res.data}&source=sso`;
        }
      }
      setHandling(false);
    }
    if (query?.clientId && query?.redirectUrl && handling) {
      fetch();
    }
  }, [query?.clientId]);

  useEffect(() => {
    if (!handling) {
      history.push(`/user/login${history.location.search}`);
    }
  }, [handling]);

  return (
    <div>
      <PageLoading tip={"页面跳转中..."} />
    </div>
  );
};

export default LoginRedirect;