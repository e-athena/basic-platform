import { G6TreeGraphData } from '@ant-design/graphs';
import { ModalForm, ProFormSelect, FormInstance } from '@ant-design/pro-components';
import React, { useEffect, useRef } from 'react';
import { queryEventSelectList, querySelectList } from '../service';

type NodeFormProps = {
  onCancel: () => void;
  onSuccess: (items: API.EventTrackingConfigItem[], node: G6TreeGraphData, target: number) => void;
  open: boolean;
  values: API.EventTrackingConfigItem;
  node: G6TreeGraphData;
  target: number;
};

const NodeForm: React.FC<NodeFormProps> = (props) => {
  const formRef = useRef<FormInstance>();
  const [selectList, setSelectList] = React.useState<API.SelectInfo[]>([]);
  const [configDataSource, setConfigDataSource] = React.useState<API.EventTrackingConfigListItem[]>([]);

  useEffect(() => {
    const fetch = async () => {
      const res = await querySelectList();
      if (res.success && res.data !== undefined) {
        setConfigDataSource(res.data);
      }
    }
    if (props.open) {
      fetch();
    }
  }, [props.open]);

  return (
    <ModalForm
      width={520}
      formRef={formRef}
      title={props.target === 0 ? '修改节点配置' : '新增子节点配置'}
      open={props.open}
      modalProps={{
        onCancel: () => {
          props.onCancel();
        },
        bodyStyle: { padding: '32px 40px 48px' },
        destroyOnClose: true,
        maskClosable: false
      }}
      onFinish={async (values: API.EventTrackingConfigItem) => {
        values.eventTypeTitle = values.eventType === 1 ? '领域事件' : '集成事件';
        values.eventTypeFullName = configDataSource.find(item => item.eventTypeName === values.eventTypeName)?.eventTypeFullName || '';
        values.processorName = configDataSource.find(item => item.processorFullName === values.processorFullName)?.processorName || '';
        // 新增
        if (props.target === 1) {
          values.configId = props.values.configId;
          values.parentId = props.values.id;
          values.id = `e${new Date().getTime()}`;
        } else {
          // 更新
          values.id = props.values.id;
        }
        const items = [] as API.EventTrackingConfigItem[];
        if (props.target === 0) {
          items.push(values);
        }
        if (props.target === 1) {
          // 读取eventTypeFullName对应的处理器
          const processorList = configDataSource.filter(x => x.eventTypeFullName === values.eventTypeFullName);
          if (processorList.length > 0) {
            // 添加为node的children
            props.node.children = props.node.children || [];
            for (let i = 0; i < processorList.length; i += 1) {
              const item = processorList[i];
              props.node.children.push({
                id: `e${new Date().getTime()}${i}`,
                value: {
                  title: item.eventName,
                  items: [
                    {
                      text: "事件类型",
                      value: item.eventType === 1 ? '领域事件' : '集成事件'
                    },
                    {
                      text: "事件处理器",
                      value: item.processorName,
                    },
                    {
                      text: "事件实体类型",
                      value: item.eventTypeName,
                    }
                  ]
                },
                children: [],
              });
            }
            // 添加到items
            items.push(...processorList.map((item, zIndex) => ({
              ...item,
              id: `e${new Date().getTime()}${zIndex}`,
              configId: props.values.configId,
              parentId: props.values.id,
            } as API.EventTrackingConfigItem)));
          }
        }
        props.onSuccess(items, props.node!, props.target);
      }}
      initialValues={{
        ...(props.target === 0 ? props.values : undefined)
      }}
    >
      <ProFormSelect
        name="eventTypeName"
        label={'事件实体类型'}
        // options={selectList}
        request={async (params) => {
          if (params.configs.length === 0) {
            return [];
          }
          if (selectList.length > 0) {
            return selectList.filter(item => configDataSource.find(config => config.eventTypeName === item.value));
          }
          const res = await queryEventSelectList();
          const data = res.data || [];
          setSelectList(data);
          return data.filter(item => configDataSource.find(config => config.eventTypeName === item.value));
        }}
        params={{
          configs: configDataSource
        }}
        fieldProps={{
          onChange() {
            formRef.current?.setFieldsValue({
              processorFullName: undefined,
              eventName: undefined,
              eventType: undefined
            });
          },
          showSearch: true,
          allowClear: true,
        }}
        rules={[
          {
            required: true,
            message: '请选择',
          },
        ]}
      />
      {/* <ProFormDependency name={['eventTypeName']}>
        {({ eventTypeName }) => {
          return <ProFormSelect
            name="processorFullName"
            label={'事件处理器'}
            options={configDataSource
              .filter(item => item.eventTypeName === eventTypeName)
              .map(item => ({
                label: item.processorName,
                value: item.processorFullName
              } as API.SelectInfo))}
            rules={[
              {
                required: true,
                message: '请选择',
              },
            ]}
            fieldProps={{
              onChange(value) {
                // 读取选择的项
                const item = configDataSource
                  .filter(item => item.eventTypeName === eventTypeName)
                  .find(item => item.processorFullName === value);
                if (item) {
                  formRef.current?.setFieldsValue({
                    eventName: item.eventName,
                    eventType: item.eventType
                  });
                }
              },
              showSearch: true,
              allowClear: true,
            }}
          />;
        }}
      </ProFormDependency>
      <ProFormText
        name="eventName"
        label={'事件处理备注'}
        tooltip={'可通过选择事件实体类型和事件处理器自动填充'}
        rules={[
          {
            required: true,
            message: '请输入事件名称',
          },
        ]}
      />
      <ProFormSelect
        name="eventType"
        label={'事件类型'}
        tooltip={'通过选择事件实体类型和事件处理器自动填充'}
        disabled
        options={[{
          label: '领域事件',
          value: 1
        }, {
          label: '集成事件',
          value: 2
        }]}
        rules={[
          {
            required: true,
            message: '请选择',
          },
        ]}
      /> */}
    </ModalForm>
  );
};

export default NodeForm;
