import Footer from '@/components/Footer';
import { getAuthToken, login } from '@/services/ant-design-pro/api';
import { getFakeCaptcha } from '@/services/ant-design-pro/login';
import {
  AlipayCircleOutlined,
  LockOutlined,
  MobileOutlined,
  TaobaoCircleOutlined,
  UserOutlined,
  WeiboCircleOutlined,
} from '@ant-design/icons';
import {
  LoginForm,
  ProFormCaptcha,
  ProFormCheckbox,
  ProFormText,
} from '@ant-design/pro-components';
import { useEmotionCss } from '@ant-design/use-emotion-css';
import { FormattedMessage, SelectLang, useIntl, useModel, Helmet, history } from '@umijs/max';
import { parse } from 'querystring';
import { Alert, message, Tabs, Tag } from 'antd';
import Settings from '../../../../config/defaultSettings';
import React, { useEffect, useState } from 'react';
import { flushSync } from 'react-dom';
import { setSessionCode, setToken } from '@/utils/token';
import { useLocalStorageState } from 'ahooks';

const ActionIcons = () => {
  const langClassName = useEmotionCss(({ token }) => {
    return {
      marginLeft: '8px',
      color: 'rgba(0, 0, 0, 0.2)',
      fontSize: '24px',
      verticalAlign: 'middle',
      cursor: 'pointer',
      transition: 'color 0.3s',
      '&:hover': {
        color: token.colorPrimaryActive,
      },
    };
  });

  return (
    <>
      <AlipayCircleOutlined
        key="AlipayCircleOutlined"
        className={langClassName}
        onClick={() => {
          // console.log(window.location)
          // window.location.href = 'http://localhost:5218/SSO/Login?clientId=web1&redirectUrl=' + window.location.href;
          window.location.href =
            'http://localhost:5079/user/login-redirect?clientId=web1&redirectUrl=http%3A%2F%2Flocalhost%3A5079%2F%23%2Fuser%2Flogin%3Fredirect%3D%252F';
        }}
      />
      <TaobaoCircleOutlined key="TaobaoCircleOutlined" className={langClassName} />
      <WeiboCircleOutlined key="WeiboCircleOutlined" className={langClassName} />
    </>
  );
};

const Lang = () => {
  const langClassName = useEmotionCss(({ token }) => {
    return {
      width: 42,
      height: 42,
      lineHeight: '42px',
      position: 'fixed',
      right: 16,
      borderRadius: token.borderRadius,
      ':hover': {
        backgroundColor: token.colorBgTextHover,
      },
    };
  });

  return (
    <div className={langClassName} data-lang="true">
      {SelectLang && <SelectLang />}
    </div>
  );
};

const LoginMessage: React.FC<{
  content: string;
}> = ({ content }) => {
  return (
    <Alert
      style={{
        marginBottom: 24,
      }}
      message={content}
      type="error"
      showIcon
    />
  );
};

type QueryProps = {
  clientId?: string;
  redirectUrl?: string;
  t_code?: string;
};

const Login: React.FC = () => {
  const [userLoginState, setUserLoginState] = useState<API.LoginResult>({});
  const [type, setType] = useState<string>('account');
  const { initialState, setInitialState } = useModel('@@initialState');
  const query: QueryProps = parse(history.location.search.split('?')[1], '&');
  const { clientId, redirectUrl, t_code } = query;
  const [tenantCode, setTenantCode] = useLocalStorageState<string | undefined>(APP_TENANT_CODE_KEY);
  useEffect(() => {
    if (t_code && t_code !== tenantCode) {
      setTenantCode(t_code);
    }
  }, [t_code]);

  const containerClassName = useEmotionCss(() => {
    return {
      display: 'flex',
      flexDirection: 'column',
      height: '100vh',
      overflow: 'auto',
      backgroundImage:
        "url('https://mdn.alipayobjects.com/yuyan_qk0oxh/afts/img/V-_oS6r-i7wAAAAAAAAAAAAAFl94AQBr')",
      backgroundSize: '100% 100%',
    };
  });

  const intl = useIntl();

  const fetchUserInfo = async () => {
    const userInfo = await initialState?.fetchUserInfo?.();
    if (userInfo) {
      flushSync(() => {
        setInitialState((s) => ({
          ...s,
          currentUser: userInfo,
        }));
      });
    }
  };
  const fetchApiResources = async () => {
    const resources = await initialState?.fetchApiResources?.();
    if (resources) {
      flushSync(() => {
        setInitialState((s) => ({
          ...s,
          apiResources: resources,
        }));
      });
    }
  };

  const handleSubmit = async (values: API.LoginParams) => {
    try {
      // 登录
      if (clientId) {
        values.clientId = clientId as string;
      }
      const res = await login({ ...values, type, tenantId: tenantCode });
      if (res.success) {
        const defaultLoginSuccessMessage = intl.formatMessage({
          id: 'pages.login.success',
          defaultMessage: '登录成功！',
        });
        message.success(defaultLoginSuccessMessage);
        setToken(res.data!.currentAuthority!);
        setSessionCode(res.data!.sessionCode!);
        await fetchUserInfo();
        await fetchApiResources();
        const urlParams = parse(window.location.href.split('?')[1], '&');
        if (redirectUrl !== undefined && clientId !== undefined) {
          const code = res.data?.sessionCode;
          // 读取子应用授权Token
          const tokenRes = await getAuthToken({
            clientId: clientId as string,
            sessionCode: code as string,
          });
          if (!tokenRes.success) {
            message.error('跳转失败，请重试。');
            return;
          }
          const url = redirectUrl as string;
          if (url !== undefined && url?.includes('?')) {
            let host = url.split('?')[0];
            const urlParams = parse(url.split('login-redirect?')[1], '&');
            const redirect = urlParams?.redirect;
            const param =
              redirect === undefined ? '' : `&redirect=${encodeURIComponent(redirect as string)}`;
            window.location.href = `${host}?token=${tokenRes.data}&source=sso${param}`;
            return;
          }
          window.location.href = `${redirectUrl}?token=${tokenRes.data}&source=sso`;
        } else {
          history.push((urlParams?.redirect as string) || '/');
        }
        return;
      }
      // 如果失败去设置用户错误信息
      setUserLoginState({
        status: 'error',
        type,
        errorMessage: res.message,
      });
    } catch (error) {
      const defaultLoginFailureMessage = intl.formatMessage({
        id: 'pages.login.failure',
        defaultMessage: '登录失败，请重试！',
      });
      message.error(defaultLoginFailureMessage);
    }
  };
  const { status, type: loginType, errorMessage } = userLoginState;

  const appSettings: AppSettings = {
    logo: 'https://cdn.gzwjz.com/FmzrX15jYA03KMVfbgMJnk-P6WGl.png',
    title: 'Athena Pro',
    subTitle: '.NET Core下更好用且功能强大的通用基础权限管理平台'
  };

  return (
    <div className={containerClassName}>
      <Helmet>
        <title>
          {intl.formatMessage({
            id: 'menu.login',
            defaultMessage: '登录页',
          })}
          - {Settings.title}
        </title>
      </Helmet>
      <Lang />
      <div
        style={{
          flex: '1',
          padding: '32px 0',
        }}
      >
        <LoginForm
          contentStyle={{
            minWidth: 280,
            maxWidth: '75vw',
          }}
          logo={<img alt="logo" src={appSettings.logo || "https://cdn.gzwjz.com/FmzrX15jYA03KMVfbgMJnk-P6WGl.png"} width={44} height={44} />}
          title={<>
            <span>{appSettings.title || 'Athena Pro'}</span>
            {tenantCode && <Tag
              color="purple"
              closable
              onClose={(e) => {
                e.stopPropagation();
                setTenantCode(undefined);
              }}>{tenantCode}</Tag>}
          </>}
          // subTitle={intl.formatMessage({ id: 'pages.layouts.userLayout.title' })}
          subTitle={appSettings.subTitle || 'Athena Pro'}
          initialValues={{
            rememberMe: true,
          }}
          actions={[
            <FormattedMessage
              key="loginWith"
              id="pages.login.loginWith"
              defaultMessage="其他登录方式"
            />,
            <ActionIcons key="icons" />,
          ]}
          onFinish={async (values) => {
            await handleSubmit(values as API.LoginParams);
          }}
        >
          <Tabs
            activeKey={type}
            onChange={setType}
            centered
            items={[
              {
                key: 'account',
                label: intl.formatMessage({
                  id: 'pages.login.accountLogin.tab',
                  defaultMessage: '账户密码登录',
                }),
              },
              {
                key: 'mobile',
                label: intl.formatMessage({
                  id: 'pages.login.phoneLogin.tab',
                  defaultMessage: '手机号登录',
                }),
              },
            ]}
          />

          {status === 'error' && loginType === 'account' && (
            <LoginMessage
              content={
                errorMessage ||
                intl.formatMessage({
                  id: 'pages.login.accountLogin.errorMessage',
                  defaultMessage: '账户或密码错误(admin/ant.design)',
                })
              }
            />
          )}
          {type === 'account' && (
            <>
              <ProFormText
                name="username"
                fieldProps={{
                  size: 'large',
                  prefix: <UserOutlined />,
                }}
                placeholder={'用户名: root or admin'}
                rules={[
                  {
                    required: true,
                    message: (
                      <FormattedMessage
                        id="pages.login.username.required"
                        defaultMessage="请输入用户名!"
                      />
                    ),
                  },
                ]}
              />
              <ProFormText.Password
                name="password"
                fieldProps={{
                  size: 'large',
                  prefix: <LockOutlined />,
                }}
                placeholder={intl.formatMessage({
                  id: 'pages.login.password.placeholder',
                  defaultMessage: '密码: ant.design',
                })}
                rules={[
                  {
                    required: true,
                    message: (
                      <FormattedMessage
                        id="pages.login.password.required"
                        defaultMessage="请输入密码！"
                      />
                    ),
                  },
                ]}
              />
            </>
          )}

          {status === 'error' && loginType === 'mobile' && <LoginMessage content="验证码错误" />}
          {type === 'mobile' && (
            <>
              <ProFormText
                fieldProps={{
                  size: 'large',
                  prefix: <MobileOutlined />,
                }}
                name="mobile"
                placeholder={intl.formatMessage({
                  id: 'pages.login.phoneNumber.placeholder',
                  defaultMessage: '手机号',
                })}
                rules={[
                  {
                    required: true,
                    message: (
                      <FormattedMessage
                        id="pages.login.phoneNumber.required"
                        defaultMessage="请输入手机号！"
                      />
                    ),
                  },
                  {
                    pattern: /^1\d{10}$/,
                    message: (
                      <FormattedMessage
                        id="pages.login.phoneNumber.invalid"
                        defaultMessage="手机号格式错误！"
                      />
                    ),
                  },
                ]}
              />
              <ProFormCaptcha
                fieldProps={{
                  size: 'large',
                  prefix: <LockOutlined />,
                }}
                captchaProps={{
                  size: 'large',
                }}
                placeholder={intl.formatMessage({
                  id: 'pages.login.captcha.placeholder',
                  defaultMessage: '请输入验证码',
                })}
                captchaTextRender={(timing, count) => {
                  if (timing) {
                    return `${count} ${intl.formatMessage({
                      id: 'pages.getCaptchaSecondText',
                      defaultMessage: '获取验证码',
                    })}`;
                  }
                  return intl.formatMessage({
                    id: 'pages.login.phoneLogin.getVerificationCode',
                    defaultMessage: '获取验证码',
                  });
                }}
                name="captcha"
                rules={[
                  {
                    required: true,
                    message: (
                      <FormattedMessage
                        id="pages.login.captcha.required"
                        defaultMessage="请输入验证码！"
                      />
                    ),
                  },
                ]}
                onGetCaptcha={async (phone) => {
                  const result = await getFakeCaptcha({
                    phone,
                  });
                  if (!result) {
                    return;
                  }
                  message.success('获取验证码成功！验证码为：1234');
                }}
              />
            </>
          )}
          <div
            style={{
              marginBottom: 24,
            }}
          >
            <ProFormCheckbox noStyle name="rememberMe">
              <FormattedMessage id="pages.login.rememberMe" defaultMessage="自动登录" />
            </ProFormCheckbox>
            <a
              style={{
                float: 'right',
              }}
            >
              <FormattedMessage id="pages.login.forgotPassword" defaultMessage="忘记密码" />
            </a>
          </div>
        </LoginForm>
      </div>
      <Footer />
    </div>
  );
};

export default Login;
