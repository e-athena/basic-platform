import { PageContainer } from '@ant-design/pro-components';
import { useParams } from '@umijs/max';
import { Button, Spin } from 'antd';
import React, { useEffect, useState } from 'react';
import Iframe from 'react-iframe';
import { useRafState } from 'ahooks';
import { LinkOutlined } from '@ant-design/icons';

const App: React.FC = () => {
  const url = useParams<{ id: string }>();

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
  const [loading, setLoading] = useState(true);
  const [lastUrl, setLastUrl] = useState<string>();
  useEffect(() => {
    if (url.id !== lastUrl) {
      setLoading(true);
    }
  }, [url.id, lastUrl]);

  return (<>
    <PageContainer extra={
      <Button icon={< LinkOutlined />} onClick={() => {
        // 新窗口打开
        window.open(url.id!);
      }}>新窗口打开</Button>
    }>
      <Spin spinning={loading}>
        <Iframe
          url={url.id!}
          width="100%"
          height={`${state.height - 90}px`}
          // id="iframe"
          className=""
          sandbox={["allow-same-origin", "allow-scripts"]}
          display="block"
          position="relative"
          styles={{ border: 'none' }}
          allowFullScreen
          onLoad={() => {
            setLoading(false);
            setLastUrl(url.id!);
          }}
        />
      </Spin>
    </PageContainer>
  </>);
};

export default App;
