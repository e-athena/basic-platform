import { get, paging, post, put } from '@/utils/request';

/** 列表 */
export function query(params: API.ArticlePagingParams) {
  return paging<API.ArticleListItem>('/api/Article/GetPaging', params);
}

/** 详情 */
export function detail(id: string) {
  return get<API.ArticleInfo>('/api/Article/Get', { id });
}

/** 创建 */
export function create(data: API.CreateArticleRequest) {
  return post<API.CreateArticleRequest, string>('/api/Article/Post', data);
}

/** 更新 */
export function update(data: API.UpdateArticleRequest) {
  return put<API.UpdateArticleRequest, string>('/api/Article/Put', data);
}

/** 发布/取消发布 */
export function publish(id: string) {
  return put<IdRequest, string>('/api/Article/Publish', { id });
}
