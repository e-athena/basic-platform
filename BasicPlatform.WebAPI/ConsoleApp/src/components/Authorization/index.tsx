import type { ActionType, ProColumns } from '@ant-design/pro-components';
import { ProTable } from '@ant-design/pro-components';
import { Checkbox } from 'antd';
import React, { useRef, useState, useEffect } from 'react';
type AuthorizationProps = {
  onChange?: (codes: ResourceModel[], currentCodes?: ResourceModel[]) => void;
  resources?: ResourceModel[];
  // 禁用的资源代码
  disabledResourceKeys?: string[];
  height?: number;
  dataSource: API.ResourceInfo[];
};
const Authorization: React.FC<AuthorizationProps> = (props) => {
  const dataSource = (props.dataSource || []).filter(
    (p) => p.children !== undefined && p.children?.filter((c) => c.isAuth).length > 0,
  );
  const actionRef = useRef<ActionType>();
  const [selectedResources, setSelectedResources] = useState<ResourceModel[]>(
    props.resources || [],
  );
  const [expandedRowKeys, setExpandedRowKeys] = useState<string[]>(() => {
    // 获取树形的code
    const getTreeCodes = (items: API.ResourceInfo[]) => {
      let codes: string[] = [];
      items.forEach((item) => {
        codes.push(item.code);
        if (item.children) {
          codes = codes.concat(getTreeCodes(item.children));
        }
      });
      return codes;
    };
    return getTreeCodes(dataSource); //dataSource.map((item) => item.code);
  });
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
      width: 220,
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
              let infos = [{ key: entity.code, code: entity.code, applicationId: entity.appId }];
              if (entity.parentCode === null) {
                infos = [
                  { key: entity.code, code: entity.code, applicationId: entity.appId },
                  ...(entity.children || []).map((p) => ({
                    key: p.code,
                    code: p.code,
                    applicationId: entity.appId,
                  })),
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
          ].map((item) => {
            let allChecked = false;
            if (item.value === 'all') {
              allChecked = entity.children!.every((child) => {
                return (child.functions || []).every((func) => {
                  return selectedResources.map((p) => p.key).includes(func.key);
                });
              });
            }
            if (item.value === 'readonly') {
              allChecked = entity.children!.every((child) => {
                return (child.functions || [])
                  .filter(
                    (p) =>
                      p.label.includes('读取') ||
                      p.label.includes('查询') ||
                      p.label.includes('查看') ||
                      p.label.includes('详情') ||
                      p.label.includes('列表'),
                  )
                  .every((func) => {
                    return selectedResources.map((p) => p.key).includes(func.key);
                  });
              });
            }
            return (
              <Checkbox
                key={item.value}
                checked={allChecked}
                onChange={(e) => {
                  const { checked } = e.target;
                  let funcKeys: ResourceModel[][] = [];
                  if (item.value === 'all') {
                    funcKeys = entity.children!.map((child) => {
                      return (child.functions || [])?.map(
                        (func): ResourceModel => ({
                          key: func.key,
                          code: func.value,
                          applicationId: entity.appId,
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
                    funcKeys2 = funcKeys2.filter(
                      (p) => !props.disabledResourceKeys?.includes(p.key),
                    );
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
            );
          });
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
                setSelectedResources([
                  ...selectedResources,
                  { key: item.key, code: item.value, applicationId: entity.appId },
                ]);
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
    {
      title: '操作',
      dataIndex: 'option',
      hideInSearch: true,
      tooltip: '行全选/反选',
      width: 70,
      align: 'center',
      render(_, entity) {
        const allChecked =
          selectedResources.map((p) => p.key).includes(entity.code) &&
          entity.functions !== undefined &&
          entity.functions?.length > 0 &&
          entity.functions.filter((p) => selectedResources.map((p) => p.key).includes(p.key))
            .length === entity.functions?.length;
        return (
          <Checkbox
            // 如果节点全选了，就勾选
            checked={allChecked}
            // 如果disabledResourceKeys包含了当前节点的key，就禁用
            disabled={
              entity.parentCode === null ||
              (props.disabledResourceKeys?.includes(entity.code) &&
                entity.functions === undefined) ||
              (entity.functions !== undefined &&
                entity.functions?.filter((p) => props.disabledResourceKeys?.includes(p.key))
                  .length === entity.functions?.length)
            }
            // disabled={entity.parentCode === null}
            onChange={(e) => {
              const checked = e.target.checked;
              // 处理菜单和功能的选择
              let infos = [
                {
                  key: entity.code,
                  code: entity.code,
                  applicationId: entity.appId,
                },
                ...(entity.functions?.map((p) => ({
                  key: p.key,
                  code: p.value,
                  applicationId: entity.appId,
                })) || []),
              ];
              if (checked) {
                // 排除disabledResourceKeys的
                if (
                  props.disabledResourceKeys !== undefined &&
                  props.disabledResourceKeys?.length > 0
                ) {
                  infos = infos.filter((p) => !props.disabledResourceKeys?.includes(p.key));
                }
                setSelectedResources([...selectedResources, ...infos]);
              } else {
                // 排除disabledResourceKeys的
                if (
                  props.disabledResourceKeys !== undefined &&
                  props.disabledResourceKeys?.length > 0
                ) {
                  infos = infos.filter((p) => !props.disabledResourceKeys?.includes(p.key));
                }
                setSelectedResources(
                  selectedResources.filter((c) => !infos.map((p) => p.key).includes(c.key)),
                );
              }
            }}
          />
        );
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
      dataSource={dataSource}
      expandable={{
        onExpandedRowsChange(expandedKeys) {
          setExpandedRowKeys(expandedKeys.map((p) => p.toString()));
        },
        expandedRowKeys,
      }}
      columns={columns}
      scroll={{ x: 730, y: props.height || 400 }}
      rowSelection={false}
      pagination={false}
    />
  );
};

export default Authorization;
