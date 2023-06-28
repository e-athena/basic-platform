import { TableColumnData } from '@arco-design/web-vue/es/table/interface';

/** Table Columns */
export type ProTableColumnData = {
  hideInTable?: boolean;
  hideInDescriptions?: boolean;
} & Partial<TableColumnData>;
