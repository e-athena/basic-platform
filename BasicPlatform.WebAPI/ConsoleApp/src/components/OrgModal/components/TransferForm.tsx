import React, { useEffect, useState } from 'react';
import { message, Spin, Table, Transfer } from 'antd';
import type { ColumnsType, TableRowSelection } from 'antd/es/table/interface';
import type { TransferItem, TransferProps } from 'antd/es/transfer';
import difference from 'lodash/difference';
import { orgList } from '@/services/ant-design-pro/system/org';

interface TableTransferProps extends TransferProps<TransferItem> {
  dataSource: TransferItem[];
  leftColumns: ColumnsType<TransferItem>;
  rightColumns: ColumnsType<TransferItem>;
}

// Customize Table Transfer
const TableTransfer = ({ leftColumns, rightColumns, ...restProps }: TableTransferProps) => (
  <Transfer {...restProps}>
    {({
      direction,
      filteredItems,
      onItemSelectAll,
      onItemSelect,
      selectedKeys: listSelectedKeys,
      disabled: listDisabled,
    }) => {
      const columns = direction === 'left' ? leftColumns : rightColumns;

      const rowSelection: TableRowSelection<TransferItem> = {
        getCheckboxProps: (item) => ({ disabled: listDisabled || item.disabled }),
        onSelectAll(selected, selectedRows) {
          const treeSelectedKeys = selectedRows
            .filter((item) => !item.disabled)
            .map(({ key }) => key);
          const diffKeys = selected
            ? difference(treeSelectedKeys, listSelectedKeys)
            : difference(listSelectedKeys, treeSelectedKeys);
          onItemSelectAll(diffKeys as string[], selected);
        },
        onSelect({ key }, selected) {
          onItemSelect(key as string, selected);
        },
        selectedRowKeys: listSelectedKeys,
      };

      return (
        <Table
          rowSelection={rowSelection}
          columns={columns}
          dataSource={filteredItems}
          size="small"
          style={{ pointerEvents: listDisabled ? 'none' : undefined }}
          scroll={{ y: 300 }}
          onRow={({ key, disabled: itemDisabled }) => ({
            onClick: () => {
              if (itemDisabled || listDisabled) return;
              onItemSelect(key as string, !listSelectedKeys.includes(key as string));
            },
          })}
        />
      );
    }}
  </Transfer>
);

const leftTableColumns: ColumnsType<TransferItem> = [
  {
    dataIndex: 'title',
    title: '组织/部门',
  },
];

const rightTableColumns: ColumnsType<Pick<TransferItem, 'title'>> = [
  {
    dataIndex: 'title',
    title: '组织/部门',
  },
];

/** 组织/部门信息 */
export type TransferOrgInfo = {
  id: string;
  name: string;
};

export type TransferFormProps = {
  onChange?: (keys: string[], rows: TransferOrgInfo[]) => void;
  organizationId?: string;
  mode?: 'multiple' | 'single';
  defaultSelectedKeys?: string[];
};

const App: React.FC<TransferFormProps> = (props) => {
  const [targetKeys, setTargetKeys] = useState<string[]>(props.defaultSelectedKeys || []);
  const [dataSource, setDataSource] = useState<TransferItem[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [lastOrganizationId, setLastOrganizationId] = useState<string | undefined>(undefined);
  useEffect(() => {
    if (lastOrganizationId !== props.organizationId) {
      setLoading(true);
      setLastOrganizationId(props.organizationId);
    }
  }, [props.organizationId]);
  useEffect(() => {
    const fetch = async () => {
      const res = await orgList(props.organizationId);
      setDataSource(
        res.success
          ? res.data!.map((item: API.SelectInfo) => ({
            key: item.value,
            title: item.label,
          }))
          : [],
      );
      setLoading(false);
    };
    if (loading) {
      fetch();
    }
  }, [loading]);

  const onChange = (nextTargetKeys: string[]) => {
    if (props.mode === 'single' && nextTargetKeys.length > 1) {
      message.warning('只能选择一个组织/部门');
      return;
    }
    setTargetKeys(nextTargetKeys);
    props.onChange?.(
      nextTargetKeys,
      dataSource
        .filter((item) => nextTargetKeys.includes(item.key!))
        .map((item) => ({
          id: item.key!,
          name: item.title!,
        })),
    );
  };

  return (
    <>
      <Spin spinning={loading}>
        <TableTransfer
          dataSource={dataSource}
          targetKeys={targetKeys}
          showSearch
          onChange={onChange}
          filterOption={(inputValue, item) =>
            item.title!.indexOf(inputValue) !== -1 || item.description!.indexOf(inputValue) !== -1
          }
          leftColumns={leftTableColumns}
          rightColumns={rightTableColumns}
        />
      </Spin>
    </>
  );
};

export default App;
