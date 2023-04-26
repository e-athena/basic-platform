import pattern from '@/utils/pattern';
import { BarsOutlined, MessageOutlined, SafetyOutlined, UserOutlined } from '@ant-design/icons';
import { ProCard, ProForm, ProFormRadio, ProFormText, ProList } from '@ant-design/pro-components';
// import { useModel } from '@umijs/max';
import { Menu, Switch } from 'antd';
import { useState } from 'react';

const Center = () => {
  // const { initialState } = useModel('@@initialState');
  const [current, setCurrent] = useState('basic');

  return (
    <ProCard split="vertical">
      <ProCard
        colSpan={'250px'}
        bodyStyle={{
          paddingRight: 0,
        }}
      >
        <Menu
          onClick={(e) => {
            // console.log('click ', e);
            setCurrent(e.key);
          }}
          selectedKeys={[current]}
          mode={'vertical'}
          items={[
            {
              label: '基本设置',
              key: 'basic',
              icon: <BarsOutlined />,
            },
            {
              label: '安全设置',
              key: 'security',
              icon: <SafetyOutlined />,
            },
            {
              label: '帐号绑定',
              key: 'account',
              icon: <UserOutlined />,
            },
            {
              label: '新消息通知',
              key: 'notification',
              icon: <MessageOutlined />,
            },
          ]}
        />
      </ProCard>
      <ProCard>
        <div style={{ minHeight: 360, paddingBottom: 24 }}>
          {current === 'basic' && (
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
          {current === 'security' && (
            <>
              <ProList<any>
                rowKey="name"
                headerTitle={'安全设置'}
                cardProps={{
                  bodyStyle: {
                    padding: 0,
                  },
                }}
                dataSource={[
                  {
                    name: '帐户密码',
                    desc: '当前密码强度：弱',
                  },
                  {
                    name: '密保手机',
                    desc: '已绑定手机：166****1061',
                  },
                  {
                    name: '密保问题',
                    desc: '未设置密保问题，密保问题可有效保护账户安全',
                  },
                  {
                    name: '备用邮箱',
                    desc: '已绑定邮箱：ant***sign.com',
                  },
                ]}
                showActions="hover"
                showExtra="hover"
                metas={{
                  title: {
                    dataIndex: 'name',
                  },
                  description: {
                    dataIndex: 'desc',
                  },
                  actions: {
                    render: (text, row) => [
                      <a href={row.html_url} target="_blank" rel="noopener noreferrer" key="link">
                        修改
                      </a>,
                    ],
                  },
                }}
              />
            </>
          )}
          {current === 'account' && (
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
          {current === 'notification' && (
            <>
              <ProList<any>
                rowKey="name"
                headerTitle={'新消息通知'}
                cardProps={{
                  bodyStyle: {
                    padding: 0,
                  },
                }}
                dataSource={[
                  {
                    name: '系统公告',
                    desc: '系统消息将以站内信的形式通知',
                  },
                  {
                    name: '系统消息',
                    desc: '系统消息将以站内信的形式通知',
                  },
                  {
                    name: '待办任务',
                    desc: '系统消息将以站内信的形式通知',
                  },
                ]}
                showActions="hover"
                showExtra="hover"
                metas={{
                  title: {
                    dataIndex: 'name',
                  },
                  description: {
                    dataIndex: 'desc',
                  },
                  actions: {
                    render: () => [
                      <Switch
                        key="switch"
                        defaultChecked
                        checkedChildren="开启"
                        unCheckedChildren="关闭"
                      />,
                    ],
                  },
                }}
              />
            </>
          )}
        </div>
      </ProCard>
    </ProCard>
  );
};

export default Center;
