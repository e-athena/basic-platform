import type { ActionType, ProColumns } from '@ant-design/pro-components';
import { ProTable } from '@ant-design/pro-components';
import { Checkbox } from 'antd';
import React, { useRef, useState, useEffect } from 'react';
import { useModel } from '@umijs/max';

export type AuthorizationProps = {
  onChange?: (codes: string[]) => void;
  resourceCodes?: string[];
};
const Authorization: React.FC<AuthorizationProps> = (props) => {

  const actionRef = useRef<ActionType>();
  const { initialState } = useModel('@@initialState');
  const [selectedCodes, setSelectedCodes] = useState<string[]>(props.resourceCodes || []);
  useEffect(() => {
    if (props.onChange !== undefined) {
      props.onChange(selectedCodes);
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
      title: '授权',
      dataIndex: 'success',
      width: 70,
      hideInSearch: true,
      align: 'center',
      render(_, entity) {
        return <Checkbox
          checked={selectedCodes.includes(entity.code)}
          onChange={(e) => {
            // console.log(entity.code);
            let codes = [entity.code];
            if (entity.parentCode === null) {
              codes = [entity.code, ...(entity.children || []).map(p => p.code)];
            }
            const { checked } = e.target;
            if (checked) {
              // setSelectedCodes([...selectedCodes, entity.code]);
              setSelectedCodes([...selectedCodes, ...codes]);
            } else {
              // setSelectedCodes(selectedCodes.filter((code) => code !== entity.code));
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
          ].map((item) => (<Checkbox key={item.value} onChange={(e) => {
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
            const funcKeys2: string[] = [...new Set(funcKeys.flat())];
            if (checked) {
              setSelectedCodes([...selectedCodes, ...(funcKeys2 || [])]);
            } else {
              setSelectedCodes(selectedCodes.filter((code) => !funcKeys2.includes(code)));
            }
          }}>{item.label}</Checkbox>));
        }
        return entity.functions?.map((item) => (
          <Checkbox
            key={item.value}
            checked={selectedCodes.includes(item.value)}
            onChange={(e) => {
              const { checked } = e.target;
              if (checked) {
                setSelectedCodes([...selectedCodes, item.value]);
              } else {
                setSelectedCodes(selectedCodes.filter((code) => code !== item.value));
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
      dataSource={initialState?.apiResources?.filter(p => p.children?.filter(c => c.isAuth).length > 0) || []}
      columns={columns}
      scroll={{ x: 730, y: 400 }}
      rowSelection={false}
      pagination={false}
      expandable={{
        defaultExpandAllRows: true,
      }}
    />
  );
};

export default Authorization;
