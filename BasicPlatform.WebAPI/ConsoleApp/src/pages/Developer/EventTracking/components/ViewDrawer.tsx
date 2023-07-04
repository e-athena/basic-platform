import React from 'react';
import { DecompositionTreeGraph, DecompositionTreeGraphConfig, G6TreeGraphData } from '@ant-design/graphs';
import { Button, Space, Drawer, Modal } from 'antd';
import { SearchOutlined } from '@ant-design/icons';
import { ProDescriptions } from '@ant-design/pro-components';
import { queryDetail } from '@/utils/utils';
import { detail } from '../service';

type ViewDrawerProps = {
  onClose: () => void;
  open: boolean;
  dataSource: G6TreeGraphData;
};

const ViewDrawer: React.FC<ViewDrawerProps> = (props) => {
  const [detailOpenModal, setDetailOpenModal] = React.useState<boolean>(false);
  const [currentRow, setCurrentRow] = React.useState<any>();
  const config: DecompositionTreeGraphConfig = {
    data: props.dataSource,
    autoFit: false,
    nodeCfg: {
      autoWidth: true,
      items: {
        layout: 'follow',
      },
    },
    height: window.innerHeight - 100,
    layout: {
      getWidth: () => {
        return 60;
      },
      getHeight: () => {
        return 120
      },
    },
    edgeCfg: {
      // type: 'cubic-horizontal',
      type: 'polyline'
    },
    menuCfg: {
      customContent(evt) {
        return <Space>
          <Button
            title={'查看详情'}
            size={'small'}
            onClick={async () => {
              const id = evt?.item?._cfg?.id;
              if (id === undefined) {
                return;
              }
              const res = await queryDetail(detail, id);
              setCurrentRow(res);
              setDetailOpenModal(true);
            }}
            icon={<SearchOutlined />}
          />
        </Space>;
      },
    },
    behaviors: [
      'drag-canvas',
      // 'zoom-canvas',
      'drag-node'
    ],
  };
  return (
    <>
      <Drawer
        width={'75%'}
        title={`追踪详情`}
        placement="right"
        onClose={props.onClose}
        open={props.open}
      >
        <DecompositionTreeGraph {...config} />
      </Drawer>
      <Modal
        width={'50%'}
        open={detailOpenModal}
        onCancel={() => {
          setDetailOpenModal(false);
          setCurrentRow(undefined);
        }}
        title={'详情'}
      >
        <ProDescriptions
          column={1}
          labelStyle={{ width: '120px' }}
          title={false}
          size={'small'}
          bordered
          dataSource={currentRow || {}}
        >
          <ProDescriptions.Item
            dataIndex={'traceId'}
            label={'追踪ID'}
            valueType={'text'}
          />
          <ProDescriptions.Item
            dataIndex={'eventType'}
            label={'事件类型'}
            valueType={'text'}
            renderText={(text) => {
              return text === 1 ? '领域事件' : '集成事件';
            }}
          />
          <ProDescriptions.Item
            dataIndex={'eventName'}
            label={'事件名称'}
            valueType={'text'}
          />
          <ProDescriptions.Item
            dataIndex={'eventTypeFullName'}
            label={'事件实体名称'}
            valueType={'text'}
          />
          <ProDescriptions.Item
            dataIndex={'trackStatus'}
            label={'执行状态'}
            valueType={'text'}
            renderText={(text) => {
              switch (text) {
                case 0:
                  return '未执行';
                case 1:
                  return '执行中';
                case 2:
                  return '执行成功';
                case 3:
                  return '执行失败';
                default:
                  return '未知';
              }
            }}
          />
          <ProDescriptions.Item
            dataIndex={'beginExecuteTime'}
            label={'开始执行时间'}
            valueType={'dateTime'}
          />
          <ProDescriptions.Item
            dataIndex={'endExecuteTime'}
            label={'执行完成时间'}
            valueType={'dateTime'}
          />
          <ProDescriptions.Item
            dataIndex={'payload'}
            label={'执行参数'}
            valueType={'text'}
          />
          <ProDescriptions.Item
            dataIndex={'processorFullName'}
            label={'处理器'}
            valueType={'text'}
          />
          {currentRow?.trackStatus === 3 && (
            <>
              <ProDescriptions.Item
                dataIndex={'exceptionMessage'}
                label={'错误信息'}
                valueType={'text'}
              />
              <ProDescriptions.Item
                dataIndex={'exceptionInnerMessage'}
                label={'错误内联信息'}
                valueType={'text'}
              />
              <ProDescriptions.Item
                dataIndex={'exceptionInnerType'}
                label={'错误类型'}
                valueType={'text'}
              />
              <ProDescriptions.Item
                dataIndex={'exceptionStackTrace'}
                label={'错误堆栈信息'}
                valueType={'text'}
              />
            </>
          )}
        </ProDescriptions>
      </Modal>
    </>
  );
};

export default ViewDrawer;