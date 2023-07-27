import { G6TreeGraphData } from '@ant-design/graphs';
import { ModalForm, ProFormSelect, FormInstance } from '@ant-design/pro-components';
import React, { useEffect, useRef } from 'react';
import { querySelectList, queryEventSelectList } from '../service';

type CreateRootNodeFormProps = {
  onCancel: () => void;
  onSuccess: (items: API.EventTrackingConfigItem[], node: G6TreeGraphData) => void;
  open: boolean;
};

const CreateRootNodeForm: React.FC<CreateRootNodeFormProps> = (props) => {
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
      title={'创建根节点配置'}
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
        const items = [] as API.EventTrackingConfigItem[];
        values.eventTypeFullName = configDataSource.find(item => item.eventTypeName === values.eventTypeName)!.eventTypeFullName;
        values.eventName = selectList.find(x => x.value === values.eventTypeName)!.label;
        const node: G6TreeGraphData = {
          id: `e${new Date().getTime()}`,
          value: {
            title: values.eventName,
            items: [
              {
                text: "事件实体类型",
                value: values.eventTypeName,
              }
            ]
          },
          children: [],
        };
        values.id = node.id;
        items.push(values);
        // 读取eventTypeFullName对应的处理器
        const processorList = configDataSource.filter(x => x.eventTypeFullName === values.eventTypeFullName);
        if (processorList.length > 0) {
          // 添加为node的children
          node.children = processorList.map((item, index) => ({
            id: `e${new Date().getTime()}${index}`,
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
          }));
          // 添加到items
          items.push(...processorList.map((item, zIndex) => ({
            ...item,
            id: `e${new Date().getTime()}${zIndex}`,
            configId: values.id,
            parentId: values.id,
          } as API.EventTrackingConfigItem)));
        }
        // console.log(items);

        props.onSuccess(items, node);
      }}
    >
      <ProFormSelect
        name="eventTypeName"
        label={'事件类型'}
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
    </ModalForm>
  );
};

export default CreateRootNodeForm;
