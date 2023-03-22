import React, { useEffect, useState, forwardRef, useImperativeHandle } from 'react';
import { Skeleton, Tree } from 'antd';
import { treeList } from '../service';

export type ExternalPageTreeInstance = {
  reload: () => void;
};
type ExternalPageTreeProps = {
  onSelect?: (key: string | null) => void;
};
const App = forwardRef((props: ExternalPageTreeProps, forwardedRef: any) => {
  const [dataSource, setDataSource] = useState<API.TreeInfo[]>();
  const [loading, setLoading] = useState<boolean>(true);
  useEffect(() => {
    const fetch = async () => {
      const res = await treeList();
      setDataSource(res.success ? res.data! : []);
      setLoading(false);
    };
    if (loading) {
      fetch();
    }
  }, [loading]);

  useImperativeHandle(forwardedRef, () => {
    return {
      reload: () => {
        setLoading(true);
      },
    };
  });

  return (
    <div style={{ margin: 10 }}>
      {loading ? (
        <Skeleton active />
      ) : (
        <Tree
          showLine={{
            showLeafIcon: false,
          }}
          // showIcon={true}
          defaultExpandAll={true}
          onSelect={(selectedKeys: React.Key[]) => {
            props.onSelect?.(selectedKeys.length > 0 ? (selectedKeys[0] as string) : null);
          }}
          treeData={dataSource}
        />
      )}
    </div>
  );
});

export default App;
