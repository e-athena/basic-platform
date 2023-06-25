import { submitHandle } from '@/utils/utils';
import { ProFormText, ProFormTextArea, ModalForm, ProForm, ProFormSwitch } from '@ant-design/pro-components';
import { FormInstance, Select } from 'antd';
import React, { useRef, useState, useEffect } from 'react';
import { update, create } from '../service';

type CreateOrUpdateFormProps = {
  onCancel: () => void;
  onSuccess: () => void;
  open: boolean;
  values?: API.ApplicationDetailItem;
};

const CreateOrUpdateForm: React.FC<CreateOrUpdateFormProps> = (props) => {
  const formRef = useRef<FormInstance>();
  const [apiUrlAgreement, setApiUrlAgreement] = useState<string>('http://');
  const [frontendUrlAgreement, setFrontendUrlAgreement] = useState<string>('http://');
  const [apiUrl, setApiUrl] = useState<string>('');
  useEffect(() => {
    if (props.open && props.values?.id) {
      setApiUrlAgreement(props.values.apiUrl?.split('://')[0] + '://');
      if (props.values.frontendUrl) {
        setFrontendUrlAgreement(props.values.frontendUrl?.split('://')[0] + '://');
      }
      setApiUrl(props.values.apiUrl?.split('://')[1] || '');
    }
  }, [props.open, props.values]);
  return (
    <ModalForm
      width={816}
      formRef={formRef}
      title={props.values === undefined ? '创建应用' : '更新应用'}
      open={props.open}
      modalProps={{
        onCancel: () => {
          props.onCancel();
          setApiUrl('');
        },
        bodyStyle: { padding: '32px 40px 48px' },
        destroyOnClose: true,
      }}
      onFinish={async (values: API.UpdateApplicationItem) => {
        const isUpdate = props.values?.id !== undefined;
        if (isUpdate) {
          values.id = props.values!.id!;
        }
        values.apiUrl = apiUrlAgreement + apiUrl;
        if (values.frontendUrl) {
          values.frontendUrl = frontendUrlAgreement + values.frontendUrl;
        }
        const req = isUpdate ? update : create;
        const succeed = await submitHandle(req, values);
        if (succeed) {
          props.onSuccess();
          setApiUrl('');
        }
      }}
      initialValues={{
        menuResourceRoute: '/api/external/get-menu-resources',
        permissionResourceRoute: '/api/external/get-data-permission-resources',
        ...props?.values,
        apiUrl: props?.values?.apiUrl?.replace(apiUrlAgreement, ''),
        frontendUrl: props?.values?.frontendUrl?.replace(frontendUrlAgreement, ''),
      }}
    >
      <ProFormText
        name="name"
        label={'名称'}
        rules={[
          {
            required: true,
            message: '请输入角色名称',
          },
        ]}
      />
      <ProForm.Group>
        <ProFormText
          name="clientId"
          label={'客户端ID'}
          width={'sm'}
          tooltip={'系统内唯一，用于配置SSO登录'}
          rules={[
            {
              required: true,
              message: '请输入客户端ID',
            },
          ]}
        />
        <ProFormText
          name="clientSecret"
          label={'客户端密钥'}
          width={'lg'}
          tooltip={'用于配置SSO登录'}
          placeholder={'密钥由系统生成'}
          disabled
        />
      </ProForm.Group>
      <ProFormText
        name="frontendUrl"
        label={'前端地址'}
        fieldProps={{
          addonBefore: (
            <>
              <Select
                value={frontendUrlAgreement}
                onChange={(value) => {
                  setFrontendUrlAgreement(value);
                }}
              >
                <Select.Option value="http://">http://</Select.Option>
                <Select.Option value="https://">https://</Select.Option>
              </Select>
            </>
          ),
        }}
      />
      <ProFormText
        name="apiUrl"
        label={'接口地址'}
        fieldProps={{
          addonBefore: (
            <>
              <Select
                value={apiUrlAgreement}
                onChange={(value) => {
                  setApiUrlAgreement(value);
                }}
              >
                <Select.Option value="http://">http://</Select.Option>
                <Select.Option value="https://">https://</Select.Option>
              </Select>
            </>
          ),
          onChange: (e) => {
            setApiUrl(e.target.value);
          },
        }}
        rules={[
          {
            required: true,
            message: '请输入接口地址',
          },
        ]}
      />
      <ProFormText
        name="menuResourceRoute"
        label={'菜单资源地址'}
        fieldProps={{
          addonBefore: apiUrl ? `${apiUrlAgreement}${apiUrl}` : undefined,
        }}
      />
      <ProFormText
        name="permissionResourceRoute"
        label={'权限资源地址'}
        fieldProps={{
          addonBefore: apiUrl ? `${apiUrlAgreement}${apiUrl}` : undefined,
        }}
      />
      <ProFormTextArea name="remarks" label={'描述'} placeholder={'请输入'} />
      <ProFormSwitch
        name="useDefaultClientSecret"
        label={'使用系统默认的客户端密钥'}
      />
    </ModalForm>
  );
};

export default CreateOrUpdateForm;
