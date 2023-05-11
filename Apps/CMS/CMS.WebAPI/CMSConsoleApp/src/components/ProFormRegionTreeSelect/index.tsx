import { queryTreeSelectList } from '@/services/ant-design-pro/basics/region';
import { ProFormTreeSelect } from '@ant-design/pro-components';
import { useState } from 'react';

type ProFormRegionTreeSelectProps = {
  multiple?: boolean;
  label?: string;
  name?: string;
  placeholder?: string;
};

const ProFormRegionTreeSelect: React.FC<ProFormRegionTreeSelectProps> = (props) => {
  const [regions, setRegions] = useState<API.TreeSelectInfo[]>([]);
  return (
    <>
      <ProFormTreeSelect
        label={props.label || '所属区域'}
        name={props.name || 'regionId'}
        allowClear
        fieldProps={{
          multiple: props.multiple,
          showSearch: true,
          treeNodeFilterProp: 'label',
          treeLine: true,
        }}
        width={'md'}
        placeholder={props.placeholder || '为空则所有区域可用'}
        request={async () => {
          if (regions.length > 0) {
            return regions;
          }
          const res = await queryTreeSelectList();
          console.log(res);
          const data = res.success ? res.data || [] : [];
          setRegions(data);
          return data;
        }}
      />
    </>
  );
};

export default ProFormRegionTreeSelect;
