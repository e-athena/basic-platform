import React, { useEffect, useState, forwardRef, useImperativeHandle } from 'react';
import { Skeleton, Tree } from 'antd';
import { queryTreeList } from '@/services/ant-design-pro/basics/region';

export type RegionTreeInstance = {
  reload: () => void;
};
type RegionTreeProps = {
  onSelect?: (key: string | null) => void;
  defaultExpandAll?: boolean;
};
const RegionTree = forwardRef((props: RegionTreeProps, forwardedRef: any) => {
  const [dataSource, setDataSource] = useState<API.TreeSelectInfo[]>();
  const [loading, setLoading] = useState<boolean>(true);
  useEffect(() => {
    const fetch = async () => {
      const res = await queryTreeList();
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
          defaultExpandAll={props.defaultExpandAll}
          onSelect={(selectedKeys: React.Key[]) => {
            props.onSelect?.(selectedKeys.length > 0 ? (selectedKeys[0] as string) : null);
          }}
          treeData={dataSource}
        />
      )}
    </div>
  );
});

export default RegionTree;
