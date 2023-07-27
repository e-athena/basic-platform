import React from 'react';
import { DecompositionTreeGraph, DecompositionTreeGraphConfig, G6TreeGraphData } from '@ant-design/graphs';
import { Button, Space, Tooltip } from 'antd';
import { FormOutlined, SearchOutlined, SubnodeOutlined } from '@ant-design/icons';


// 从data中根据id读取数据，data是一个树形结构
const findNode = (items: G6TreeGraphData[], id: string): G6TreeGraphData | undefined => {
  for (let i = 0; i < items.length; i++) {
    const item = items[i];
    if (item.id === id) {
      return item;
    }
    if (item.children) {
      const findChildren = findNode(item.children, id);
      if (findChildren) {
        return findChildren;
      }
    }
  }
  return undefined;
};

const DemoDecompositionTreeGraph = () => {
  const [dataSource, setDataSource] = React.useState<G6TreeGraphData>({
    "id": "64a01a6846a0d9319da623dc",
    "value": {
      "title": "创建用户",
      "items": [
        {
          "text": "执行总耗时",
          "value": "1.618595s",
          "icon": null
        },
        {
          "text": "开始执行时间",
          "value": "2023-07-01 20:22:00",
          "icon": null
        },
        {
          "text": "执行完成时间",
          "value": "2023-07-01 20:22:02",
          "icon": null
        }
      ]
    },
    "children": [
      {
        "id": "64a01a6846a0d9319da623dd",
        "value": {
          "title": "用户通知",
          "items": [
            {
              "text": "执行状态",
              "value": "执行成功",
              "icon": null
            },
            {
              "text": "执行耗时",
              "value": "2.41ms",
              "icon": null
            },
            {
              "text": "开始执行时间",
              "value": "2023-07-01 20:22:00",
              "icon": null
            },
            {
              "text": "执行完成时间",
              "value": "2023-07-01 20:22:00",
              "icon": null
            }
          ]
        },
      },
      {
        "id": "64a01a6846a0d9319da623de",
        "value": {
          "title": "测试集成事件",
          "items": [
            {
              "text": "执行状态",
              "value": "执行成功",
              "icon": null
            },
            {
              "text": "执行耗时",
              "value": "17.421ms",
              "icon": null
            },
            {
              "text": "开始执行时间",
              "value": "2023-07-01 20:22:01",
              "icon": null
            },
            {
              "text": "执行完成时间",
              "value": "2023-07-01 20:22:01",
              "icon": null
            }
          ]
        },
      },
      {
        "id": "64a01a6846a0d9319da623df",
        "value": {
          "title": "租户设置为已初始化",
          "items": [
            {
              "text": "执行状态",
              "value": "执行成功",
              "icon": null
            },
            {
              "text": "执行耗时",
              "value": "88.859ms",
              "icon": null
            },
            {
              "text": "开始执行时间",
              "value": "2023-07-01 20:22:01",
              "icon": null
            },
            {
              "text": "执行完成时间",
              "value": "2023-07-01 20:22:01",
              "icon": null
            }
          ]
        },
        "children": [
          {
            "id": "64a01a6846a0d9319da623e0",
            "value": {
              "title": "测试租户已初始化事件",
              "items": [
                {
                  "text": "执行状态",
                  "value": "执行成功",
                  "icon": null
                },
                {
                  "text": "执行耗时",
                  "value": "0.046ms",
                  "icon": null
                },
                {
                  "text": "开始执行时间",
                  "value": "2023-07-01 20:22:02",
                  "icon": null
                },
                {
                  "text": "执行完成时间",
                  "value": "2023-07-01 20:22:02",
                  "icon": null
                }
              ]
            },
          }
        ]
      }
    ]
  });
  const config: DecompositionTreeGraphConfig = {
    data: dataSource,
    autoFit: false,
    nodeCfg: {
      autoWidth: true,
      items: {
        layout: 'follow',
      },
    },
    // onReady(graph) {
    //   graph.on('node:click', (evt) => {
    //     // console.log(evt);
    //     const id = evt.item?._cfg?.id;
    //     console.log(id);
    //   });
    // },
    layout: {
      getWidth: () => {
        return 60;
      },
      getHeight: () => {
        return 120
      },
      // getVGap: () => {
      //   // 每个节点的垂直间隙，会结合 getHeight 返回值使用
      //   return 16;
      // },
      // getHGap: () => {
      //   // 每个节点的水平间隙，会结合 getWidth 返回值使用
      //   return 100;
      // },
    },
    menuCfg: {
      customContent(evt) {
        return <Space>
          <Tooltip title={'查看'}>
            <Button
              size={'small'}
              onClick={() => {

              }}
              icon={<SearchOutlined />}
            />
          </Tooltip>
          <Tooltip title={'修改'}>
            <Button
              size={'small'}
              onClick={() => {

              }}
              icon={<FormOutlined />}
            />
          </Tooltip>
          <Tooltip title={'添加订阅节点'}>
            <Button
              size={'small'}
              onClick={() => {
                if (evt?.item?._cfg?.id === undefined) {
                  return;
                }
                const node = findNode(dataSource.children!, evt?.item?._cfg?.id);
                // 添加子节点
                if (node) {
                  node.children = node.children || [];
                  node.children.push({
                    id: "a" + new Date().getTime(),
                    value: {
                      title: '新节点',
                      items: [
                        {
                          text: '执行状态',
                          value: '执行成功',
                          icon: null,
                        },
                        {
                          text: '执行耗时',
                          value: '0.046ms',
                          icon: null,
                        },
                        {
                          text: '开始执行时间',
                          value: '2023-07-01 20:22:02',
                          icon: null,
                        },
                        {
                          text: '执行完成时间',
                          value: '2023-07-01 20:22:02',
                          icon: null,
                        },
                      ],
                    },
                  });
                  setDataSource({ ...dataSource });
                }
              }}
              icon={<SubnodeOutlined />}
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

  return <DecompositionTreeGraph {...config} />;
};

export default DemoDecompositionTreeGraph;