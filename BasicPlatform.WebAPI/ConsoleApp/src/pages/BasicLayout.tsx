import FixIcon from '@/components/FixIcon';
import { ProCard } from '@ant-design/pro-components';
import { useLocation, Outlet, useModel, history } from '@umijs/max';
import React from 'react';

const Admin: React.FC = () => {
  const { initialState } = useModel('@@initialState');
  const location = useLocation();

  const resource = (initialState?.apiResources || []).find(p => p.path === location.pathname);
  if (resource === undefined) {
    return <Outlet />
  }

  return (<ProCard ghost gutter={16}>
    {resource.children?.map(p => (
      <ProCard
        key={p.path}
        colSpan={6}
        style={{ cursor: 'pointer' }}
        actions={[
          // @ts-ignore
          <FixIcon key={'icon'} name={p.icon} />
        ]}
        hoverable
        title={p.name}
        onClick={() => {
          history.push(p.path);
        }}
      >
        {p.description || '无描述'}
      </ProCard>
    ))}
  </ProCard>);

};

export default Admin;
