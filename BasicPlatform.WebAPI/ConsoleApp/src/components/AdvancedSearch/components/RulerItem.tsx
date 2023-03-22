import {
  Button,
  Col,
  Row,
  Select,
  TreeSelect,
  Tooltip,
  DatePicker,
  Radio,
  InputNumber,
  Input,
} from 'antd';
import dayjs from 'dayjs';

const { RangePicker } = DatePicker;

export type FilterGroupItem = {
  xor: string;
  filters: FilterItem[];
};
export type FilterItem = {
  key?: string;
  propertyType?: string;
  label?: string;
  value?: any;
  xor?: string;
  operator?: string;
  groupIndex: number;
  index?: number;
};
type RulerItemProps = {
  item: FilterItem;
  colSelect: ColSelectItem[];
  onChange: (value: FilterItem) => void;
  onRemoveItem?: () => void;
};

export type ColSelectItem = {
  label: string;
  value: string;
  propertyType: string;
  enumOptions: any[];
};

const RulerItem: React.FC<RulerItemProps> = (props) => {
  const rulerSelect = [
    {
      label: '等于',
      value: '==',
    },
    {
      label: '不等于',
      value: '!=',
    },
    {
      label: '大于',
      value: '>',
    },
    {
      label: '小于',
      value: '<',
    },
    {
      label: '大于等于',
      value: '>=',
    },
    {
      label: '小于等于',
      value: '<=',
    },
    {
      label: '包含',
      value: 'contains',
    },
    {
      label: '属于',
      value: 'in',
    },
    {
      label: '含有任意一个',
      value: 'intersect',
    },
  ];
  const { onRemoveItem, item, onChange, colSelect } = props;
  const getOptions = () => {
    return [];
  };
  const getRulerSelect = () => {
    console.log(item);
    // if (isTop) {
    // 如果有UserId关键字
    if (item.key?.includes('UserId')) {
      return [
        {
          label: '属于',
          value: 'in',
        },
      ];
    }
    if (item.key === 'OrganizationIds') {
      return [
        {
          label: '包含',
          value: 'contains',
        },
      ];
    }
    // 有额外规则
    if (getOptions().length > 0) {
      return [
        {
          label: '属于',
          value: 'in',
        },
      ];
    }
    let propertyType = item.propertyType;
    switch (propertyType) {
      case 'string':
        return [
          {
            label: '等于',
            value: '==',
          },
          {
            label: '包含',
            value: 'contains',
          },
        ];
      case 'dateTime':
        return [
          {
            label: '大于',
            value: '>',
          },
          {
            label: '小于',
            value: '<',
          },
          {
            label: '介于',
            value: 'between',
          },
          {
            label: '大于等于',
            value: '>=',
          },
          {
            label: '小于等于',
            value: '<=',
          },
        ];
      case 'number':
        return [
          {
            label: '等于',
            value: '==',
          },
          {
            label: '不等于',
            value: '!=',
          },
          {
            label: '大于',
            value: '>',
          },
          {
            label: '小于',
            value: '<',
          },
          {
            label: '大于等于',
            value: '>=',
          },
          {
            label: '小于等于',
            value: '<=',
          },
        ];
      case 'boolean':
        return [
          {
            label: '等于',
            value: '==',
          },
        ];
      case 'enum':
        return [
          {
            label: '等于',
            value: '==',
          },
          {
            label: '属于',
            value: 'in',
          },
        ];

      default:
        return rulerSelect;
    }
  };

  const getValueDom = () => {
    if (item.key === 'OrganizationIds') {
      return (
        <Tooltip placement={'top'} title="组织架构和指定策略二选一">
          <TreeSelect
            style={{ width: '350px' }}
            value={item.value === undefined || item.value === '' ? [] : item.value.split(',')}
            dropdownStyle={{ maxHeight: 400, overflow: 'auto' }}
            treeData={getOptions()}
            placeholder="请选择组织架构或别名"
            multiple
            treeLine={{
              showLeafIcon: true,
            }}
            treeDefaultExpandAll
            treeCheckable
            showCheckedStrategy="SHOW_ALL"
            onChange={(value) => {
              const newItem = { ...item };
              if (value.length === 0) {
                newItem.value = undefined;
              } else {
                if (value.includes('{OrganizationIds}')) {
                  newItem.value = '{OrganizationIds}';
                } else {
                  newItem.value = value.join(','); // value[value.length - 1];
                }
              }
              onChange(newItem);
            }}
          />
        </Tooltip>
      );
    }
    const options = getOptions();
    if (options.length > 0) {
      return (
        <Select
          autoClearSearchValue
          options={getOptions()}
          mode="tags"
          style={{ width: '350px' }}
          placeholder="请选择"
          value={item.value === undefined || item.value === '' ? [] : [item.value]}
          onChange={(value) => {
            const newItem = { ...item };
            if (value.length === 0) {
              newItem.value = undefined;
            } else {
              newItem.value = value[value.length - 1];
            }
            onChange(newItem);
          }}
        />
      );
    }
    // // 如果是用户类型
    // if (item.key?.includes('UserId')) {
    //   return <span>aaaa</span>
    // }
    // 如果是枚举类型
    if (item.propertyType === 'enum') {
      let mode: string | undefined = 'tags';
      if (item.operator === '==') {
        mode = undefined;
      }
      let value: string[] | string | undefined = [];
      if (item.value === undefined || item.value === '') {
        if (item.operator === '==') {
          value = undefined;
        } else {
          value = [];
        }
      } else {
        if (item.operator === '==') {
          // 如果上一个值是数组，那么就取第一个
          const arr = item.value.split(',');
          value = arr.length > 0 ? arr[0] : item.value;
        } else {
          value = item.value.split(',');
        }
      }
      return (
        <Select
          autoClearSearchValue
          options={colSelect.find((p) => p.value === item.key)?.enumOptions}
          mode={mode as 'tags' | undefined}
          style={{ width: '350px' }}
          placeholder="请选择"
          value={value}
          onChange={(value) => {
            const newItem = { ...item };
            if (value) {
              if (item.operator === '==') {
                newItem.value = value as string;
              } else {
                newItem.value = (value as string[]).join(',');
              }
            } else {
              newItem.value = value;
            }
            onChange(newItem);
          }}
        />
      );
    }
    // 如果是时间类型
    if (item.propertyType === 'dateTime') {
      if (item.operator === 'between') {
        return (
          <RangePicker
            style={{ width: '350px' }}
            placeholder={['开始时间', '结束时间']}
            value={
              item.value === '' || item.value === undefined || item.value?.split(',')?.length !== 2
                ? null
                : [dayjs(item.value.split(',')[0]), dayjs(item.value.split(',')[1])]
            }
            onChange={(values) => {
              const newItem = { ...item };
              if (values?.length === 2) {
                newItem.value = `${values[0]!.format('YYYY-MM-DD')},${values[1]!.format(
                  'YYYY-MM-DD',
                )}`;
              } else {
                newItem.value = undefined;
              }
              onChange(newItem);
            }}
          />
        );
      }
      return (
        <DatePicker
          showTime
          style={{ width: '350px' }}
          placeholder="请选择时间"
          value={item.value === undefined || item.value === '' ? null : dayjs(item.value)}
          onChange={(value) => {
            const newItem = { ...item };
            if (!value) {
              newItem.value = undefined;
            } else {
              newItem.value = value.format('YYYY-MM-DD HH:mm:ss');
            }
            onChange(newItem);
          }}
        />
      );
    }
    // 如果是布尔类型
    if (item.propertyType === 'boolean') {
      return (
        <Radio.Group
          style={{ width: '350px', paddingTop: 5 }}
          value={item.value === '' ? null : item.value}
          onChange={(e) => {
            const newItem = { ...item };
            newItem.value = e.target.value;
            onChange(newItem);
          }}
        >
          <Radio value={'1'}>是</Radio>
          <Radio value={'0'}>否</Radio>
        </Radio.Group>
      );
    }
    // 如果是数值类型
    if (item.propertyType === 'number') {
      return (
        <InputNumber
          style={{ width: '350px' }}
          placeholder="请输入数值"
          value={item.value}
          onChange={(value) => {
            const newItem = { ...item };
            newItem.value = value;
            onChange(newItem);
          }}
        />
      );
    }
    return (
      <Input
        allowClear
        placeholder="请输入关键字"
        style={{ width: '350px' }}
        value={item.value}
        onChange={(e) => {
          const newItem = { ...item };
          newItem.value = e.target.value;
          onChange(newItem);
        }}
      />
    );
  };
  if (item.operator === '' && item.key !== '') {
    const newItem = { ...item };
    newItem.operator = getRulerSelect()[0]?.value;
    onChange(newItem);
  }
  return (
    <div style={{ margin: '5px 0', border: '1px solid #f2f2f2', padding: '5px 10px' }}>
      <Row gutter={[24, 24]}>
        <Col span={90}>
          {item.index && item.index > 0 ? (
            <Select
              style={{ width: 90 }}
              placeholder="与或"
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
              value={item.xor}
              onChange={(value) => {
                const newItem = { ...item };
                newItem.xor = value;
                onChange(newItem);
              }}
            />
          ) : (
            <div style={{ width: 90 }}></div>
          )}
        </Col>
        <Col span={'200px'}>
          <Select
            style={{ width: '190px' }}
            showSearch
            placeholder="选择筛选字段"
            options={colSelect}
            value={item.key === '' ? null : item.key}
            onChange={(value) => {
              const newItem = { ...item };
              newItem.key = value;
              // 字段变更，重置值
              newItem.value = undefined;
              // 字段变更，重置规则
              newItem.operator = undefined;
              newItem.propertyType = colSelect.find((p) => p.value === value)!.propertyType;
              onChange(newItem);
            }}
          />
        </Col>
        <Col span={'160px'}>
          <Select
            style={{ width: '100px' }}
            showSearch
            placeholder="运算符"
            options={getRulerSelect()}
            value={item.operator === '' ? null : item.operator}
            onChange={(value) => {
              const newItem = { ...item };
              newItem.operator = value;
              newItem.value = undefined;
              onChange(newItem);
            }}
          />
        </Col>
        <Col span={'180px'}>{getValueDom()}</Col>
        <Col span={'80px'} style={{ paddingTop: 5 }}>
          <Button
            size={'small'}
            type={'dashed'}
            danger
            onClick={() => {
              onRemoveItem?.();
            }}
          >
            移除
          </Button>
        </Col>
      </Row>
    </div>
  );
};
export default RulerItem;
