import React, { useEffect, useState, forwardRef, useImperativeHandle } from 'react';
import { Skeleton, Tree } from 'antd';
import { orgTree } from '@/services/ant-design-pro/system/org';

export type OrganizationTreeInstance = {
  reload: () => void;
};
type OrganizationTreeProps = {
  onSelect?: (key: string | null) => void;
  maxHeight?: number;
};
const OrganizationTree = forwardRef((props: OrganizationTreeProps, forwardedRef: any) => {
  const [dataSource, setDataSource] = useState<API.TreeInfo[]>();
  const [loading, setLoading] = useState<boolean>(true);
  useEffect(() => {
    const fetch = async () => {
      const res = await orgTree();
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
          // blockNode
          height={props.maxHeight}
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

export default OrganizationTree;
