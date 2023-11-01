import { query, detail, statusChange } from './service';
import { PlusOutlined, FormOutlined } from '@ant-design/icons';
import { ActionType, ProCard, ProColumns } from '@ant-design/pro-components';
import { PageContainer } from '@ant-design/pro-components';
import { FormattedMessage, useModel, useLocation, Access } from '@umijs/max';
import { Button, message, Modal, Switch } from 'antd';
import React, { useRef, useState } from 'react';
import IconStatus from '@/components/IconStatus';
import permission from '@/utils/permission';
import { canAccessible, hasPermission, queryDetail } from '@/utils/utils';
import CreateOrUpdateForm from './components/CreateOrUpdateForm';
import OrganizationTree from '@/components/OrganizationTree';
import { useSize } from 'ahooks';
import ProTablePlus from '@/components/ProTablePlus';

const TableList: React.FC = () => {
  const [createOrUpdateModalOpen, handleCreateOrUpdateModalOpen] = useState<boolean>(false);

  // 获取card的宽度，用于计算表格的宽度
  const tableSize = useSize(document.getElementsByClassName('ant-pro-card-body')[0]);
  const actionRef = useRef<ActionType>();
  const [currentRow, setCurrentRow] = useState<API.PositionDetailItem>();
  // const [selectedRowsState, setSelectedRows] = useState<API.PositionListItem[]>([]);
  const { getResource } = useModel('resource');
  const location = useLocation();
  const resource = getResource(location.pathname);
  const showOption: boolean = hasPermission([permission.position.putAsync], resource);

  const [defaultColumns] = useState<ProColumns<API.PositionListItem>[]>([
    {
      title: '状态',
      dataIndex: 'status',
      width: 90,
      hideInSearch: true,
      align: 'center',
      sorter: true,
      render(_, entity) {
        if (canAccessible(permission.position.statusChangeAsync, resource)) {
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
      width: 85,
      render(_, entity) {
        return [
          <Access key={'edit'} accessible={canAccessible(permission.position.putAsync, resource)}>
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
        ];
      },
    },
  ]);
  const [organizationId, setOrganizationId] = useState<string | null>(null);

  return (
    <PageContainer
      header={{
        title: resource?.name,
        children: resource?.description,
      }}
    >
      <ProCard split="vertical">
        <ProCard colSpan="270px">
          <OrganizationTree
            maxHeight={window.innerHeight - 266}
            onSelect={(key) => {
              setOrganizationId(key);
            }}
          />
        </ProCard>
        <ProCard>
          <ProTablePlus<API.PositionListItem, API.PositionPagingParams>
            actionRef={actionRef}
            style={
              tableSize?.width
                ? {
                    maxWidth: tableSize?.width - 270 - 24,
                  }
                : {}
            }
            defaultColumns={defaultColumns}
            query={query}
            moduleName={'Position'}
            params={{
              organizationId: organizationId,
            }}
            scrollY={window.innerHeight - 406}
            toolBarRender={() => [
              <Access
                key={'add'}
                accessible={canAccessible(permission.position.postAsync, resource)}
              >
                <Button
                  type="primary"
                  onClick={() => {
                    handleCreateOrUpdateModalOpen(true);
                  }}
                  icon={<PlusOutlined />}
                >
                  <FormattedMessage id="pages.searchTable.new" defaultMessage="New" />
                </Button>
              </Access>,
            ]}
          />
        </ProCard>
      </ProCard>
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
