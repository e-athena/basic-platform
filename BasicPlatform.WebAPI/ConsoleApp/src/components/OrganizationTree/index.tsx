import React, { useEffect, useState } from 'react';
import { Skeleton, Tree } from 'antd';
import { orgTree } from '@/services/ant-design-pro/system/org';

type OrganizationTreeProps = {
  onSelect?: (key: string | null) => void;
};
const App: React.FC<OrganizationTreeProps> = (props) => {
  const [dataSource, setDataSource] = useState<API.TreeInfo[]>();
  const [loading, setLoading] = useState<boolean>(true);
  useEffect(() => {
    const fetch = async () => {
      const res = await orgTree();
      setDataSource(res.success ? res.data! : []);
      setLoading(false);
    }
    if (loading) {
      fetch();
    }
  }, [loading]);

  return (
    <div style={{ margin: 10 }}>
      {loading ? <Skeleton active /> :
        <Tree
          showLine={{
            showLeafIcon: false
          }}
          showIcon={false}
          defaultExpandAll={true}
          onSelect={(selectedKeys: React.Key[]) => {
            props.onSelect?.(selectedKeys.length > 0 ? selectedKeys[0] as string : null);
          }}
          treeData={dataSource}
        />}
    </div>
  );
};

export default App;