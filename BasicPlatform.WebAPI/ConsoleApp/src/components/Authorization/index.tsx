import type { ActionType, ProColumns } from '@ant-design/pro-components';
import { ProTable } from '@ant-design/pro-components';
import { Checkbox } from 'antd';
import React, { useRef, useState, useEffect } from 'react';
import { useModel } from '@umijs/max';

export type AuthorizationProps = {
  onChange?: (codes: string[], currentCodes?: string[]) => void;
  resourceCodes?: string[];
  // 禁用的资源代码
  disabledResourceCodes?: string[];
};
const Authorization: React.FC<AuthorizationProps> = (props) => {

  const actionRef = useRef<ActionType>();
  const { initialState } = useModel('@@initialState');
  const [selectedCodes, setSelectedCodes] = useState<string[]>(props.resourceCodes || []);
  const [expandedRowKeys, setExpandedRowKeys] = useState<string[]>([]);
  useEffect(() => {
    if (props.onChange !== undefined) {
      props.onChange(selectedCodes, selectedCodes.filter(p => !props.disabledResourceCodes?.includes(p)));
    }
  }, [selectedCodes])

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
      render(_, entity) {
        return <Checkbox
          checked={selectedCodes.includes(entity.code)}
          disabled={props.disabledResourceCodes?.includes(entity.code)}
          onChange={(e) => {
            // console.log(entity.code);
            let codes = [entity.code];
            if (entity.parentCode === null) {
              codes = [entity.code, ...(entity.children || []).map(p => p.code)];
            }
            const { checked } = e.target;
            if (checked) {
              setSelectedCodes([...selectedCodes, ...codes]);
            } else {
              setSelectedCodes(selectedCodes.filter((code) => !codes.includes(code)));
            }
          }}
        />;
      }
    },
    {
      title: '功能',
      dataIndex: 'functions',
      hideInSearch: true,
      render(_, entity) {
        if (entity.parentCode === null && entity.children?.filter(p => p.isAuth).length > 0) {
          return [
            { label: '只读', value: 'readonly' },
            { label: '全部', value: 'all' }
          ].map((item) => (<Checkbox
            key={item.value}
            onChange={(e) => {
              const { checked } = e.target;

              let funcKeys: string[][] = [];
              if (item.value === 'all') {
                funcKeys = entity.children.map((child) => {
                  return (child.functions || [])?.map((func) => func.key);
                });
              } else {
                funcKeys = entity.children.map((child) => {
                  // 包含"读取","查询","查看","详情"的都是读取的权限
                  return (child.functions || []).filter(p =>
                    p.label.includes('读取') ||
                    p.label.includes('查询') ||
                    p.label.includes('查看') ||
                    p.label.includes('详情')
                  ).map((func) => func.key);
                });
              }
              // 二维数组转一维数组
              let funcKeys2: string[] = [...new Set(funcKeys.flat())];
              // 禁用的不处理
              if (props.disabledResourceCodes !== undefined && props.disabledResourceCodes?.length > 0) {
                funcKeys2 = funcKeys2.filter(p => !props.disabledResourceCodes?.includes(p));
              }
              if (checked) {
                setSelectedCodes([...selectedCodes, ...(funcKeys2 || [])]);
              } else {
                setSelectedCodes(selectedCodes.filter((code) => !funcKeys2.includes(code)));
              }
            }}>{item.label}</Checkbox>));
        }
        return entity.functions?.map((item) => (
          <Checkbox
            key={item.key}
            checked={selectedCodes.includes(item.key)}
            disabled={props.disabledResourceCodes?.includes(item.key)}
            onChange={(e) => {
              const { checked } = e.target;
              if (checked) {
                setSelectedCodes([...selectedCodes, item.key]);
              } else {
                setSelectedCodes(selectedCodes.filter((code) => code !== item.key));
              }
            }}>{item.label}</Checkbox>
        ));
      }
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
        const list = (data || []).filter(p => p.children?.filter(c => c.isAuth).length > 0);
        setExpandedRowKeys(list!.map(item => item.code));
        return {
          data: list,
          success: true,
          total: 0,
        }
      }}
      expandable={{
        expandedRowKeys: expandedRowKeys
      }}
      columns={columns}
      scroll={{ x: 730, y: 400 }}
      rowSelection={false}
      pagination={false}
    />
  );
};

export default Authorization;
