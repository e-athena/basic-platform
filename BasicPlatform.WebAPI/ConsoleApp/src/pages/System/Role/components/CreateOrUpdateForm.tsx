import { submitHandle } from '@/utils/utils';
import {
  ProFormText,
  ProFormTextArea,
  ModalForm,
  ProFormSelect,
  ProFormDependency,
} from '@ant-design/pro-components';
import { Button, FormInstance } from 'antd';
import React, { useEffect, useRef, useState } from 'react';
import { update, create } from '../service';
import OrgModal from '@/components/OrgModal';
import { TransferOrgInfo } from '@/components/OrgModal/components/TransferForm';

type CreateOrUpdateFormProps = {
  onCancel: () => void;
  onSuccess: () => void;
  open: boolean;
  values?: API.RoleDetailItem;
};

const CreateOrUpdateForm: React.FC<CreateOrUpdateFormProps> = (props) => {
  const [orgModalOpen, setOrgModalOpen] = useState<boolean>(false);
  const [dataScopeCustomOptions, setDataScopeCustomOptions] = useState<API.SelectInfo[]>([]);
  const formRef = useRef<FormInstance>();
  useEffect(() => {
    if (props.open && props.values !== undefined) {
      setDataScopeCustomOptions(props.values.dataScopeCustomSelectList || []);
    }
  }, [props.values, props.open]);
  return (
    <ModalForm
      width={530}
      formRef={formRef}
      title={props.values?.id === undefined ? '创建角色' : '更新角色'}
      open={props.open}
      modalProps={{
        onCancel: () => {
          props.onCancel();
        },
        bodyStyle: { padding: '32px 40px 48px' },
        destroyOnClose: true,
      }}
      onFinish={async (values: API.UpdateRoleItem) => {
        const isUpdate = props.values !== undefined;
        if (isUpdate) {
          values.id = props.values!.id!;
        }
        const req = isUpdate ? update : create;
        const succeed = await submitHandle(req, values);
        if (succeed) {
          props.onSuccess();
        }
      }}
      initialValues={{
        ...props?.values,
      }}
    >
      <ProFormText
        name="name"
        label={'名称'}
        // width="md"
        rules={[
          {
            required: true,
            message: '请输入角色名称',
          },
        ]}
      />
      <ProFormSelect
        label={'默认的数据访问范围'}
        name="dataScope"
        showSearch
        allowClear
        tooltip={'全局生效，可针对不同模块进行单独设置，详见分配权限功能。'}
        options={[
          {
            value: 0,
            label: '全部',
          },
          {
            value: 1,
            label: '本人',
          },
          {
            value: 2,
            label: '本部门',
          },
          {
            value: 3,
            label: '本部门及下属部门',
          },
          {
            value: 4,
            label: '自定义',
          },
        ]}
      />
      <ProFormDependency name={['dataScope']}>
        {({ dataScope }) => {
          return (
            <>
              {dataScope === 4 && (
                <ProFormSelect
                  name="dataScopeCustomList"
                  disabled
                  label={'自定义数据访问范围'}
                  placeholder={'请选择'}
                  tooltip={'指定角色可以访问的数据范围。'}
                  mode={'tags'}
                  width="md"
                  allowClear
                  options={dataScopeCustomOptions || props.values?.dataScopeCustomSelectList || []}
                  fieldProps={{
                    style: {
                      backgroundColor: '#ffffff',
                    },
                  }}
                  rules={[
                    {
                      required: true,
                      message: '请选择自定义数据访问范围',
                    },
                  ]}
                  addonAfter={
                    <Button
                      onClick={() => {
                        setOrgModalOpen(true);
                      }}
                    >
                      选择
                    </Button>
                  }
                />
              )}
            </>
          );
        }}
      </ProFormDependency>
      <ProFormTextArea
        name="remarks"
        // width="md"
        label={'描述'}
        placeholder={'请输入'}
      />
      <OrgModal
        mode={'multiple'}
        open={orgModalOpen}
        onCancel={() => {
          setOrgModalOpen(false);
        }}
        onOk={(keys: string[], rows: TransferOrgInfo[]) => {
          setDataScopeCustomOptions(
            rows.map((item) => {
              return {
                label: item.name,
                value: item.id,
                disabled: false,
              };
            }),
          );
          formRef.current?.setFieldsValue({
            dataScopeCustomList: keys,
          });
          setOrgModalOpen(false);
        }}
        defaultSelectedKeys={
          dataScopeCustomOptions.length > 0 ? dataScopeCustomOptions.map((p) => p.value) : []
        }
      />
    </ModalForm>
  );
};

export default CreateOrUpdateForm;
