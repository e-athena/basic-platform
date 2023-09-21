import { ProColumns, ProTable } from '@ant-design/pro-components';
import { Checkbox, Input, InputNumber, Modal, Select } from 'antd';
import React, { useState } from 'react';
import { ColumnConfigType } from '..';

export type ColumnProperty = {
  enabled: boolean;
  columnName: string;
  columnType: string;
  columnKey: string;
  isEnableDataMask: boolean;
  dataMaskType: number;
  maskLength: number;
  maskPosition: number;
  maskChar: string;
  propertyType?: string;
}

type ColumnConfigProps = {
  onCancel?: () => void;
  onOk: (data: ColumnProperty[]) => void;
  open?: boolean;
  title?: string;
  data: ColumnConfigType;
};

const ColumnConfig: React.FC<ColumnConfigProps> = (props) => {
  // 跳过包含Id字符的字段
  const [dataSources, setDataSources] = useState<ColumnProperty[]>(props.data.items);

  const columns: ProColumns<ColumnProperty>[] = [
    {
      title: '序号',
      dataIndex: 'index',
      fixed: 'left',
      valueType: 'indexBorder',
      width: 75,
      align: 'center',
    },
    {
      title: '显示',
      dataIndex: 'enabled',
      hideInSearch: true,
      width: 70,
      tooltip: '选中后，该角色将能够查看该列的数据',
      align: 'center',
      render(_, entity) {
        return (
          <Checkbox
            checked={entity.enabled}
            onChange={(e) => {
              // 更新dataSources的启用状态
              const index = dataSources.findIndex((item) => item.columnKey === entity.columnKey);
              dataSources[index].enabled = e.target.checked;
              setDataSources([...dataSources]);
            }}
          />
        );
      },
    },
    {
      title: '列名',
      dataIndex: 'columnName',
      hideInSearch: true,
      ellipsis: true,
      // width: 180,
    },
    {
      title: '脱敏配置',
      dataIndex: 'label',
      hideInSearch: true,
      ellipsis: true,
      children: [
        {
          title: '启用',
          dataIndex: 'isEnableDataMask',
          hideInSearch: true,
          tooltip: '启用后，该列的数据将会被脱敏处理',
          align: 'center',
          width: 70,
          render(_, entity) {
            return (
              <Checkbox
                checked={entity.isEnableDataMask}
                disabled={entity.propertyType !== 'string'}
                onChange={(e) => {
                  // 更新dataSources的启用状态
                  const index = dataSources.findIndex((item) => item.columnKey === entity.columnKey);
                  dataSources[index].isEnableDataMask = e.target.checked;
                  setDataSources([...dataSources]);
                }}
              />
            );
          }
        },
        {
          title: '脱敏类型',
          dataIndex: 'dataMaskType',
          hideInSearch: true,
          width: 135,
          render(_, entity) {
            // 姓名，手机号，身份证号，银行卡号，邮箱，地址，自定义
            return (
              <Select
                placeholder={'请选择'}
                style={{ width: 120 }}
                allowClear
                showSearch
                value={entity.dataMaskType}
                disabled={entity.propertyType !== 'string'}
                onChange={(value) => {
                  // 更新dataSources的脱敏类型
                  const index = dataSources.findIndex((item) => item.columnKey === entity.columnKey);
                  dataSources[index].dataMaskType = value;
                  // 更新其他设置
                  if (value === 1) {
                    if (dataSources[index].maskLength !== 2) {
                      dataSources[index].maskLength = 2;
                    }
                  } else if (value === 3) {
                    if (dataSources[index].maskLength !== 6) {
                      dataSources[index].maskLength = 6;
                    }
                    if (dataSources[index].maskPosition !== 3) {
                      dataSources[index].maskPosition = 3;
                    }
                  } else if (value === 6) {
                    if (dataSources[index].maskLength !== 8) {
                      dataSources[index].maskLength = 8;
                    }
                  }

                  setDataSources([...dataSources]);
                }}
                options={
                  [{
                    label: '姓名',
                    value: 1
                  }, {
                    label: '手机号',
                    value: 2
                  }, {
                    label: '身份证号',
                    value: 3
                  }, {
                    label: '银行卡号',
                    value: 4
                  }, {
                    label: '邮箱',
                    value: 5
                  }, {
                    label: '地址',
                    value: 6
                  }, {
                    label: '通用类型',
                    value: 99
                  }]}
              />
            );
          }
        },
        {
          title: '掩码位置',
          dataIndex: 'maskPosition',
          hideInSearch: true,
          width: 135,
          render(_, entity) {
            return (
              <Select
                placeholder={'请选择'}
                value={entity.maskPosition}
                disabled={entity.propertyType !== 'string'}
                style={{ width: 120 }}
                allowClear
                showSearch
                onChange={(value) => {
                  // 更新dataSources的掩码位置
                  const index = dataSources.findIndex((item) => item.columnKey === entity.columnKey);
                  dataSources[index].maskPosition = value;
                  setDataSources([...dataSources]);
                }}
                options={
                  [{
                    label: '前面',
                    value: 1
                  }, {
                    label: '中间',
                    value: 2
                  }, {
                    label: '后面',
                    value: 3
                  }]}
              />
            );
          }
        },
        {
          title: '掩码长度',
          dataIndex: 'maskLength',
          hideInSearch: true,
          align: 'center',
          width: 105,
          render(_, entity) {
            return (
              <InputNumber
                min={1}
                value={entity.maskLength}
                disabled={entity.propertyType !== 'string'}
                placeholder={'请输入'}
                onChange={(value) => {
                  // 更新dataSources的掩码长度
                  const index = dataSources.findIndex((item) => item.columnKey === entity.columnKey);
                  if (value) {
                    dataSources[index].maskLength = value;
                  } else {
                    dataSources[index].maskLength = 2;
                  }

                  setDataSources([...dataSources]);
                }}
              />
            );
          }
        },
        {
          title: '掩码字符',
          dataIndex: 'maskChar',
          hideInSearch: true,
          align: 'center',
          width: 100,
          render(_, entity) {
            return (
              <Input
                value={entity.maskChar}
                disabled={entity.propertyType !== 'string'}
                placeholder={'请输入'}
                onChange={(e) => {
                  // 更新dataSources的掩码字符
                  const index = dataSources.findIndex((item) => item.columnKey === entity.columnKey);
                  dataSources[index].maskChar = e.target.value;
                  setDataSources([...dataSources]);
                }}
              />
            );
          }
        },
      ],
    },
  ];
  return (
    <>
      <Modal
        title={props.title || '数据列权限配置'}
        open={props.open}
        width={1000}
        bodyStyle={{
          paddingTop: 15,
        }}
        maskClosable={false}
        destroyOnClose
        onCancel={() => {
          props.onCancel?.();
        }}
        onOk={() => {
          props.onOk(dataSources);
        }}
      >
        <ProTable<ColumnProperty>
          // loading={loading}
          headerTitle={'权限列表'}
          rowKey="resourceKey"
          search={false}
          toolBarRender={false}
          bordered
          dataSource={dataSources}
          columns={columns}
          // scroll={{ x: 740, y: 400 }}
          scroll={{ y: 500 }}
          rowSelection={false}
          pagination={false}
        />
      </Modal>
    </>
  );
};

export default ColumnConfig;
