import { query, detail } from './service';
import {
  PlusOutlined,
  FormOutlined,
} from '@ant-design/icons';
import type { ActionType, ProColumns } from '@ant-design/pro-components';
import { PageContainer } from '@ant-design/pro-components';
import { FormattedMessage, useModel, useLocation, Access } from '@umijs/max';
import { Button, message } from 'antd';
import React, { useRef, useState } from 'react';
import permission from '@/utils/permission';
import { canAccessible, hasPermission } from '@/utils/utils';
import CreateOrUpdateForm from './components/CreateOrUpdateForm';
import ProTablePlus from '@/components/ProTablePlus';

const TableList: React.FC = () => {
  const [createOrUpdateModalOpen, handleCreateOrUpdateModalOpen] = useState<boolean>(false);

  const actionRef = useRef<ActionType>();
  const [currentRow, setCurrentRow] = useState<API.ApplicationDetailItem>();
  const { getResource } = useModel('resource');
  const location = useLocation();
  const resource = getResource(location.pathname);
  const showOption: boolean = hasPermission(
    [
      permission.application.putAsync,
    ],
    resource,
  );

  /**需要重写的Column */
  const [defaultColumns] = useState<ProColumns<API.ApplicationListItem>[]>([
    {
      dataIndex: 'frontendUrl',
      render: (_, entity) => {
        return entity.frontendUrl ? (
          <a target={'_blank'} href={entity.frontendUrl} rel="noreferrer">{entity.frontendUrl}</a>
        ) : null;
      }
    },
    {
      title: '操作',
      dataIndex: 'option',
      valueType: 'option',
      hideInTable: !showOption,
      width: 95,
      render(_, entity) {
        return [
          <Access key={'edit'} accessible={canAccessible(permission.application.putAsync, resource)}>
            <Button
              shape="circle"
              type={'link'}
              icon={<FormOutlined />}
              onClick={async () => {
                const hide = message.loading('正在查询', 0);
                const res = await detail(entity.id);
                hide();
                if (res.success) {
                  setCurrentRow(res.data);
                  handleCreateOrUpdateModalOpen(true);
                  return;
                }
                message.error(res.message);
              }}
            >
              编辑
            </Button>
          </Access>,
        ];
      },
    },
  ]);

  return (
    <PageContainer
      header={{
        title: resource?.name,
        children: resource?.description,
      }}
    >
      <ProTablePlus<API.ApplicationListItem, API.ApplicationPagingParams>
        actionRef={actionRef}
        defaultColumns={defaultColumns}
        query={query}
        moduleName={'Application'}
        toolBarRender={() => [
          <Access key={'add'} accessible={canAccessible(permission.application.postAsync, resource)}>
            <Button
              type="primary"
              onClick={() => {
                setCurrentRow(undefined);
                handleCreateOrUpdateModalOpen(true);
              }}
              icon={<PlusOutlined />}
            >
              <FormattedMessage id="pages.searchTable.new" defaultMessage="New" />
            </Button>
          </Access>,
        ]}
      />
      <CreateOrUpdateForm
        onCancel={() => {
          handleCreateOrUpdateModalOpen(false);
          setCurrentRow(undefined);
        }}
        onSuccess={() => {
          handleCreateOrUpdateModalOpen(false);
          setCurrentRow(undefined);
          if (actionRef.current) {
            actionRef.current.reload();
          }
        }}
        open={createOrUpdateModalOpen}
        values={currentRow}
      />
    </PageContainer>
  );
};

export default TableList;
