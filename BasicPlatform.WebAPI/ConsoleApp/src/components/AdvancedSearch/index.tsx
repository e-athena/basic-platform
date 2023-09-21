import { SearchOutlined } from '@ant-design/icons';
import { ProCard } from '@ant-design/pro-components';
import { Badge, Button, Empty, message, Modal, Select, Space, theme, Tooltip } from 'antd';
import { useEffect, useState } from 'react';
import RulerItem from './components/RulerItem';
import { useKeyPress } from 'ahooks';

type EditTableColumnFormProps = {
  onCancel?: () => void;
  onSearch: (data: FilterGroupItem[]) => void;
  open?: boolean;
  data: API.TableColumnItem[];
  historyFilters?: FilterGroupItem[];
};

/**
 * 自定义查询
 * @param props
 * @returns
 */
const AdvancedSearch: React.FC<EditTableColumnFormProps> = (props) => {
  const { useToken } = theme;
  const { token } = useToken();
  const { open, onCancel, onSearch, data, historyFilters } = props;
  const [selfOpen, setSelfOpen] = useState<boolean>(false);
  const [filterGroups, setFilterGroups] = useState<FilterGroupItem[]>(historyFilters || []);
  // 通过filterGroups读取filters的总数
  let count = 0;
  for (let i = 0; i < filterGroups.length; i += 1) {
    const group = filterGroups[i];
    count += group.filters.length;
  }

  useKeyPress(['meta.k', 'ctrl.k'], () => {
    setSelfOpen(!selfOpen);
  });
  useEffect(() => {
    if (open || selfOpen) {
      // 如果分组为空，添加一个默认分组
      if (filterGroups.length === 0) {
        setFilterGroups([
          {
            xor: 'and',
            filters: [
              {
                groupIndex: 0,
                index: 0,
                xor: 'and',
              },
            ],
          },
        ]);
      }
    }
  }, [open, selfOpen]);
  // 关闭或取消
  const onClose = () => {
    onCancel?.();
    setSelfOpen(false);
    // 数据验证
    if (filterGroups.length > 0) {
      let errCount: number = 0;
      for (let i = 0; i < filterGroups.length; i++) {
        const group = filterGroups[i];
        for (let j = 0; j < group.filters.length; j++) {
          const filter = group.filters[j];
          // 验证是否有空值
          if (!filter.key || !filter.operator || !filter.value) {
            // message.error(`第${i + 1}组第${j + 1}条数据有空值`);
            errCount++;
            break;
          }
        }
      }
      if (errCount > 0) {
        setFilterGroups([]);
        onSearch([]);
      }
    }
  }
  return (
    <>
      {props.open === undefined && (
        <Tooltip title={'自定义查询'}>
          {count === 0 ? <Button
            type={'link'}
            style={{ color: token.colorText }}
            icon={<SearchOutlined />}
            onClick={() => setSelfOpen(true)}
          /> : <Badge count={count}>
            <Button
              type={'link'}
              style={{ color: token.colorText }}
              icon={<SearchOutlined />}
              onClick={() => setSelfOpen(true)}
            />
          </Badge>}

        </Tooltip>
      )}
      <Modal
        title="自定义查询"
        open={open || selfOpen}
        width={1000}
        bodyStyle={{
          maxHeight: 'calc(100vh - 400px)',
          overflow: 'auto',
          minHeight: 400,
          paddingTop: 15,
        }}
        destroyOnClose
        onCancel={onClose}
        footer={[
          <Button
            key={'add'}
            type={'dashed'}
            onClick={() => {
              setFilterGroups([
                ...filterGroups,
                {
                  xor: 'and',
                  filters: [
                    {
                      groupIndex: filterGroups.length,
                      index: 0,
                      xor: 'and',
                    },
                  ],
                },
              ]);
            }}
          >
            添加分组
          </Button>,
          <Button
            key="reset"
            danger
            type={'dashed'}
            onClick={() => {
              // 重置
              setFilterGroups([]);
              setSelfOpen(false);
              onSearch([]);
            }}
          >
            重置
          </Button>,
          <Button
            key="cancel"
            onClick={onClose}
          >
            取消
          </Button>,
          <Button
            key="search"
            type="primary"
            icon={<SearchOutlined />}
            onClick={() => {
              // 数据验证
              if (filterGroups.length > 0) {
                let errCount: number = 0;
                for (let i = 0; i < filterGroups.length; i++) {
                  const group = filterGroups[i];
                  for (let j = 0; j < group.filters.length; j++) {
                    const filter = group.filters[j];
                    // 验证是否有空值
                    if (!filter.key || !filter.operator || !filter.value) {
                      message.error(`第${i + 1}组第${j + 1}条数据有空值`);
                      errCount++;
                      break;
                    }
                  }
                }
                if (errCount > 0) {
                  return;
                }
              }
              setSelfOpen(false);
              onSearch(filterGroups);
            }}
          >
            搜索
          </Button>,
        ]}
      >
        {filterGroups.length === 0 && (
          <Empty
            image="https://gw.alipayobjects.com/zos/antfincdn/ZHrcdLPrvN/empty.svg"
            imageStyle={{ height: 70 }}
            style={{ marginBottom: 100, marginTop: 100 }}
            description={<span>暂无查询条件</span>}
          >
            <Button
              type="primary"
              onClick={() => {
                setFilterGroups([
                  ...filterGroups,
                  {
                    xor: 'and',
                    filters: [
                      {
                        groupIndex: filterGroups.length,
                        index: 0,
                        xor: 'and',
                      },
                    ],
                  },
                ]);
              }}
            >
              添加分组
            </Button>
          </Empty>
        )}
        {filterGroups.map((group, groupIndex) => (
          <ProCard
            title={
              <Space>
                <span>第{groupIndex + 1}组</span>
                {groupIndex > 0 && (
                  <Select
                    style={{ width: '100px' }}
                    placeholder="请选择"
                    options={[
                      {
                        label: '与(&&)',
                        value: 'and',
                      },
                      {
                        label: '或(||)',
                        value: 'or',
                      },
                    ]}
                    value={group.xor}
                    onChange={(value) => {
                      // 更新
                      group.xor = value;
                      setFilterGroups([...filterGroups]);
                    }}
                  />
                )}
              </Space>
            }
            key={groupIndex}
            headerBordered
            bordered
            style={{ marginBottom: 16 }}
            extra={
              <Space>
                <Button
                  onClick={() => {
                    // 添加一条filter记录
                    group.filters.push({
                      groupIndex,
                      xor: 'and',
                    });
                    setFilterGroups([...filterGroups]);
                  }}
                >
                  添加条件
                </Button>
                {groupIndex > 0 && (
                  <Button
                    type={'link'}
                    danger
                    onClick={() => {
                      // 删除分组
                      filterGroups.splice(groupIndex, 1);
                      setFilterGroups([...filterGroups]);
                    }}
                  >
                    删除分组
                  </Button>
                )}
              </Space>
            }
          >
            <div>
              {group.filters.map((item, index) => (
                <RulerItem
                  key={index}
                  item={{ ...item, groupIndex, index }}
                  colSelect={data
                    .filter((p) => !p.hideInSearch)
                    .map(
                      (p) =>
                      ({
                        label: p.title,
                        value: p.propertyName,
                        propertyType: p.propertyType,
                        enumOptions: p.enumOptions,
                      } as ColSelectItem),
                    )}
                  onChange={(value) => {
                    // 更新
                    group.filters[index] = value;
                    setFilterGroups([...filterGroups]);
                  }}
                  onRemoveItem={() => {
                    if (group.filters.length === 1) {
                      // 如果只有一条，删除分组
                      filterGroups.splice(groupIndex, 1);
                      setFilterGroups([...filterGroups]);
                      return;
                    }
                    // 删除
                    group.filters.splice(index, 1);
                    setFilterGroups([...filterGroups]);
                  }}
                />
              ))}
            </div>
          </ProCard>
        ))}
      </Modal>
    </>
  );
};

export default AdvancedSearch;
