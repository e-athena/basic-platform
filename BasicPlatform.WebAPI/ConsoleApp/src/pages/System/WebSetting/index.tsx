import { PageContainer, ProForm, ProFormInstance, ProFormText, ProFormTextArea, ProFormUploadButton } from '@ant-design/pro-components';
import { useModel, useLocation } from '@umijs/max';
import React, { } from 'react';
import { detail, save } from './service';
import { submitHandle } from '@/utils/utils';
import { message } from 'antd';
import { getToken } from '@/utils/token';
const WebSetting: React.FC = () => {
  const { getResource } = useModel('resource');
  const location = useLocation();
  const resource = getResource(location.pathname);

  const formRef = React.useRef<ProFormInstance<WebSetting>>();

  return (
    <PageContainer
      header={{
        title: resource?.name,
        children: resource?.description,
      }}
    >
      <ProForm<WebSetting>
        onFinish={async (values) => {
          console.log(values);
          if (values.logo.length === 0) {
            values.logo = null;
          }
          if (values.logo.length > 0) {
            values.logo = values.logo[0].url.replace(API_URL, '');
          }
          const succeed = await submitHandle(save, values, '', true);
          if (succeed) {
            message.success('保存成功，刷新页面生效。');
          }
        }}
        formRef={formRef}
        request={async () => {
          const res = await detail();
          if (res.data === null) {
            return {
              logo: [{
                url: 'https://cdn.gzwjz.com/FmzrX15jYA03KMVfbgMJnk-P6WGl.png'
              }],
              name: 'Athena Pro',
              shortName: 'Athena Pro',
              description: '.NET Core下更好用且功能强大的通用基础权限管理平台',
              copyRight: 'Athena Pro',
            };
          }
          // 处理logo，如果不包含http或者https，则加上API_URL
          if (res.data!.logo && res.data!.logo.indexOf('http') === -1) {
            res.data!.logo = API_URL + res.data!.logo;
          }

          return {
            ...(res.data || {}),
            logo: [{
              url: res.data!.logo || 'https://cdn.gzwjz.com/FmzrX15jYA03KMVfbgMJnk-P6WGl.png'
            }]
          };
        }}
        autoFocusFirstInput
      >
        <ProFormUploadButton
          label="LOGO"
          name="logo"
          width={'lg'}
          action={API_URL + '/api/file/upload?dir=logo'}
          fieldProps={{
            maxCount: 1,
            headers: {
              Authorization: getToken(),
            }
          }}
          buttonProps={{
            style: {
              border: 0,
              boxShadow: 'none',
              backgroundColor: 'transparent'
            }
          }}
          listType="picture-circle"
          onChange={(info) => {
            if (info.file.status === 'done') {
              formRef.current?.setFieldsValue({
                logo: [{
                  url: `${API_URL}${info.file.response.data}`
                }]
              });
            }
          }}
        />
        <ProFormText
          name="name"
          width={'lg'}
          label="名称"
          rules={[{
            required: true,
            message: '请输入名称'
          }]}
        />
        <ProFormText
          name="shortName"
          width={'lg'}
          label="简称"
          rules={[{
            required: true,
            message: '请输入简称'
          }]}
        />
        <ProFormTextArea
          name="description"
          width={'lg'}
          label="描述"
        />
        <ProFormText
          name="copyRight"
          width={'lg'}
          label="版权"
        />
      </ProForm>
    </PageContainer>
  );
};

export default WebSetting;
