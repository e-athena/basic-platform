import { FilterGroupItem, FilterItem } from "@/components/AdvancedSearch/components/RulerItem";

const convertSubFiltersToText = (subFilters: FilterItem[]): string => {
  let text = "";

  for (let i = 0; i < subFilters.length; i++) {
    const subFilter = subFilters[i];
    const { key, value, operator, propertyType } = subFilter;

    if (i > 0) {
      if (subFilter.xor === "or") {
        text += " 或 ";
      } else {
        text += " 且 ";
      }
    }

    if (propertyType === "string") {
      text += `${key}包含"${value}"`;
    } else if (propertyType === "enum") {
      text += `${key}属于"${value}"`;
    } else if (propertyType === "dateTime") {
      const [start, end] = value.split(",");
      text += `${key}介于"${start}"和"${end}"之间`;
    } else {
      text += `${key}${operator}"${value}"`;
    }
  }

  return text;
}

export const convertFiltersToText = (filters: FilterGroupItem[]): string => {
  let text = "";

  for (let i = 0; i < filters.length; i++) {
    const filter = filters[i];
    const subFilters = filter.filters;
    const groupText = convertSubFiltersToText(subFilters);

    if (i > 0) {
      text += " 或 ";
    }
    text += groupText;
  }

  return text;
}