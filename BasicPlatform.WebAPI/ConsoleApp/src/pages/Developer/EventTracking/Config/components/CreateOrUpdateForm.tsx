import { ModalForm } from '@ant-design/pro-components';
import { Button, Empty, Space, message } from 'antd';
import React, { useEffect } from 'react';
import { FormOutlined, SubnodeOutlined } from '@ant-design/icons';
import { DecompositionTreeGraph, DecompositionTreeGraphConfig, G6TreeGraphData } from '@ant-design/graphs';
import { convert, findNode } from '../utils';
import NodeForm from './NodeForm';
import CreateRootNodeForm from './CreateRootNodeForm';
import { submitHandle } from '@/utils/utils';
import { save } from '../service';

type CreateOrUpdateFormProps = {
  onCancel: () => void;
  onSuccess: () => void;
  open: boolean;
  values?: API.EventTrackingConfigDetailItem;
};

const CreateOrUpdateForm: React.FC<CreateOrUpdateFormProps> = (props) => {
  const [dataConfigs, setDataConfigs] = React.useState<API.EventTrackingConfigItem[]>([]);
  const [currentConfig, setCurrentConfig] = React.useState<API.EventTrackingConfigItem>();
  const [hangleTarget, setHandleTarget] = React.useState<number>(0);
  const [dataSource, setDataSource] = React.useState<G6TreeGraphData>();
  const [currentNode, setCurrentNode] = React.useState<G6TreeGraphData>();
  const [nodeModalOpen, handleNodeModalOpen] = React.useState<boolean>(false);
  const [createRootNodeModalOpen, handleCreateRootNodeModalOpen] = React.useState<boolean>(false);
  // `n${new Date().getTime()}`
  useEffect(() => {
    if (props.open && props.values !== undefined) {
      const data = convert(props.values!, undefined);
      setDataSource(data);
      // 将props.values数据展开，保存到dataConfigs中
      const configs: API.EventTrackingConfigItem[] = [];
      const loop = (item: API.EventTrackingConfigDetailItem) => {
        configs.push({
          ...item as API.EventTrackingConfigItem,
        });
        if (item.children) {
          item.children.forEach(loop);
        }
      }
      loop(props.values!);
      setDataConfigs(configs);
    }
  }, [props.open]);
  const isEmpty = props.values === undefined && dataSource === undefined;
  const config: DecompositionTreeGraphConfig | undefined = isEmpty ? undefined :
    {
      data: dataSource!,
      autoFit: false,
      nodeCfg: {
        autoWidth: true,
        items: {
          layout: 'follow',
        },
      },
      layout: {
        getWidth: () => {
          return 200;
        },
        getHeight: () => {
          return 120
        },
        // getHGap: () => {
        //   return 80;
        // }
      },
      edgeCfg: {
        type: 'polyline'
      },
      menuCfg: {
        customContent(evt) {
          return <Space>
            <Button
              title='修改'
              size={'small'}
              onClick={() => {
                const id = evt?.item?._cfg?.id;
                if (id === undefined) {
                  setCurrentConfig(undefined);
                  setCurrentNode(undefined);
                  return;
                }
                // id是否为根节点
                if (dataSource?.id === id) {
                  // todo 待处理
                  console.log('根节点');
                  // return;
                }
                const node = findNode(dataSource!.children!, id);
                setHandleTarget(0);
                setCurrentConfig(dataConfigs.find(item => item.id === id));
                setCurrentNode(node);
                handleNodeModalOpen(true);
              }}
              icon={<FormOutlined />}
            />
            <Button
              title='添加订阅节点'
              size={'small'}
              onClick={() => {
                const id = evt?.item?._cfg?.id;
                if (id === undefined) {
                  setCurrentConfig(undefined);
                  setCurrentNode(undefined);
                  return;
                }
                setHandleTarget(1);
                const config = dataConfigs.find(item => item.id === id);
                setCurrentConfig(config);
                const node = dataSource?.id === id ? dataSource : findNode(dataSource!.children!, id);
                setCurrentNode(node);
                handleNodeModalOpen(true);
              }}
              icon={<SubnodeOutlined />}
            />
          </Space>;
        },
      },
      behaviors: [
        'drag-canvas',
        'zoom-canvas',
        'drag-node'
      ],
    };
  return (
    <>
      <ModalForm
        width={'75%'}
        title={'配置事件追踪'}
        open={props.open}
        modalProps={{
          onCancel: () => {
            props.onCancel();
            setDataConfigs([]);
            setCurrentConfig(undefined);
            setHandleTarget(0);
            setDataSource(undefined);
          },
          destroyOnClose: true,
          maskClosable: false,
        }}
        onFinish={async () => {
          if (dataConfigs.length === 0) {
            message.warning('请添加配置');
            return;
          }
          if (dataConfigs.length === 1) {
            message.warning('至少需要一个处理节点');
            return;
          }
          const succeed = await submitHandle(save, { configs: dataConfigs });
          if (succeed) {
            props.onSuccess();
          }
        }}
        initialValues={{
        }}
      >
        {config === undefined ? <Empty
          image="https://gw.alipayobjects.com/zos/antfincdn/ZHrcdLPrvN/empty.svg"
          imageStyle={{ height: 70 }}
          style={{ marginBottom: 150, marginTop: 150 }}
          description={<span>暂无配置</span>}
        >
          <Button
            type="primary"
            onClick={() => {
              handleCreateRootNodeModalOpen(true);
            }}
          >
            添加根节点配置
          </Button>
        </Empty> : <DecompositionTreeGraph {...config} />}
      </ModalForm>
      {currentConfig && currentNode && (<NodeForm
        onCancel={() => {
          handleNodeModalOpen(false);
          setCurrentConfig(undefined);
          setCurrentNode(undefined);
        }}
        onSuccess={(items, node, target) => {
          // console.log(item, node,target);
          if (target === 0) {
            const item = items[0];
            // 修改setDataConfigs
            const index = dataConfigs.findIndex(item => item.id === node.id);
            if (index !== -1) {
              dataConfigs.splice(index, 1, item);
              setDataConfigs([...dataConfigs]);
            }
            // 修改节点
            node.value = {
              title: item.eventName,
              items: [
                {
                  text: "事件类型",
                  value: item.eventTypeTitle,
                },
                {
                  text: "事件处理器",
                  value: item.processorName,
                },
                {
                  text: "事件实体类型",
                  value: item.eventTypeName,
                }
              ]
            }
            setDataSource({ ...dataSource! });
          }
          if (target === 1) {
            // 添加到setDataConfigs
            setDataConfigs([...dataConfigs, ...items])
            // 添加子节点
            node.children = node.children || [];
            // node.children.push({
            //   id: item.id,
            //   value: {
            //     title: item.eventName,
            //     items: [
            //       {
            //         text: "事件类型",
            //         value: item.eventTypeTitle,
            //       },
            //       {
            //         text: "事件处理器",
            //         value: item.processorName,
            //       },
            //       {
            //         text: "事件实体类型",
            //         value: item.eventTypeName,
            //       }
            //     ]
            //   },
            //   children: [],
            // })
            setDataSource({ ...dataSource! });
          }
          handleNodeModalOpen(false);
          setCurrentConfig(undefined);
          setCurrentNode(undefined);
        }}
        open={nodeModalOpen}
        values={currentConfig}
        node={currentNode}
        target={hangleTarget}
      />)}
      <CreateRootNodeForm
        onCancel={() => {
          handleCreateRootNodeModalOpen(false);
        }}
        onSuccess={(items, node) => {
          // 添加到setDataConfigs
          setDataConfigs(items)
          setDataSource(node);
          handleCreateRootNodeModalOpen(false)
        }}
        open={createRootNodeModalOpen}
      />
    </>
  );
};

export default CreateOrUpdateForm;
