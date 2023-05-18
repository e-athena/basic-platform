import { get } from '@/utils/request';

/** 列表 */
export function queryProductCascaders() {
  return get<API.TreeSelectInfo[]>('/api/Product/getCascaderList');
}

/** 产品分类列表 */
export function queryProductCategorySelectList() {
  return get<API.SelectInfo[]>('/api/ProductCategory/getSelectList');
}

/** 产品规格列表 */
export function queryProductRecipeSelectList(productId: string) {
  return get<API.SelectInfo[]>('/api/ProductRecipe/getSelectList', { productId });
}
