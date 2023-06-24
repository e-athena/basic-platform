import { submitHandle } from '@/utils/utils';
import { ProFormText, ProFormTextArea, ModalForm, ProForm, ProFormDatePicker, ProCard, ProTable } from '@ant-design/pro-components';
import { Checkbox, DatePicker, FormInstance, Input, message } from 'antd';
import React, { useEffect, useRef, useState } from 'react';
import { update, create } from '../service';
import dayjs from 'dayjs';
import pattern from '@/utils/pattern';
import { queryAppSelectList } from '@/services/ant-design-pro/api';

type CreateOrUpdateFormProps = {
  onCancel: () => void;
  onSuccess: () => void;
  open: boolean;
  values?: API.TenantDetailItem;
};

const CreateOrUpdateForm: React.FC<CreateOrUpdateFormProps> = (props) => {
  const formRef = useRef<FormInstance>();
  const [tab, setTab] = useState('basic');
  const [dataSource, setDataSource] = React.useState<API.TenantApplicationItem[]>([]);
  useEffect(() => {
    const fetch = async () => {
      const result = await queryAppSelectList();
      if (result.success) {
        setDataSource((result.data || []).filter(x => !x.disabled).map((item) => ({
          tenantId: null,
          isEnabled: false,
          applicationId: item.value,
          applicationName: item.label,
          connectionString: null,
          expiredTime: null,
        } as unknown as API.TenantApplicationItem)));
      }
    }
    if (props.open) {
      if (props.values) {
        setDataSource(props.values?.applications || []);
      } else {
        fetch();
      }
    }
  }, [props.open]);
  return (
    <ModalForm
      width={786}
      formRef={formRef}
      title={props.values?.id === undefined ? '创建租户' : '更新租户'}
      open={props.open}
      modalProps={{
        onCancel: () => {
          props.onCancel();
          setTab('basic');
        },
        // bodyStyle: { padding: '32px 40px 48px' },
        bodyStyle: { paddingTop: 10, paddingBottom: 10 },
        destroyOnClose: true,
      }}
      onFinish={async (values: API.UpdateTenantItem) => {
        const isUpdate = props.values?.id !== undefined;
        if (isUpdate) {
          values.id = props.values!.id!;
        }
        // 检查子应用配置，如果启用了，必须填写连接字符串，否则则给出相应的提示
        const enabledApplications = dataSource.filter((item) => item.isEnabled);
        if (enabledApplications.length > 0) {
          const invalidApplication = enabledApplications.find((item) => item.connectionString === undefined || item.connectionString === null || item.connectionString === '');
          if (invalidApplication !== undefined) {
            message.error(`子应用【${invalidApplication.applicationName}】启用了，必须填写连接字符串`);
            return false;
          }
        }
        values.applications = dataSource;
        let succeed;
        if (isUpdate) {
          values.id = props.values!.id!;
          succeed = await submitHandle(update, values);
        } else {
          succeed = await submitHandle(create, values as API.CreateTenantItem);
        }
        if (succeed) {
          props.onSuccess();
          setTab('basic');
        }
      }}
      initialValues={{
        ...props?.values
      }}
    >
      <ProCard
        bordered
        tabs={{
          tabPosition: 'top',
          activeKey: tab,
          items: [
            {
              label: `基础信息`,
              key: 'basic',
              children: <>
                <ProForm.Group>
                  <ProFormText
                    name="name"
                    label={'名称'}
                    width="md"
                    rules={[
                      {
                        required: true,
                        message: '请输入租户名称',
                      },
                    ]}
                  />
                  <ProFormText
                    name="code"
                    label={'编码'}
                    width="md"
                    tooltip={'唯一标识'}
                    placeholder={'请输入'}
                    rules={[
                      {
                        required: true,
                        message: '请输入租户编码',
                      },
                    ]}
                  />
                </ProForm.Group>
                <ProFormText
                  name="connectionString"
                  label={'数据库连接字符串'}
                  tooltip={'用户租户数据隔离'}
                  placeholder={'例：sqlite,Data Source=test_local.db;'}
                  rules={[
                    {
                      required: true,
                      message: '请输入数据库连接字符串',
                    },
                  ]}
                />
                <ProForm.Group>
                  <ProFormText
                    name="contactName"
                    label={'联系人姓名'}
                    width="md"
                    rules={[
                      {
                        required: true,
                        message: '请输入联系人姓名',
                      },
                    ]}
                  />
                  <ProFormText
                    name="contactPhoneNumber"
                    label={'联系人手机号'}
                    width="md"
                    rules={[
                      {
                        required: true,
                        message: '请输入联系人手机号',
                      },
                      {
                        pattern: pattern.mobile,
                        message: '手机号格式不正确',
                      },
                    ]}
                  />
                </ProForm.Group>
                <ProFormText
                  name="contactEmail"
                  label={'联系人邮箱'}
                  rules={[
                    {
                      pattern: pattern.email,
                      message: '邮箱格式不正确',
                    },
                  ]}
                />
                <ProForm.Group>
                  <ProFormDatePicker
                    name="effectiveTime"
                    label={'订阅生效日期'}
                    width="md"
                    placeholder={'为空时立即生效'}
                  />
                  <ProFormDatePicker
                    name="expiredTime"
                    label={'订阅过期日期'}
                    width="md"
                    placeholder={'为空时表示永不过期'}
                  />
                </ProForm.Group>
                <ProFormTextArea
                  name="remarks"
                  label={'描述'}
                  placeholder={'请输入'}
                />
              </>,
            },
            {
              label: `子应用配置`,
              key: 'application',
              children: <>
                <ProTable
                  size={'middle'}
                  search={false}
                  options={false}
                  cardProps={{
                    bodyStyle: { margin: 0, padding: 0 }
                  }}
                  dataSource={dataSource}
                  key={'applicationId'}
                  columns={[
                    {
                      title: '授权',
                      dataIndex: 'isEnabled',
                      width: 50,
                      align: 'center',
                      render(_, entity) {
                        return (
                          <Checkbox
                            checked={entity.isEnabled}
                            onChange={(e) => {
                              entity.isEnabled = e.target.checked;
                              setDataSource([...dataSource]);
                            }}
                          />
                        );
                      },
                    },
                    {
                      title: '应用名称',
                      dataIndex: 'applicationName',
                      width: 100,
                      ellipsis: true,
                    },
                    {
                      title: '连接字符串',
                      dataIndex: 'connectionString',
                      tooltip: '数据库连接字符串',
                      ellipsis: true,
                      render: (_, entity) => {
                        const connectionString = entity.connectionString || undefined;
                        return <Input
                          value={connectionString}
                          placeholder={'例：sqlite,Data Source=test_local.db;'}
                          onChange={(e) => {
                            entity.connectionString = e.target.value;
                            setDataSource([...dataSource]);
                          }}
                        />
                      },
                    },
                    {
                      title: '失效日期',
                      dataIndex: 'expiredTime',
                      tooltip: '为空时表示永不过期',
                      width: 140,
                      valueType: 'date',
                      render(_, entity) {
                        if (entity.expiredTime !== undefined && entity.expiredTime !== null && entity.expiredTime !== '') {
                          return <DatePicker
                            value={dayjs(entity.expiredTime)}
                            onChange={(e) => {
                              entity.expiredTime = e === null ? null : e?.format('YYYY-MM-DD');
                              setDataSource([...dataSource]);
                            }}
                            placeholder={'请选择'}
                          />
                        }
                        // 日期控件
                        return <DatePicker
                          value={null}
                          onChange={(e) => {
                            console.log(e);
                          }}
                          placeholder={'请选择'}
                        />
                      },
                    },
                  ]}
                  pagination={false}
                />
              </>,
            },
          ],
          onChange: (key) => {
            setTab(key);
          },
        }}
      />
    </ModalForm>
  );
};

export default CreateOrUpdateForm;
