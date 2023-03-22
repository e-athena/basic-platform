import { submitHandle } from '@/utils/utils';
import {
  ProFormText,
  ProFormTextArea,
  ModalForm,
  ProFormSelect,
  ProFormRadio,
  ProFormInstance,
  ProFormDigit,
  ProForm,
  ProFormSwitch,
} from '@ant-design/pro-components';
import React from 'react';
import { update, create, selectList } from '../service';
import pattern from '@/utils/pattern';
import { useModel } from '@umijs/max';

type CreateOrUpdateFormProps = {
  onCancel: () => void;
  onSuccess: () => void;
  open: boolean;
  values?: API.ExternalPageDetailItem;
};

const CreateOrUpdateForm: React.FC<CreateOrUpdateFormProps> = (props) => {
  const formRef = React.useRef<ProFormInstance>();
  const { initialState } = useModel('@@initialState');
  const isRoot = initialState?.currentUser?.userName === 'root';
  const canSelectGroup = !(props.values?.id !== undefined && props.values.isGroup);
  return (
    <>
      <ModalForm
        width={600}
        formRef={formRef}
        title={props.values?.id === undefined ? '创建外部页面' : '更新外部页面'}
        open={props.open}
        modalProps={{
          onCancel: () => {
            props.onCancel();
          },
          bodyStyle: { padding: '32px 40px 48px' },
          destroyOnClose: true,
        }}
        onFinish={async (values: API.UpdateExternalPageRequest) => {
          const isUpdate = props.values?.id !== undefined;
          let succeed;
          if (isUpdate) {
            values.id = props.values!.id!;
            succeed = await submitHandle(update, values);
          } else {
            succeed = await submitHandle(create, values as API.CreateExternalPageRequest);
          }
          if (succeed) {
            props.onSuccess();
          }
        }}
        initialValues={{
          isPublic: true,
          sort: 0,
          type: 2,
          layout: 'default',
          ...props.values,
        }}
      >
        <ProForm.Group>
          <ProFormSelect
            name="parentId"
            label="所属分组"
            width={'sm'}
            tooltip={
              canSelectGroup
                ? undefined
                : '如果想变成二级页面，则需要先把自身子页面全部删除或移动到其他分组'
            }
            placeholder={'默认为一级页面'}
            disabled={!canSelectGroup}
            showSearch
            request={async () => {
              const { data } = await selectList();
              return data || [];
            }}
          />
          <ProFormText
            name="name"
            label={'名称'}
            width={'sm'}
            rules={[
              {
                required: true,
                message: '请输入',
              },
            ]}
          />
        </ProForm.Group>
        <ProFormText
          name="path"
          label={'跳转地址'}
          tooltip={'示例:https://www.baidu.com'}
          rules={[
            {
              required: true,
              message: '请输入',
            },
            {
              pattern: pattern.url,
              message: '请输入正确的url',
            },
          ]}
        />
        <ProFormRadio.Group
          name="layout"
          label="布局"
          options={[
            {
              label: '默认',
              value: 'default',
            },
            {
              label: 'top',
              value: 'top',
            },
            {
              label: 'side',
              value: 'side',
            },
            {
              label: 'mix',
              value: 'mix',
            },
          ]}
        />
        <ProForm.Group>
          <ProFormDigit name="sort" label={'排序'} width={'sm'} min={0} />
          <ProFormRadio.Group
            name="type"
            label="跳转类型"
            tooltip={'外部链接：新窗口打开，内部链接：系统内部打开'}
            options={[
              {
                label: '外部链接',
                value: 1,
              },
              {
                label: '内部链接',
                value: 2,
              },
            ]}
          />
        </ProForm.Group>
        <ProFormTextArea name="remarks" label={'备注'} placeholder={'请输入'} />
        {isRoot && (
          <ProFormSwitch name="isPublic" label="通用页面" tooltip={'为true时其他用户也能看到'} />
        )}
      </ModalForm>
    </>
  );
};

export default CreateOrUpdateForm;
