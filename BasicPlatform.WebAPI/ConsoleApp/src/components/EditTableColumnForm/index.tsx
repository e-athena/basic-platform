import { DownOutlined, DragOutlined, SettingOutlined } from '@ant-design/icons';
import type { ProColumns } from '@ant-design/pro-components';
import { DragSortTable } from '@ant-design/pro-components';
import { Button, Checkbox, Dropdown, InputNumber, message, Modal, Space, Tooltip } from 'antd';
import { useEffect, useState } from 'react';

type EditTableColumnFormProps = {
  onCancel?: () => void;
  onOk: (data: API.TableColumnItem[]) => void;
  open?: boolean;
  data: API.TableColumnItem[];
};

const App: React.FC<EditTableColumnFormProps> = (props) => {
  const { open, onCancel, onOk, data } = props;
  const [leftDataSource, setLeftDataSource] = useState<API.TableColumnItem[]>([]);
  const [rightDataSource, setRightDataSource] = useState<API.TableColumnItem[]>([]);
  const [dataSource, setDataSource] = useState<API.TableColumnItem[]>([]);
  const [selfOpen, setSelfOpen] = useState<boolean>(false);

  useEffect(() => {
    if (open || selfOpen) {
      setLeftDataSource(data.filter((item) => item.fixed === 'left'));
      setRightDataSource(data.filter((item) => item.fixed === 'right'));
      setDataSource(data.filter((item) => item.fixed !== 'left' && item.fixed !== 'right'));
    }
  }, [open, selfOpen]);

  const columns: ProColumns<API.TableColumnItem>[] = [
    {
      title: '显示',
      dataIndex: 'show',
      width: 50,
      align: 'center',
      render(_, entity) {
        return (
          <Checkbox
            checked={!entity.hideInTable}
            disabled={entity.required}
            onChange={(e) => {
              entity.hideInTable = !e.target.checked;
              setDataSource([...dataSource]);
            }}
          />
        );
      },
    },
    {
      title: '排序',
      dataIndex: 'sort',
    },
    {
      title: '宽度',
      dataIndex: 'width',
      tooltip: '为空时，自动适应宽度',
      width: 170,
      render(_, entity) {
        return (
          <Tooltip placement="top" title={'为空时，自动适应宽度'}>
            <InputNumber
              min={50}
              max={400}
              value={entity.width}
              placeholder={'auto'}
              addonBefore={'宽度'}
              addonAfter={'px'}
              onChange={(value) => {
                entity.width = value;
                setDataSource([...dataSource]);
              }}
            />
          </Tooltip>
        );
      },
    },
    {
      title: '操作',
      dataIndex: 'option',
      width: 100,
      align: 'center',
      render(_, entity) {
        let items = [
          { key: 1, label: '固定到左则' },
          { key: 2, label: '固定到右则' },
        ];
        if (entity.fixed === 'left') {
          items = [
            { key: 0, label: '取消固定' },
            { key: 2, label: '固定到右则' },
          ];
        }
        if (entity.fixed === 'right') {
          items = [
            { key: 0, label: '取消固定' },
            { key: 1, label: '固定到左则' },
          ];
        }
        return (
          <>
            <Dropdown
              menu={{
                items,
                onClick: ({ key }) => {
                  console.log(key);
                  // 取消固定
                  if (key === '0') {
                    // 更新为空
                    entity.fixed = '';
                    // 更新数据源
                    setLeftDataSource(
                      leftDataSource.filter((item) => item.dataIndex !== entity.dataIndex),
                    );
                    setRightDataSource(
                      rightDataSource.filter((item) => item.dataIndex !== entity.dataIndex),
                    );
                    // 更新数据源
                    setDataSource([...dataSource, entity]);
                  }
                  // 固定到左侧
                  if (key === '1') {
                    // 最多固定两列
                    if (leftDataSource.length >= 2) {
                      message.error('最多固定两列');
                      return;
                    }
                    if (entity.fixed === 'right') {
                      setRightDataSource(
                        rightDataSource.filter((item) => item.dataIndex !== entity.dataIndex),
                      );
                    }
                    // 更新为left
                    entity.fixed = 'left';
                    // 更新数据源
                    setLeftDataSource([...leftDataSource, entity]);
                    setDataSource(dataSource.filter((item) => item.dataIndex !== entity.dataIndex));
                  }
                  // 固定到右侧
                  if (key === '2') {
                    // 最多固定两列
                    if (rightDataSource.length >= 2) {
                      message.error('最多固定两列');
                      return;
                    }
                    if (entity.fixed === 'left') {
                      setLeftDataSource(
                        leftDataSource.filter((item) => item.dataIndex !== entity.dataIndex),
                      );
                    }
                    // 更新为right
                    entity.fixed = 'right';
                    // 更新数据源
                    setRightDataSource([...rightDataSource, entity]);
                    setDataSource(dataSource.filter((item) => item.dataIndex !== entity.dataIndex));
                  }
                },
              }}
              trigger={['click']}
            >
              <a onClick={(e) => e.preventDefault()}>
                <Space>
                  操作
                  <DownOutlined />
                </Space>
              </a>
            </Dropdown>
          </>
        );
      },
    },
  ];

  const handleDragSortEndLeft = (newDataSource: any) => {
    setLeftDataSource(newDataSource);
    message.success('修改列表排序成功');
  };
  const handleDragSortEndMid = (newDataSource: any) => {
    setDataSource(newDataSource);
    message.success('修改列表排序成功');
  };
  const handleDragSortEndRight = (newDataSource: any) => {
    setRightDataSource(newDataSource);
    message.success('修改列表排序成功');
  };

  const dragHandleRender = (row: API.TableColumnItem) => (
    <div style={{ position: 'relative' }}>
      <div
        style={{
          cursor: 'move',
          width: row.width ?? 200,
          backgroundColor: '#f2f2f2',
        }}
      >
        <DragOutlined />
        &nbsp;{row.title}
      </div>
    </div>
  );

  return (
    <>
      {props.open === undefined && (
        <Button
          type={'link'}
          style={{ color: '#1f1f1f' }}
          icon={<SettingOutlined />}
          onClick={() => setSelfOpen(true)}
        />
      )}
      <Modal
        title="自定义列，勾选需要显示的列，拖动列名进行排序。"
        open={open || selfOpen}
        width={800}
        bodyStyle={{ paddingTop: 32 }}
        destroyOnClose
        onCancel={() => {
          onCancel?.();
          setSelfOpen(false);
        }}
        onOk={() => {
          // 按组合数据并重置排序值
          let sort = 0;
          const newData = [...leftDataSource, ...dataSource, ...rightDataSource].map((item) => {
            item.sort = sort;
            sort++;
            return item;
          });
          setSelfOpen(false);
          onOk(newData);
        }}
      >
        {leftDataSource.length > 0 && (
          <DragSortTable<API.TableColumnItem>
            showHeader={false}
            bordered
            cardProps={{ bodyStyle: { padding: 0 } }}
            style={{ marginBottom: 16 }}
            headerTitle={false}
            columns={columns}
            rowKey="dataIndex"
            options={false}
            search={false}
            pagination={false}
            dataSource={leftDataSource}
            dragSortKey="sort"
            dragSortHandlerRender={dragHandleRender}
            onDragSortEnd={handleDragSortEndLeft}
          />
        )}
        <DragSortTable<API.TableColumnItem>
          showHeader={false}
          bordered
          cardProps={{ bodyStyle: { padding: 0 } }}
          headerTitle={false}
          columns={columns}
          rowKey="dataIndex"
          options={false}
          search={false}
          pagination={false}
          // request={request}
          dataSource={dataSource}
          scroll={{ y: 400 }}
          dragSortKey="sort"
          dragSortHandlerRender={dragHandleRender}
          onDragSortEnd={handleDragSortEndMid}
        />
        {rightDataSource.length > 0 && (
          <DragSortTable<API.TableColumnItem>
            showHeader={false}
            bordered
            cardProps={{ bodyStyle: { padding: 0 } }}
            style={{ marginTop: 16 }}
            headerTitle={false}
            columns={columns}
            rowKey="dataIndex"
            options={false}
            search={false}
            pagination={false}
            dataSource={rightDataSource}
            dragSortKey="sort"
            dragSortHandlerRender={dragHandleRender}
            onDragSortEnd={handleDragSortEndRight}
          />
        )}
      </Modal>
    </>
  );
};

export default App;
