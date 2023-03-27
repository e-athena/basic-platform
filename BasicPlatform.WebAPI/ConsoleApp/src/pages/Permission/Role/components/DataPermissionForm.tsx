import { submitHandle } from '@/utils/utils';
import { ModalForm, ProColumns, ProTable } from '@ant-design/pro-components';
import { Badge, Button, Checkbox, Radio, Segmented, Space, Tooltip } from 'antd';
import React, { useState } from 'react';
import { dataPermission, assignDataPermissions } from '../service';
import OrgModal from '@/components/OrgModal';

type DataPermissionFormProps = {
  onCancel: () => void;
  onSuccess: () => void;
  open: boolean;
  roleId: string;
  title?: string;
};

const DataPermissionForm: React.FC<DataPermissionFormProps> = (props) => {
  const [dataSources, setDataSources] = useState<API.RoleDataPermissionGroup[]>([]);
  const [segmentedOptions, setSegmentedOptioins] = useState<string[]>([]);
  const [currentSegmented, setCurrentSegmented] = useState<string>('');
  const [orgModalOpen, setOrgModalOpen] = useState<boolean>(false);
  const [dataScopeCustoms, setDataScopeCustoms] = useState<string[]>([]);
  const [currentResourceKey, setCurrentResourceKey] = useState<string>('');

  const columns: ProColumns<API.RoleDataPermission>[] = [
    {
      title: '启用',
      dataIndex: 'enabled',
      hideInSearch: true,
      width: 70,
      tooltip: '选中后，该模块的数据访问范围才会生效',
      align: 'center',
      render(_, entity) {
        return (
          <Checkbox
            checked={entity.enabled}
            onChange={(e) => {
              // 更新dataSources的启用状态
              const index = dataSources.findIndex((item) => item.displayName === currentSegmented);
              const items = dataSources[index].items;
              const itemIndex = items.findIndex((item) => item.resourceKey === entity.resourceKey);
              items[itemIndex].enabled = e.target.checked;
              setDataSources([...dataSources]);
            }}
          />
        );
      },
    },
    {
      title: '模块名称',
      dataIndex: 'displayName',
      hideInSearch: true,
      ellipsis: true,
      width: 180,
    },
    {
      title: '数据访问范围',
      dataIndex: 'dataScope',
      hideInSearch: true,
      render(_, entity) {
        const defaultDom = (
          <Radio.Group
            onChange={(e) => {
              // 更新dataSources的访问范围
              const index = dataSources.findIndex((item) => item.displayName === currentSegmented);
              const items = dataSources[index].items;
              const itemIndex = items.findIndex((item) => item.resourceKey === entity.resourceKey);
              items[itemIndex].dataScope = e.target.value;
              setDataSources([...dataSources]);
            }}
            value={entity.dataScope}
          >
            <Radio value={0}>
              <Tooltip title={'不受查询限制，能查询所有的数据。'} placement={'top'}>
                全部
              </Tooltip>
            </Radio>
            <Radio value={1}>
              <Tooltip title={'只能查询本人创建的数据。'} placement={'top'}>
                本人
              </Tooltip>
            </Radio>
            <Radio value={2}>
              <Tooltip
                title={'只能查询本人所在的部门成员创建的数据，离开本部门后将查询不到。'}
                placement={'top'}
              >
                本部门
              </Tooltip>
            </Radio>
            <Radio value={3}>
              <Tooltip
                title={'只能查询本人所在的部门成员及下级部门创建的数据，离开本部门后将查询不到。'}
                placement={'top'}
              >
                本部门及下属部门
              </Tooltip>
            </Radio>
            <Radio value={4}>
              <Tooltip title={'可查询指定组织/部门成员创建的数据。'} placement={'top'}>
                {entity.dataScope === 4 && entity.dataScopeCustoms.length > 0 ? (
                  <Badge count={entity.dataScopeCustoms.length}>自定义</Badge>
                ) : (
                  <span>自定义</span>
                )}
              </Tooltip>
            </Radio>
          </Radio.Group>
        );
        if (entity.dataScope !== 4) {
          return defaultDom;
        }
        return (
          <Space>
            {defaultDom}
            <Button
              size={'small'}
              onClick={() => {
                setCurrentResourceKey(entity.resourceKey);
                setDataScopeCustoms(entity.dataScopeCustoms || []);
                setOrgModalOpen(true);
              }}
            >
              选择组织
            </Button>
          </Space>
        );
      },
    },
  ];
  return (
    <ModalForm
      width={860}
      title={props.title || '分配权限'}
      open={props.open}
      modalProps={{
        onCancel: () => {
          props.onCancel();
        },
        // bodyStyle: { padding: '32px 40px 48px' },
        bodyStyle: { padding: '10px 0', minHeight: 400 },
        destroyOnClose: true,
      }}
      onFinish={async () => {
        // 将DataSources展开
        let permissions: API.RoleDataPermissionItem[] = [];
        for (let j = 0; j < dataSources.length; j++) {
          const group = dataSources[j];
          for (let i = 0; i < group.items.length; i++) {
            const item = group.items[i];
            permissions.push({
              resourceKey: item.resourceKey,
              dataScope: item.dataScope,
              enabled: item.enabled,
              dataScopeCustom: (item.dataScopeCustoms || []).join(','),
            });
          }
        }
        const succeed = await submitHandle(assignDataPermissions, {
          id: props.roleId,
          permissions,
        });
        if (succeed) {
          props.onSuccess();
        }
      }}
    >
      {segmentedOptions.length > 1 && (
        <div style={{ padding: '20px 0' }}>
          <Segmented
            block
            options={segmentedOptions}
            value={currentSegmented}
            onChange={(value) => {
              setCurrentSegmented(value.toString());
            }}
          />
        </div>
      )}
      <ProTable<API.RoleDataPermission>
        headerTitle={'资源列表'}
        rowKey="resourceKey"
        search={false}
        toolBarRender={false}
        request={async (params) => {
          if (dataSources.length > 0 && params.group !== '') {
            const items =
              dataSources.find((item) => item.displayName === params.group)?.items || [];
            return {
              data: items,
              success: true,
              total: 0,
            };
          }
          const res = await dataPermission(props.roleId);
          let list: any = [];
          if (res.success && res.data !== null && res.data!.length > 0) {
            const data = res.data!;
            setDataSources(data);
            setSegmentedOptioins(data.map((item) => item.displayName!));
            setCurrentSegmented(data[0].displayName!);
            list = res.data![0].items;
          }
          return {
            data: list,
            success: true,
            total: 0,
          };
        }}
        params={{
          group: currentSegmented,
        }}
        columns={columns}
        scroll={{ x: 730, y: 400 }}
        rowSelection={false}
        pagination={false}
      />

      <OrgModal
        mode={'multiple'}
        open={orgModalOpen}
        onCancel={() => {
          setOrgModalOpen(false);
        }}
        onOk={(keys: string[]) => {
          // 更新dataSources的访问范围
          const index = dataSources.findIndex((item) => item.displayName === currentSegmented);
          const items = dataSources[index].items;
          const itemIndex = items.findIndex((item) => item.resourceKey === currentResourceKey);
          items[itemIndex].dataScope = 4;
          items[itemIndex].dataScopeCustoms = keys;
          setDataSources([...dataSources]);
          setOrgModalOpen(false);
        }}
        defaultSelectedKeys={dataScopeCustoms || []}
      />
    </ModalForm>
  );
};

export default DataPermissionForm;
