import { get, paging } from '@/utils/request';
import { G6TreeGraphData } from '@ant-design/graphs';

/** 列表 */
export function query(params: API.EventTrackingPagingParams) {
  return paging<API.EventTrackingListItem>('/api/EventTracking/GetPaging', params);
}

/** 详情 */
export function queryDecompositionTreeGraph(id: string) {
  return get<G6TreeGraphData>('/api/EventTracking/GetDecompositionTreeGraph', { id });
}

/** 详情 */
export function detail(id: string) {
  return get<API.EventTrackingListItem>('/api/EventTracking/Get', { id });
}