import { detail, getServiceSelectOptions, query } from './service';
import { FilterOutlined, SearchOutlined } from '@ant-design/icons';
import type { ActionType, FormInstance, ProColumns } from '@ant-design/pro-components';
import { LightFilter, PageContainer, ProFormDateRangePicker, ProFormSelect } from '@ant-design/pro-components';
import { useModel, useLocation, Access } from '@umijs/max';
import { Button, Tag, notification } from 'antd';
import React, { useRef, useState } from 'react';
import permission from '@/utils/permission';
import { canAccessible, hasPermission } from '@/utils/utils';
import ViewModal from './components/ViewModal';
import ProTablePlus from '@/components/ProTablePlus';
import dayjs from 'dayjs';
import SearchModal from './components/SearchModal';


// 日志等级
const logLevelArray = ['Trace', 'Debug', 'Information', 'Warning', 'Error', 'Critical', 'None'];

const filterMaps: Record<string, string> = {
  serviceName: '服务名',
  dateRange: '记录日期',
  logLevel: '日志等级',
  traceId: '追踪ID',
  realName: '操作人',
}

const TableList: React.FC = () => {
  const [viewModalOpen, handleViewModalOpen] = useState<boolean>(false);
  const [searchModalOpen, handleSearchModalOpen] = useState<boolean>(false);

  const actionRef = useRef<ActionType>();
  const searchFormRef = useRef<FormInstance>();
  const [currentRow, setCurrentRow] = useState<API.LogDetail>();
  const { getResource } = useModel('resource');
  const location = useLocation();
  const resource = getResource(location.pathname);
  const showOption: boolean = hasPermission([permission.log.getByIdAsync], resource);


  /**需要重写的Column */
  const defaultColumns: ProColumns<API.LogListItem>[] = ([
    {
      title: '日志等级',
      dataIndex: 'logLevel',
      width: 100,
      render(_, entity) {
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
      },
    },
    {
      title: 'HTTP方法',
      dataIndex: 'httpMethod',
      width: 100,
      render(dom) {
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
      },
    },
    {
      title: '路由',
      dataIndex: 'route',
      // width: 180,
      ellipsis: true,
    },
    {
      title: '耗时/毫秒',
      dataIndex: 'elapsedMilliseconds',
      ellipsis: true,
      sorter: true,
      width: 95,
      renderText: (val: number) => `${val}ms`,
    },
    {
      title: '追踪ID',
      dataIndex: 'traceId',
      ellipsis: true,
      hideInTable: true,
    },
    // 操作人
    {
      title: '操作人',
      dataIndex: 'userId',
      ellipsis: true,
      width: 120,
    },
    {
      title: '存储时间',
      dataIndex: 'createdOn',
      ellipsis: true,
      width: 175,
      sorter: true,
      valueType: 'dateTime',
    },
    {
      title: '操作',
      dataIndex: 'option',
      valueType: 'option',
      hideInTable: !showOption,
      width: 95,
      render(_, entity) {
        return [
          <Access
            key={'edit'}
            accessible={canAccessible(permission.log.getByIdAsync, resource)}
          >
            <Button
              size="small"
              icon={<SearchOutlined />}
              onClick={async (e) => {
                e.stopPropagation();
                const rsp = await detail(entity.id, entity.serviceName);
                if (rsp.success) {
                  setCurrentRow(rsp.data);
                  handleViewModalOpen(true);
                }
              }}
            >
              查看
            </Button>
          </Access>,
        ];
      },
    },
  ]);

  const [searchFilter, setSearchFilter] = useState<API.LogPagingParams>({
    serviceName: undefined,
    dateRange: [dayjs().startOf('month'), dayjs().endOf('month')],
  });
  const [serviceList, setServiceList] = useState<API.SelectInfo[]>([]);

  return (
    <PageContainer
      header={{
        title: resource?.name,
        children: resource?.description,
      }}
    >
      <ProTablePlus<API.LogListItem, API.LogPagingParams>
        actionRef={actionRef}
        defaultColumns={defaultColumns}
        query={query}
        toolbar={{
          search: false,
          filter: <>
            <LightFilter
              formRef={searchFormRef}
              bordered
              // 默认当月
              initialValues={{
                ...searchFilter
              }}
              collapseLabel={<FilterOutlined />}
              onFinish={async (values) => {
                setSearchFilter({ ...searchFilter, ...values });
                return true;
              }}
            >
              <ProFormSelect
                name="serviceName"
                showSearch
                request={async () => {
                  if (serviceList.length > 0) {
                    return serviceList;
                  }
                  const { data } = await getServiceSelectOptions();
                  setServiceList(data || []);
                  return data || [];
                }}
                placeholder="服务名"
              />
              <ProFormDateRangePicker
                name="dateRange"
                label="日期"
                placeholder={['开始日期', '结束日期']}
              />
              <Button onClick={() => { handleSearchModalOpen(true) }}>
                更多条件
              </Button>
            </LightFilter>
          </>
        }}
        requestBeforeTransform={async (params) => {
          if (!params.serviceName) {
            return Promise.reject('请选择服务名');
          }
          return Promise.resolve(params);
        }}
        params={searchFilter}
        searchPlaceholder='请选择服务名'
        moduleName={'Log'}
        onlyUseLocalColumns
        hideAdvancedSearch
        hideCloumnSetting
      />
      {currentRow && (
        <ViewModal
          onClose={() => {
            handleViewModalOpen(false);
            setCurrentRow(undefined);
          }}
          open={viewModalOpen}
          values={currentRow}
        />
      )}

      {searchModalOpen && (
        <SearchModal
          onCancel={() => {
            handleSearchModalOpen(false);
          }}
          open={searchModalOpen}
          values={searchFilter}
          serviceList={serviceList}
          onSearch={(values) => {
            searchFormRef.current?.setFieldsValue({ ...values });
            setSearchFilter({ ...searchFilter, ...values });
            handleSearchModalOpen(false);
            let descriptions: string[] = [];
            console.log(values);
            Object.keys(values).forEach((key) => {
              if (values[key]) {
                if (key === 'logLevel') {
                  descriptions.push(`${filterMaps[key]}：${logLevelArray[values[key]!]}`);
                  return;
                }
                descriptions.push(`${filterMaps[key]}：${values[key]}`);
              }
            });
            // 先关闭其他的
            notification.destroy();
            if (descriptions.length > 0) {
              notification.info({
                message: '当前查询条件',
                description: <>
                  {descriptions.map((item, index) => {
                    return <div key={index}>{index + 1}、{item}</div>;
                  })}
                </>,
                style: {
                  width: 445
                },
                duration: 0,
                onClose: () => {
                  // 重置查询条件
                  searchFormRef.current?.setFieldsValue({
                    serviceName: searchFilter.serviceName,
                    dateRange: [dayjs().startOf('month'), dayjs().endOf('month')],
                    logLevel: undefined,
                    traceId: undefined,
                    realName: undefined,
                  });
                  setSearchFilter({
                    serviceName: searchFilter.serviceName,
                    dateRange: [dayjs().startOf('month'), dayjs().endOf('month')],
                  });
                }
              });
            }
          }}
        />
      )}
    </PageContainer>
  );
};

export default TableList;
