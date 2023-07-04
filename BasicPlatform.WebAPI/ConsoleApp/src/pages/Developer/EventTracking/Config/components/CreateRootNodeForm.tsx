import { G6TreeGraphData } from '@ant-design/graphs';
import { ModalForm, ProFormText, ProFormSelect, FormInstance } from '@ant-design/pro-components';
import React, { useEffect, useRef } from 'react';
import { querySelectList } from '../service';

type CreateRootNodeFormProps = {
  onCancel: () => void;
  onSuccess: (item: API.EventTrackingConfigItem, node: G6TreeGraphData) => void;
  open: boolean;
};

const CreateRootNodeForm: React.FC<CreateRootNodeFormProps> = (props) => {
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
        values.eventTypeName = selectList.find(item => item.value === values.eventTypeFullName)!.label;
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
        props.onSuccess(values, node);
      }}
    >
      <ProFormText
        name="eventName"
        label={'事件名称'}
        rules={[
          {
            required: true,
            message: '请输入事件名称',
          },
        ]}
      />
      <ProFormSelect
        name="eventTypeFullName"
        label={'事件实体类型'}
        options={selectList}
        fieldProps={{
          onChange(value) {
            formRef.current?.setFieldsValue({
              eventName: selectListDataSource.find(x => x.eventTypeFullName === value)?.eventName,
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
    </ModalForm>
  );
};

export default CreateRootNodeForm;
