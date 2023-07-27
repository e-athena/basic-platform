import { PageContainer } from '@ant-design/pro-components';
import { useParams } from '@umijs/max';
import { Button, Spin } from 'antd';
import React, { useEffect, useState } from 'react';
import Iframe from 'react-iframe';
import { useRafState } from 'ahooks';
import { LinkOutlined } from '@ant-design/icons';
import './index.less';

const ExternalPages: React.FC = () => {
  const params = useParams<{ url: string }>();

  const [state, setState] = useRafState({
    width: 0,
    height: 0,
  });
  useEffect(() => {
    const onResize = () => {
      setState({
        width: document.documentElement.clientWidth,
        height: document.documentElement.clientHeight,
      });
    };
    onResize();

    window.addEventListener('resize', onResize);

    return () => {
      window.removeEventListener('resize', onResize);
    };
  }, []);
  const [loading, setLoading] = useState<boolean>(true);
  const [lastUrl, setLastUrl] = useState<string>();
  useEffect(() => {
    if (params.url !== lastUrl) {
      setLoading(true);
    }
  }, [params.url, lastUrl]);

  return (
    <>
      <PageContainer
        extra={
          <Button
            icon={<LinkOutlined />}
            onClick={() => {
              // 新窗口打开
              window.open(params.url!);
            }}
          >
            新窗口打开
          </Button>
        }
        prefixCls={'athena-external-pages'}
      >
        <Spin spinning={loading}>
          <Iframe
            url={params.url!}
            width="100%"
            height={`${state.height - 64}px`}
            // id="iframe"
            className=""
            sandbox={['allow-same-origin', 'allow-scripts']}
            display="block"
            position="relative"
            styles={{ border: 'none' }}
            allowFullScreen
            onLoad={() => {
              setLoading(false);
              setLastUrl(params.url!);
            }}
          />
        </Spin>
      </PageContainer>
    </>
  );
};

export default ExternalPages;
