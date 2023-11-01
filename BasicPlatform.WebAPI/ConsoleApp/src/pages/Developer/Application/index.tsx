import { query, detail, statusChange, getEnvironmentList } from './service';
import { PlusOutlined, FormOutlined } from '@ant-design/icons';
import type { ActionType, ProColumns } from '@ant-design/pro-components';
import { PageContainer } from '@ant-design/pro-components';
import { FormattedMessage, useModel, useLocation, Access } from '@umijs/max';
import { Button, Modal, Segmented, Switch, message } from 'antd';
import React, { useEffect, useRef, useState } from 'react';
import permission from '@/utils/permission';
import { canAccessible, hasPermission, queryDetail } from '@/utils/utils';
import CreateOrUpdateForm from './components/CreateOrUpdateForm';
import ProTablePlus from '@/components/ProTablePlus';
import IconStatus from '@/components/IconStatus';

const TableList: React.FC = () => {
  const [createOrUpdateModalOpen, handleCreateOrUpdateModalOpen] = useState<boolean>(false);

  const actionRef = useRef<ActionType>();
  const [currentRow, setCurrentRow] = useState<API.ApplicationDetailItem>();
  const { getResource } = useModel('resource');
  const location = useLocation();
  const resource = getResource(location.pathname);
  const showOption: boolean = hasPermission([permission.application.putAsync], resource);
  const [environmentOptions, setEnvironmentOptions] = useState<string[]>([]);
  const [currentEnvironment, setCurrentEnvironment] = useState<string>('');
  const [environmentOptionsLoading, setEnvironmentOptionsLoading] = useState<boolean>(true);

  useEffect(() => {
    const fetch = async () => {
      setEnvironmentOptionsLoading(false);
      const res = await getEnvironmentList();
      const options = res.data || ['Development', 'Test', 'Production'];
      // 将'All'放在第一位
      options.unshift('All');
      setEnvironmentOptions(options);
      setCurrentEnvironment('All');
    }
    if (environmentOptionsLoading) {
      fetch();
    }
  }, [environmentOptionsLoading]);

  /**需要重写的Column */
  const [defaultColumns] = useState<ProColumns<API.ApplicationListItem>[]>([
    {
      dataIndex: 'frontendUrl',
      render: (_, entity) => {
        return entity.frontendUrl ? (
          <a target={'_blank'} href={entity.frontendUrl} rel="noreferrer">
            {entity.frontendUrl}
          </a>
        ) : null;
      },
    },
    {
      title: '状态',
      dataIndex: 'status',
      width: 90,
      hideInSearch: true,
      align: 'center',
      sorter: true,
      render(_, entity) {
        if (canAccessible(permission.application.statusChangeAsync, resource)) {
          return (
            <Switch
              checkedChildren="启用"
              unCheckedChildren="禁用"
              checked={entity.status === 1}
              onClick={async (_, e) => {
                e.stopPropagation();
                const statusName = entity.status === 1 ? '禁用' : '启用';
                Modal.confirm({
                  title: '操作提示',
                  content: `确定${statusName}{${entity.name}}吗？`,
                  onOk: async () => {
                    const hide = message.loading(`正在${statusName}`, 0);
                    const res = await statusChange(entity.id!);
                    hide();
                    if (res.success) {
                      actionRef.current?.reload();
                      return;
                    }
                    message.error(res.message);
                  },
                });
              }}
            />
          );
        }
        return <IconStatus status={entity.status === 1} />;
      },
    },
    {
      title: '操作',
      dataIndex: 'option',
      valueType: 'option',
      hideInTable: !showOption,
      width: 140,
      render(_, entity) {
        return [
          <Access
            key={'edit'}
            accessible={canAccessible(permission.application.putAsync, resource)}
          >
            <Button
              size="small"
              icon={<FormOutlined />}
              onClick={async (e) => {
                e.stopPropagation();
                const data = await queryDetail(detail, entity.id);
                if (data) {
                  setCurrentRow(data);
                  handleCreateOrUpdateModalOpen(true);
                }
              }}
            >
              编辑
            </Button>
          </Access>,
          <Access
            key={'copy'}
            accessible={canAccessible(permission.application.postAsync, resource)}
          >
            <Button
              size="small"
              onClick={async (e) => {
                e.stopPropagation();
                const data = await queryDetail(detail, entity.id!);
                if (data) {
                  data.id = undefined;
                  data.clientSecret = undefined;
                  setCurrentRow(data);
                  handleCreateOrUpdateModalOpen(true);
                }
              }}
            >
              复制
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
        headerTitle={<Segmented
          options={environmentOptions}
          value={currentEnvironment}
          onChange={(value) => {
            setCurrentEnvironment(value.toString());
          }} />}
        actionRef={actionRef}
        defaultColumns={defaultColumns}
        query={query}
        moduleName={'Application'}
        showDescriptions
        params={{
          environment: currentEnvironment === 'All' ? undefined : currentEnvironment,
        }}
        toolBarRender={() => [
          <Access
            key={'add'}
            accessible={canAccessible(permission.application.postAsync, resource)}
          >
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
