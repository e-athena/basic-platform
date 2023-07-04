import React from 'react';
import { DecompositionTreeGraph, DecompositionTreeGraphConfig, G6TreeGraphData } from '@ant-design/graphs';
import { Button, Space, Tooltip, Drawer } from 'antd';
import { SearchOutlined } from '@ant-design/icons';
import { convert } from '../utils';

type ViewDrawerProps = {
  onClose: () => void;
  open: boolean;
  values: API.EventTrackingConfigDetailItem;
};

const ViewDrawer: React.FC<ViewDrawerProps> = (props) => {
  // const parent: G6TreeGraphData = {
  //   id: props.dataSource.id!,
  //   value: null
  // };
  console.log(props.values);
  const data: G6TreeGraphData = convert(props.values, undefined);
  const config: DecompositionTreeGraphConfig = {
    data: data,
    autoFit: false,
    nodeCfg: {
      autoWidth: true,
      items: {
        layout: 'follow',
      },
    },
    height:window.innerHeight-100,
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
          <Tooltip title={'查看详情'}>
            <Button
              size={'small'}
              onClick={() => {
                if (evt?.item?._cfg?.id === undefined) {
                  return;
                }
                console.log(evt?.item?._cfg?.id);
              }}
              icon={<SearchOutlined />}
            />
          </Tooltip>
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
    </>
  );
};

export default ViewDrawer;