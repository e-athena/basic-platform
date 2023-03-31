import { SearchOutlined } from '@ant-design/icons';
import { ProCard } from '@ant-design/pro-components';
import { Button, Empty, message, Modal, Select, Space } from 'antd';
import { useEffect, useState } from 'react';
import RulerItem from './components/RulerItem';

type EditTableColumnFormProps = {
  onCancel?: () => void;
  onOk: (data: FilterGroupItem[], saveTemplate: boolean) => void;
  open?: boolean;
  resourceKey: string;
  data: API.DataPermissionProperty[];
  historyFilters?: FilterGroupItem[];
  title?: string;
};

/**
 * 自定义查询
 * @param props
 * @returns
 */
const QueryPolicyModal: React.FC<EditTableColumnFormProps> = (props) => {
  const { open, onCancel, onOk, data, historyFilters } = props;
  const [selfOpen, setSelfOpen] = useState<boolean>(false);
  const [filterGroups, setFilterGroups] = useState<FilterGroupItem[]>(historyFilters || []);

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

  const okHandle = (saveTemplate?: boolean) => {
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
    if (saveTemplate && filterGroups.length === 0) {
      message.error('保存的模板不能为空');
      return;
    }
    setSelfOpen(false);
    onOk(filterGroups, saveTemplate || false);
  }
  return (
    <>
      {props.open === undefined && (
        <Button
          type={'link'}
          style={{ color: '#1f1f1f' }}
          icon={<SearchOutlined />}
          onClick={() => setSelfOpen(true)}
        />
      )}
      <Modal
        title={props.title || '配置查询策略'}
        open={open || selfOpen}
        width={1000}
        bodyStyle={{
          maxHeight: 'calc(100vh - 500px)',
          overflow: 'auto',
          minHeight: 500,
          paddingTop: 15,
        }}
        destroyOnClose
        onCancel={() => {
          onCancel?.();
          setSelfOpen(false);
        }}
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
            }}
          >
            清空
          </Button>,
          <Button key="cancel" onClick={() => {
            onCancel?.();
            setSelfOpen(false);
          }}>
            取消
          </Button>,
          <Button
            key="save"
            type="primary"
            onClick={() => {
              okHandle();
            }}
          >
            保存
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
                  colSelect={data.map((p) => ({
                    label: p.label,
                    value: p.value,
                    propertyType: p.propertyType,
                    enumOptions: p.enumOptions,
                  } as ColSelectItem)
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

export default QueryPolicyModal;
