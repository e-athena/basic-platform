import { getMenuResource } from '@/utils/utils';
import { useModel, history } from '@umijs/max';

export default () => {
  const { initialState } = useModel('@@initialState');

  const getResource = (pathname?: string): API.ResourceInfo | null => {
    const path = pathname || history.location.pathname;
    return getMenuResource(initialState?.apiResources ? (initialState.apiResources[0]?.children || []) : [], path);
  };

  return { getResource };
};
