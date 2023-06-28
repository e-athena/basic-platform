declare namespace API {
  /** 创建 */
  type CreateArticleRequest = {
    title: string;
    summary: string;
    content: string;
    author: string;
    source: string;
    sourceUrl: string;
    cover: string;
  };
  /** 更新 */
  type UpdateArticleRequest = {
    id: string;
  } & Partial<CreateArticleRequest>;

  /**
   * 列表
   */
  type ArticleListItem = Partial<ArticleInfo>;

  /** 详情 */
  type ArticleInfo = {
    id?: string;
    title: string;
    summary: string;
    content: string;
    author: string;
    source: string;
    sourceUrl: string;
    cover: string;
    viewCount: number;
    likeCount: number;
    commentCount: number;
    isTop: boolean;
    isRecommend: boolean;
    isPublish: boolean;
    publishTime: string;
  } & Partial<CreatedInfo> &
    Partial<UpdatedInfo>;

  /**
   * 分页请求参数
   */
  type ArticlePagingParams = {
    title?: string;
  } & Partial<Record<string, any>>;
}
