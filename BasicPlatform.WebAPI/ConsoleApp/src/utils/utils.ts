import { message, Modal } from "antd";
import { SortOrder } from "antd/es/table/interface";

/**
 * 是否有权限
 * @param key 权限代码
 * @param resource API资源
 * @returns 
 */
export const canAccessible = (key: string, resource: API.ResourceInfo | null): boolean => {
  if (resource === null || resource?.functions === null || resource?.functions?.length === 0) {
    return false;
  }
  const permissions = resource?.functions?.map((p) => p.key);
  return permissions?.includes(key) || false;
}

/**
 * 是否有权限
 * @param keys 权限代码
 * @param resource API资源
 * @returns 
 */
export const hasPermission = (keys: string[], resource: API.ResourceInfo | null): boolean => {
  if (resource === null || resource?.functions === null || resource?.functions?.length === 0) {
    return false;
  }
  const permissions = resource?.functions?.map((p) => p.key);
  return keys.some((p) => permissions?.includes(p));
}

/**
 * 提交处理
 * @param {*} func
 * @param {*} fields
 */
export async function submitHandle<T>(func: (values: T) => Promise<ApiResponse<boolean | string>>, fields: T): Promise<boolean> {
  const hide = message.loading('处理中');
  try {
    const res = await func(fields);
    hide();
    if (res.success) {
      message.success('处理成功');
      return true;
    }
    Modal.error({
      title: '处理失败',
      content: res.message,
    });
    return false;
  } catch (error) {
    hide();
    Modal.error({
      title: '处理失败',
      content: '请重试或联系管理员！',
    });
    return false;
  }
};


/**
 * 获取Sorter
 * @param {*} sorter
 */
export const getSorter = (sorter: Record<string, SortOrder>, alias?: string) => {
  let str = '';
  if (Object.keys(sorter).length > 0) {
    const key = Object.keys(sorter)[0];
    const value = sorter[key];

    str = `${key.charAt(0).toUpperCase() + key.slice(1)} ${value === 'descend' ? 'DESC' : 'ASC'}`;
  }

  return str === '' ? {} : { sorter: alias ? `${alias}.${str}` : str };
};

/**
 * 获取Filter
 * @param {*} filter
 */
export const getFilter = (filter: Record<string, (string | number)[] | null>) => {
  const obj: Record<string, any> = {};
  if (!filter) {
    return obj;
  }
  if (Object.keys(filter).length > 0) {
    for (let i = 0; i < Object.keys(filter).length; i++) {
      const key = Object.keys(filter)[i];
      const value = filter[key];
      if (value) {
        obj[key] = value.map((p) => {
          if (p === 'true') {
            return true;
          }
          if (p === 'false') {
            return false;
          }
          if (new Number(p).toString() === 'NaN') {
            return p;
          }
          if (typeof p === 'number') {
            return p;
          }
          return parseInt(p);
        });
      }
    }
  }
  return obj;
};

/**
 * 生成文字图片
 * @param text 文本
 * @param width 宽
 * @param height 高
 * @returns 
 */
export const generateTextImage = (text: string, width: number, height: number) => {
  let size = [width, height];
  let firstName = text.substring(1, 0);
  let colors = [
    "rgb(239,150,26)",
    "rgb(255,58,201)",
    "rgb(111,75,255)",
    "rgb(36,174,34)",
    "rgb(80,80,80)",
  ];
  let cvs = document.createElement("canvas");
  cvs.setAttribute('width', `${size[0]}`);
  cvs.setAttribute('height', `${size[1]}`);
  let ctx = cvs.getContext("2d");
  if (ctx === null) return;
  ctx.fillStyle = colors[Math.floor(Math.random() * (colors.length))];
  ctx.fillRect(0, 0, size[0], size[1]);
  ctx.fillStyle = 'rgb(255,255,255)';
  ctx.font = size[0] * 0.6 + "px Arial";
  ctx.textBaseline = "middle";
  ctx.textAlign = "center";
  ctx.fillText(firstName, size[0] / 2, size[1] / 2);

  return cvs.toDataURL('image/jpeg', 1);
};