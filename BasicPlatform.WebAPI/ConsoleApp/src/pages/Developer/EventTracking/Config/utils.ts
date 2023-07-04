import { G6TreeGraphData } from "@ant-design/graphs";

// 递归，根据API.EventTrackingConfigDetailItem转成G6TreeGraphData
export const convert = (item: API.EventTrackingConfigDetailItem, parent?: G6TreeGraphData) => {
  const node: G6TreeGraphData = {
    id: item.id!,
    value: {
      title: item.eventName,
      items: parent === undefined ?
        [
          {
            text: "事件实体类型",
            value: item.eventTypeName,
          }
        ] :
        [
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
    },
    children: [],
  };
  if (parent) {
    parent.children?.push(node);
  }
  if (item.children) {
    item.children.forEach((child) => {
      convert(child, node);
    });
  }
  return node;
};

//从data中根据id读取数据，data是一个树形结构
export const findNode = (items: G6TreeGraphData[], id: string): G6TreeGraphData | undefined => {
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