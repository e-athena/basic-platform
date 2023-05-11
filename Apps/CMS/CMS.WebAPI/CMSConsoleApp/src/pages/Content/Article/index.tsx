import { query, detail, publish } from './service';
import { PlusOutlined, FormOutlined } from '@ant-design/icons';
import type { ActionType, ProColumns } from '@ant-design/pro-components';
import { PageContainer } from '@ant-design/pro-components';
import { useModel, useLocation, Access } from '@umijs/max';
import { Button, Modal, Switch } from 'antd';
import React, { useRef, useState } from 'react';
import IconStatus from '@/components/IconStatus';
import permission from '@/utils/permission';
import { canAccessible, hasPermission, queryDetail, submitHandle } from '@/utils/utils';
import CreateOrUpdateForm from './components/CreateOrUpdateForm';
import ProTablePlus from '@/components/ProTablePlus';

const TableList: React.FC = () => {
  const [createOrUpdateModalOpen, handleCreateOrUpdateModalOpen] = useState<boolean>(false);

  const actionRef = useRef<ActionType>();
  const [currentRow, setCurrentRow] = useState<API.ArticleInfo>();
  const { getResource } = useModel('resource');
  const location = useLocation();
  const resource = getResource(location.pathname);
  const showOption: boolean = hasPermission(
    [permission.article.putAsync, permission.article.postAsync],
    resource,
  );

  /**需要重写的Column */
  const [defaultColumns] = useState<ProColumns<API.ArticleListItem>[]>([
    {
      title: '状态',
      dataIndex: 'isPublish',
      width: 90,
      hideInSearch: true,
      align: 'center',
      sorter: true,
      render(_, entity) {
        if (canAccessible(permission.article.publishAsync, resource)) {
          return (
            <Switch
              checkedChildren="已发布"
              unCheckedChildren="待发布"
              checked={entity.isPublish}
              onClick={async (_, e) => {
                e.stopPropagation();
                const name = entity.isPublish ? '撤销发布' : '发布';
                Modal.confirm({
                  title: '操作提示',
                  content: `确定${name}{${entity.title}}吗？`,
                  onOk: async () => {
                    const succeed = await submitHandle(publish, entity.id!, name);
                    if (succeed) {
                      actionRef.current?.reload();
                    }
                  },
                });
              }}
            />
          );
        }
        return <IconStatus status={entity.isPublish || false} />;
      },
    },
    {
      title: '操作',
      dataIndex: 'option',
      valueType: 'option',
      hideInTable: !showOption,
      width: 125,
      render(_, entity) {
        return [
          <Access
            key={'edit'}
            accessible={canAccessible(permission.article.putAsync, resource)}
          >
            <Button
              shape="circle"
              type={'link'}
              icon={<FormOutlined />}
              onClick={async (e) => {
                e.stopPropagation();
                const data = await queryDetail(detail, entity.id!);
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
            accessible={canAccessible(permission.article.postAsync, resource)}
          >
            <Button
              shape="circle"
              type={'link'}
              onClick={async (e) => {
                e.stopPropagation();
                const data = await queryDetail(detail, entity.id!);
                if (data) {
                  data.id = undefined;
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
      <ProTablePlus<API.ArticleListItem, API.ArticlePagingParams>
        actionRef={actionRef}
        defaultColumns={defaultColumns}
        query={query}
        moduleName={'Article'}
        toolBarRender={() => [
          <Access
            key={'add'}
            accessible={canAccessible(permission.article.postAsync, resource)}
          >
            <Button
              type="primary"
              onClick={() => {
                setCurrentRow(undefined);
                handleCreateOrUpdateModalOpen(true);
              }}
              icon={<PlusOutlined />}
            >
              新建
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
