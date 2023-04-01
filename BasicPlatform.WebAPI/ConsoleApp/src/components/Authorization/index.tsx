import type { ActionType, ProColumns } from '@ant-design/pro-components';
import { ProTable } from '@ant-design/pro-components';
import { Checkbox } from 'antd';
import React, { useRef, useState, useEffect } from 'react';
import { useModel } from '@umijs/max';

type AuthorizationProps = {
  onChange?: (codes: ResourceModel[], currentCodes?: ResourceModel[]) => void;
  resources?: ResourceModel[];
  // 禁用的资源代码
  disabledResourceKeys?: string[];
  height?: number;
};
const Authorization: React.FC<AuthorizationProps> = (props) => {
  const actionRef = useRef<ActionType>();
  const { initialState } = useModel('@@initialState');
  const [selectedResources, setSelectedResources] = useState<ResourceModel[]>(
    props.resources || [],
  );
  const [expandedRowKeys, setExpandedRowKeys] = useState<string[]>();
  useEffect(() => {
    if (props.onChange !== undefined) {
      props.onChange(
        selectedResources,
        selectedResources.filter((p) => !props.disabledResourceKeys?.includes(p.key)),
      );
    }
  }, [selectedResources]);

  const columns: ProColumns<API.ResourceInfo>[] = [
    {
      title: '名称',
      dataIndex: 'name',
      hideInSearch: true,
      width: 150,
    },
    {
      title: '菜单',
      dataIndex: 'success',
      width: 70,
      hideInSearch: true,
      align: 'center',
      tooltip: '左侧菜单，需要授权显示就勾选',
      render(_, entity) {
        return (
          <Checkbox
            style={{
              marginTop: 5,
              marginInlineStart: 0,
              marginRight: 8,
            }}
            checked={selectedResources.map((p) => p.key).includes(entity.code)}
            disabled={props.disabledResourceKeys?.includes(entity.code)}
            onChange={(e) => {
              let infos = [{ key: entity.code, code: entity.code }];
              if (entity.parentCode === null) {
                infos = [
                  { key: entity.code, code: entity.code },
                  ...(entity.children || []).map((p) => ({ key: p.code, code: p.code })),
                ];
              }
              const { checked } = e.target;
              if (checked) {
                setSelectedResources([...selectedResources, ...infos]);
              } else {
                setSelectedResources(
                  selectedResources.filter((c) => !infos.map((p) => p.key).includes(c.key)),
                );
              }
            }}
          />
        );
      },
    },
    {
      title: '功能',
      dataIndex: 'functions',
      hideInSearch: true,
      tooltip: '功能权限，需要授权操作就勾选',
      render(_, entity) {
        if (
          entity.parentCode === null &&
          entity.children !== undefined &&
          entity.children?.filter((p) => p.isAuth).length > 0
        ) {
          return [
            { label: '只读', value: 'readonly' },
            { label: '全部', value: 'all' },
          ].map((item) => (
            <Checkbox
              key={item.value}
              onChange={(e) => {
                const { checked } = e.target;

                let funcKeys: ResourceModel[][] = [];
                if (item.value === 'all') {
                  funcKeys = entity.children!.map((child) => {
                    return (child.functions || [])?.map(
                      (func): ResourceModel => ({
                        key: func.key,
                        code: func.value,
                      }),
                    );
                  });
                } else {
                  funcKeys = entity.children!.map((child) => {
                    // 包含"读取","查询","查看","详情"的都是读取的权限
                    return (child.functions || [])
                      .filter(
                        (p) =>
                          p.label.includes('读取') ||
                          p.label.includes('查询') ||
                          p.label.includes('查看') ||
                          p.label.includes('详情') ||
                          p.label.includes('列表'),
                      )
                      .map((func) => ({
                        key: func.key,
                        code: func.value,
                      }));
                  });
                }
                // 二维数组转一维数组
                let funcKeys2: ResourceModel[] = [...new Set(funcKeys.flat())];
                // 禁用的不处理
                if (
                  props.disabledResourceKeys !== undefined &&
                  props.disabledResourceKeys?.length > 0
                ) {
                  funcKeys2 = funcKeys2.filter((p) => !props.disabledResourceKeys?.includes(p.key));
                }
                if (checked) {
                  setSelectedResources([...selectedResources, ...(funcKeys2 || [])]);
                } else {
                  setSelectedResources(
                    selectedResources.filter((c) => !funcKeys2.map((p) => p.key).includes(c.key)),
                  );
                }
              }}
            >
              {item.label}
            </Checkbox>
          ));
        }
        return entity.functions?.map((item) => (
          <Checkbox
            style={{
              marginTop: 5,
              marginInlineStart: 0,
              marginRight: 8,
            }}
            key={item.key}
            checked={selectedResources.map((p) => p.key).includes(item.key)}
            disabled={props.disabledResourceKeys?.includes(item.key)}
            onChange={(e) => {
              const { checked } = e.target;
              if (checked) {
                setSelectedResources([...selectedResources, { key: item.key, code: item.value }]);
              } else {
                setSelectedResources(selectedResources.filter((c) => c.key !== item.key));
              }
            }}
          >
            {item.label}
          </Checkbox>
        ));
      },
    },
  ];

  return (
    <ProTable<API.ResourceInfo, API.PageParams>
      headerTitle={'资源列表'}
      actionRef={actionRef}
      rowKey="code"
      search={false}
      toolBarRender={false}
      // dataSource={initialState?.apiResources?.filter(p => p.children?.filter(c => c.isAuth).length > 0) || []}
      request={async () => {
        const data = await initialState?.fetchApiResources?.();
        const list = (data || []).filter(
          (p) => p.children !== undefined && p.children?.filter((c) => c.isAuth).length > 0,
        );
        setExpandedRowKeys(list.map((item) => item.code));
        return {
          data: list,
          success: true,
          total: 0,
        };
      }}
      expandable={{
        expandedRowKeys: expandedRowKeys,
      }}
      columns={columns}
      scroll={{ x: 730, y: props.height || 400 }}
      rowSelection={false}
      pagination={false}
    />
  );
};

export default Authorization;
