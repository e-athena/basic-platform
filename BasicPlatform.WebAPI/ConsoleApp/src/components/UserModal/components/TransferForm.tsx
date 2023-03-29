import React, { useEffect, useState } from 'react';
import { message, Spin, Table, Transfer } from 'antd';
import type { ColumnsType, TableRowSelection } from 'antd/es/table/interface';
import type { TransferItem, TransferProps } from 'antd/es/transfer';
import difference from 'lodash/difference';
import { userList } from '@/services/ant-design-pro/system/user';

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
          locale={{ emptyText: direction === 'left' ? '暂无数据' : '未选择用户' }}
          style={{ pointerEvents: listDisabled ? 'none' : undefined }}
          scroll={{ y: 350 }}
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
    title: '姓名',
  },
  {
    dataIndex: 'description',
    title: '帐号',
  },
];

const rightTableColumns: ColumnsType<Pick<TransferItem, 'title'>> = [
  {
    dataIndex: 'title',
    title: '姓名',
  },
];

/** 用户信息 */
export type TransferUserInfo = {
  id: string;
  realName: string;
  userName: string;
};

export type TransferFormProps = {
  onChange?: (keys: string[], rows: TransferUserInfo[]) => void;
  organizationId?: string;
  mode?: 'multiple' | 'single';
  defaultSelectedKeys?: string[];
};

const TransferForm: React.FC<TransferFormProps> = (props) => {
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
      const res = await userList(props.organizationId);
      setDataSource(
        res.success
          ? res.data!.map((item: API.SelectInfo) => ({
            key: item.value,
            title: item.label,
            description: item.extend,
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
      message.warning('只能选择一个用户');
      return;
    }
    setTargetKeys(nextTargetKeys);
    props.onChange?.(
      nextTargetKeys,
      dataSource
        .filter((item) => nextTargetKeys.includes(item.key!))
        .map((item) => ({
          id: item.key!,
          realName: item.title!,
          userName: item.description!,
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

export default TransferForm;
