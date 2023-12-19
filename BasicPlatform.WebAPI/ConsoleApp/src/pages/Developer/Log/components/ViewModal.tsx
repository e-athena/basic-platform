import React from 'react';
import { Modal, Space, Tag, Tooltip } from 'antd';
import { ProDescriptions } from '@ant-design/pro-components';
import { querySelectList } from '@/services/ant-design-pro/system/user';
import { ExportOutlined } from '@ant-design/icons';
import { exportTxtFile } from '@/utils/utils';

type ViewDrawerProps = {
  onClose: () => void;
  open: boolean;
  values: API.LogDetail;
};

// 日志等级
const logLevelArray = ['Trace', 'Debug', 'Information', 'Warning', 'Error', 'Critical', 'None'];

const ViewModal: React.FC<ViewDrawerProps> = (props) => {
  const [userList, setUserList] = React.useState<API.SelectInfo[]>([]);
  React.useEffect(() => {
    if (!props.open) {
      return;
    }
    if (userList.length > 0) {
      return;
    }
    querySelectList().then((res) => {
      setUserList(res.data || []);
    });
  }, []);
  return (
    <>
      <Modal
        width={'50%'}
        open={props.open}
        onCancel={() => {
          props.onClose();
        }}
      >
        <ProDescriptions<API.LogDetail>
          column={1}
          labelStyle={{ width: '120px' }}
          title={<Space>
            <span>日志详情</span>
            <Tooltip title={'导出日志'}>
              <ExportOutlined onClick={() => {
                exportTxtFile(`${props.values.traceId}-日志`, JSON.stringify(props.values));
              }} />
            </Tooltip>
          </Space>}
          size={'small'}
          bordered
          dataSource={props.values || {}}
        >
          <ProDescriptions.Item
            dataIndex={'id'}
            label={'ID'}
            valueType={'text'}
          />
          <ProDescriptions.Item
            dataIndex={'serviceName'}
            label={'服务名'}
            valueType={'text'}
          />
          <ProDescriptions.Item
            dataIndex={'aliasName'}
            label={'别名'}
            valueType={'text'}
          />
          <ProDescriptions.Item
            dataIndex={'traceId'}
            label={'追踪ID'}
            copyable
            valueType={'text'}
          />
          <ProDescriptions.Item
            dataIndex={'startTime'}
            label={'请求开始时间'}
            valueType={'dateTime'}
          />
          <ProDescriptions.Item
            dataIndex={'endTime'}
            label={'请求结束时间'}
            valueType={'dateTime'}
          />
          <ProDescriptions.Item
            dataIndex={'elapsedMilliseconds'}
            label={'耗时/毫秒'}
            valueType={'text'}
            renderText={(text) => {
              return `${text}ms`;
            }}
          />
          <ProDescriptions.Item
            dataIndex={'route'}
            label={'路由'}
            valueType={'text'}
          />
          <ProDescriptions.Item
            dataIndex={'httpMethod'}
            label={'请求方法'}
            render={(dom) => {
              let color = 'default';
              switch (dom) {
                case 'GET':
                  color = 'cyan';
                  break;
                case 'POST':
                  color = 'blue';
                  break;
                case 'PUT':
                  color = 'green';
                  break;
                case 'DELETE':
                  color = 'orange';
                  break;
                case 'HEAD':
                  color = 'red';
                  break;
                case 'OPTIONS':
                  color = 'magenta';
                  break;
                case 'TRACE':
                  color = 'default';
                  break;
                case 'PATCH':
                  color = 'default';
                  break;
              }

              return <Tag color={color}>{dom}</Tag>;
            }}
          />
          <ProDescriptions.Item
            dataIndex={'statusCode'}
            label={'状态码'}
            valueType={'text'}
          />
          <ProDescriptions.Item
            dataIndex={'logLevel'}
            label={'日志等级'}
            render={(_, entity) => {
              let color = 'default';
              switch (entity.logLevel) {
                case 0:
                  color = 'cyan';
                  break;
                case 1:
                  color = 'blue';
                  break;
                case 2:
                  color = 'green';
                  break;
                case 3:
                  color = 'orange';
                  break;
                case 4:
                  color = 'red';
                  break;
                case 5:
                  color = 'magenta';
                  break;
                case 6:
                  color = 'default';
                  break;
              }
              return <Tag color={color}>{logLevelArray[entity.logLevel]}</Tag>;
            }}
          />
          <ProDescriptions.Item
            dataIndex={'device'}
            label={'设备'}
            valueType={'text'}
          />
          <ProDescriptions.Item
            dataIndex={'browser'}
            label={'浏览器'}
            valueType={'text'}
          />
          <ProDescriptions.Item
            dataIndex={'os'}
            label={'操作系统'}
            valueType={'text'}
          />
          <ProDescriptions.Item
            dataIndex={'ipAddress'}
            label={'IP地址'}
            valueType={'text'}
          />
          <ProDescriptions.Item
            dataIndex={'userAgent'}
            label={'用户代理'}
            valueType={'text'}
          />
          <ProDescriptions.Item
            dataIndex={'userId'}
            label={'操作人ID'}
            copyable
            valueType={'text'}
          />
          <ProDescriptions.Item
            dataIndex={'userId'}
            label={'操作人'}
            valueType={'text'}
            renderText={(text) => {
              return userList.find((item) => item.value === text)?.label;
            }}
          />
          <ProDescriptions.Item
            dataIndex={'createdOn'}
            label={'存储时间'}
            valueType={'dateTime'}
          />
          <ProDescriptions.Item
            dataIndex={'requestBody'}
            label={'请求内容'}
          // valueType={'jsonCode'}
          />
          <ProDescriptions.Item
            dataIndex={'responseBody'}
            label={'响应内容'}
          // valueType={'jsonCode'}
          />
          <ProDescriptions.Item
            dataIndex={'rawData'}
            label={'原始数据'}
          />
          <ProDescriptions.Item
            dataIndex={'errorMessage'}
            label={'错误信息'}
            valueType={'text'}
          />
        </ProDescriptions>
      </Modal>
    </>
  );
};

export default ViewModal;