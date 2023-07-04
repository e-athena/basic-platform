import { G6TreeGraphData } from '@ant-design/graphs';
import { ModalForm, ProFormText, ProFormSelect, FormInstance, ProFormDependency } from '@ant-design/pro-components';
import React, { useEffect, useRef } from 'react';
import { querySelectList } from '../service';

type NodeFormProps = {
  onCancel: () => void;
  onSuccess: (item: API.EventTrackingConfigItem, node: G6TreeGraphData, target: number) => void;
  open: boolean;
  values: API.EventTrackingConfigItem;
  node: G6TreeGraphData;
  target: number;
};

const NodeForm: React.FC<NodeFormProps> = (props) => {
  const formRef = useRef<FormInstance>();
  const [selectList, setSelectList] = React.useState<API.SelectInfo[]>([]);
  const [selectListDataSource, setSelectListDataSource] = React.useState<API.EventTrackingConfigListItem[]>([]);

  useEffect(() => {
    const fetch = async () => {
      const res = await querySelectList();
      if (res.success && res.data !== undefined) {
        const d = res.data.map(item => ({
          label: item.eventTypeName,
          value: item.eventTypeFullName
        } as API.SelectInfo));
        // 去重
        const list = d.filter((item, index) => d.findIndex(i => i.value === item.value) === index);
        setSelectList(list);
        setSelectListDataSource(res.data);
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
        values.eventTypeName = selectListDataSource.find(item => item.eventTypeFullName === values.eventTypeFullName)?.eventTypeName || '';
        values.processorName = selectListDataSource.find(item => item.processorFullName === values.processorFullName)?.processorName || '';
        // 新增
        if (props.target === 1) {
          values.configId = props.values.configId;
          values.parentId = props.values.id;
          values.id = `e${new Date().getTime()}`;
        } else {
          // 更新
          values.id = props.values.id;
        }
        props.onSuccess(values, props.node!, props.target);
      }}
      initialValues={{
        ...(props.target === 0 ? props.values : undefined)
      }}
    >
      <ProFormSelect
        name="eventTypeFullName"
        label={'事件实体类型'}
        options={selectList}
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
      <ProFormDependency name={['eventTypeFullName']}>
        {({ eventTypeFullName }) => {
          return <ProFormSelect
            name="processorFullName"
            label={'事件处理器'}
            options={selectListDataSource
              .filter(item => item.eventTypeFullName === eventTypeFullName)
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
                const item = selectListDataSource
                  .filter(item => item.eventTypeFullName === eventTypeFullName)
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
        label={'事件名称'}
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
      />
    </ModalForm>
  );
};

export default NodeForm;
