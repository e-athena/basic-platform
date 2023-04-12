import pattern from '@/utils/pattern';
import { AntDesignOutlined } from '@ant-design/icons';
import { ProCard, ProForm, ProFormRadio, ProFormText } from '@ant-design/pro-components';
import { useModel } from '@umijs/max';
import { Avatar, Descriptions, Divider, Segmented } from 'antd';
import { useState } from 'react';
import OrgChart from '@twp0217/react-org-chart';

const Center = () => {
  const { initialState } = useModel('@@initialState');
  const [segmentedValue, setSegmentedValue] = useState<string>('基础信息');
  return (
    <ProCard split="vertical">
      <ProCard colSpan="30%">
        <div style={{ textAlign: 'center', padding: 24 }}>
          <Avatar
            size={{ xs: 24, sm: 32, md: 40, lg: 64, xl: 80, xxl: 100 }}
            icon={<AntDesignOutlined />}
            src={initialState?.currentUser?.avatar}
          />
          <h1 style={{ marginTop: 16 }}>{initialState?.currentUser?.realName}</h1>
          <p>{initialState?.currentUser?.signature}</p>
          <Descriptions size={'default'} column={1}>
            <Descriptions.Item label={'邮箱号'}>
              {initialState?.currentUser?.email}
            </Descriptions.Item>
            <Descriptions.Item label={'手机号'}>
              {initialState?.currentUser?.phoneNumber}
            </Descriptions.Item>
          </Descriptions>
          <Divider />
        </div>
      </ProCard>
      <ProCard
        title={
          <Segmented
            options={['基础信息', '组织架构', '修改密码']}
            value={segmentedValue}
            onChange={(value) => {
              setSegmentedValue(value.toString());
            }}
          />
        }
      >
        <div style={{ minHeight: 360, paddingBottom: 24 }}>
          {segmentedValue === '基础信息' && (
            <>
              <ProForm>
                <ProForm.Group>
                  <ProFormText
                    name="userName"
                    label={'登录名'}
                    width="md"
                    rules={[
                      {
                        required: true,
                        message: '请输入登录名',
                      },
                    ]}
                  />
                  <ProFormText
                    name="realName"
                    label={'姓名'}
                    width="md"
                    rules={[
                      {
                        required: true,
                        message: '请输入真实姓名',
                      },
                    ]}
                  />
                </ProForm.Group>
                <ProForm.Group>
                  <ProFormText name="NickName" label={'昵称'} width="md" />
                  <ProFormRadio.Group
                    name="gender"
                    label="性别"
                    width="md"
                    options={[
                      {
                        label: '男',
                        value: 1,
                      },
                      {
                        label: '女',
                        value: 2,
                      },
                    ]}
                  />
                </ProForm.Group>
                <ProForm.Group>
                  <ProFormText
                    name="phoneNumber"
                    label={'手机号'}
                    width="md"
                    rules={[
                      {
                        message: '请输入正确的手机号!',
                        pattern: pattern.mobile,
                      },
                    ]}
                  />
                  <ProFormText
                    name="email"
                    label={'邮箱'}
                    width="md"
                    rules={[
                      {
                        message: '请输入正确的邮箱!',
                        pattern: pattern.email,
                      },
                    ]}
                  />
                </ProForm.Group>
              </ProForm>
            </>
          )}
          {segmentedValue === '组织架构' && (
            <>
              <OrgChart
                data={{
                  key: 0,
                  label: '科技有限公司',
                  children: [
                    {
                      key: 1,
                      label: '研发部',
                      children: [
                        { key: 11, label: '开发-前端' },
                        { key: 12, label: '开发-后端' },
                        { key: 13, label: 'UI设计' },
                        { key: 14, label: '产品经理' },
                      ],
                    },
                    {
                      key: 2,
                      label: '销售部',
                      children: [
                        { key: 21, label: '销售一部' },
                        { key: 22, label: '销售二部' },
                      ],
                    },
                    { key: 3, label: '财务部' },
                    { key: 4, label: '人事部' },
                  ],
                }}
              />
            </>
          )}
          {segmentedValue === '修改密码' && (
            <>
              <ProForm>
                <ProForm.Group>
                  <ProFormText.Password
                    name="oldPassword"
                    label={'旧密码'}
                    width="md"
                    rules={[
                      {
                        required: true,
                        message: '请输入旧密码',
                      },
                    ]}
                  />
                  <ProFormText.Password
                    name="newPassword"
                    label={'新密码'}
                    width="md"
                    rules={[
                      {
                        required: true,
                        message: '请输入新密码',
                      },
                    ]}
                  />
                </ProForm.Group>
                <ProForm.Group>
                  <ProFormText.Password
                    name="confirmPassword"
                    label={'确认密码'}
                    width="md"
                    rules={[
                      {
                        required: true,
                        message: '请再次输入新密码',
                      },
                      // 检查密码是否一致
                      ({ getFieldValue }) => ({
                        validator(_, value) {
                          if (!value || getFieldValue('newPassword') === value) {
                            return Promise.resolve();
                          }
                          return Promise.reject(new Error('两次输入的密码不一致!'));
                        },
                      }),
                    ]}
                  />
                </ProForm.Group>
              </ProForm>
            </>
          )}
        </div>
      </ProCard>
    </ProCard>
  );
};

export default Center;
