import { message, Modal } from "antd";
import { SortOrder } from "antd/es/table/interface";

/**
 * 是否有权限
 * @param code 权限代码
 * @param resource API资源
 * @returns 
 */
export const canAccessible = (code: string, resource: API.ResourceInfo | null): boolean => {
  if (resource === null || resource?.functions === null || resource?.functions?.length === 0) {
    return false;
  }
  const permissions = resource?.functions?.map((p) => p.key);
  return permissions?.includes(code) || false;
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