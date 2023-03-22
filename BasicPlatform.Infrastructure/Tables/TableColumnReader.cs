using System.Reflection;
using System.Xml;
using System.Xml.XPath;
using Athena.Infrastructure.Enums;
using Athena.Infrastructure.Summaries;

namespace BasicPlatform.Infrastructure.Tables;

public abstract class TableColumnReader
{
    /// <summary>
    /// 缓存表达式树
    /// </summary>
    private static readonly Dictionary<string, IList<TableColumnInfo>> TableColumnCacheDict = new();

    /// <summary>
    /// 获取实体注释
    /// </summary>
    /// <returns></returns>
    public static IList<TableColumnInfo> GetTableColumns(Type type)
    {
        var key = $"Key_{type.FullName}";
        if (TableColumnCacheDict.ContainsKey(key))
        {
            return TableColumnCacheDict[key];
        }

        XPathNavigator? xmlNavigator = null;
        var assemblyName = type.Assembly.GetName().Name;
        var path = AppDomain.CurrentDomain.BaseDirectory;
        // 找出里面的xml
        var fileInfo = new DirectoryInfo(path).GetFiles()
            // 读取文件后缀名为.xml的文件信息
            .Where(p => p.Extension.ToLower() == ".xml")
            .FirstOrDefault(n => assemblyName!.Contains(n.Name.Replace(n.Extension, "")));
        if (fileInfo != null)
        {
            var document = new XmlDocument();
            document.Load(fileInfo.OpenRead());
            xmlNavigator = document.CreateNavigator();
        }

        var list = new List<TableColumnInfo>();
        foreach (var property in type.GetProperties())
        {
            var tableColumnInfo = new TableColumnInfo();
            // 如果包含了TableColumnAttribute特性，则读取
            if (property.IsDefined(typeof(TableColumnAttribute), true))
            {
                var attribute = property.GetCustomAttribute<TableColumnAttribute>(true);
                if (attribute != null)
                {
                    if (attribute.Ignore)
                    {
                        continue;
                    }

                    tableColumnInfo.Width = attribute.Width == -1 ? null : attribute.Width;
                    tableColumnInfo.HideInTable = attribute.HideInTable;
                    tableColumnInfo.HideInSearch = attribute.HideInSearch;
                    tableColumnInfo.Required = attribute.Required;
                    tableColumnInfo.Fixed = attribute.Fixed;
                    tableColumnInfo.Sort = attribute.Sort;
                    tableColumnInfo.Ellipsis = attribute.Ellipsis;
                    tableColumnInfo.Align = attribute.Align;
                    tableColumnInfo.Sorter = attribute.Sorter;
                    tableColumnInfo.Filters = attribute.Filters;
                    tableColumnInfo.ValueType = attribute.ValueType;
                    tableColumnInfo.Title = attribute.Title;
                }
            }

            string propertyType;
            var valueType = "text";
            var enumOptions = new List<dynamic>();
            var valueEnumDict = new Dictionary<int, dynamic>();
            if (
                property.PropertyType == typeof(int) ||
                property.PropertyType == typeof(int?) ||
                property.PropertyType == typeof(long) ||
                property.PropertyType == typeof(long?) ||
                property.PropertyType == typeof(decimal) ||
                property.PropertyType == typeof(decimal?) ||
                property.PropertyType == typeof(double) ||
                property.PropertyType == typeof(double?)
            )
            {
                propertyType = "number";
                if (
                    property.PropertyType == typeof(decimal) ||
                    property.PropertyType == typeof(decimal?))
                {
                    valueType = "money";
                }
            }
            else if (
                property.PropertyType == typeof(DateTime) ||
                property.PropertyType == typeof(DateTime?)
            )
            {
                // 高级查询 Generic queries
                propertyType = "dateTime";
                valueType = "dateTime";
            }
            else if (
                property.PropertyType == typeof(bool) ||
                property.PropertyType == typeof(bool?)
            )
            {
                propertyType = "boolean";
            }
            else if (property.PropertyType.IsEnum)
            {
                propertyType = "enum";
                valueType = "select";
                var enums = Enum.GetValues(property.PropertyType);
                var enumList = enums.OfType<Enum>().ToList();
                enumOptions.AddRange(enumList.Select(p => new
                {
                    Label = p.ToDescription(),
                    Value = p.GetHashCode().ToString()
                }));
                foreach (var @enum in enumList)
                {
                    valueEnumDict.Add(@enum.GetHashCode(), new
                    {
                        Status = "Default",
                        Text = @enum.ToDescription()
                    });
                }
            }
            else if (
                property.PropertyType == typeof(string) ||
                property.PropertyType == typeof(Guid) ||
                property.PropertyType == typeof(Guid?))
            {
                propertyType = "string";
            }
            else
            {
                propertyType = "string";
            }

            tableColumnInfo.PropertyType = propertyType;
            tableColumnInfo.PropertyName = property.Name;
            tableColumnInfo.EnumOptions = enumOptions;
            // 首字母小写
            tableColumnInfo.DataIndex = property.Name[..1].ToLower() + property.Name[1..];
            tableColumnInfo.Title ??= GetPropertySummaryName(xmlNavigator, property);
            if (tableColumnInfo.ValueType == null)
            {
                tableColumnInfo.ValueType = valueType;
                tableColumnInfo.ValueEnum = valueEnumDict;
            }

            if (tableColumnInfo.ValueType == "dateTime")
            {
                tableColumnInfo.Sorter = true;
                tableColumnInfo.Width = 165;
            }

            list.Add(tableColumnInfo);
        }

        list = list.OrderBy(p => p.Sort).ToList();

        TableColumnCacheDict.TryAdd(key, list);
        return list;
    }

    /// <summary>
    /// 读取属性注释
    /// </summary>
    /// <param name="xmlNavigator"></param>
    /// <param name="property"></param>
    /// <returns></returns>
    private static string GetPropertySummaryName(
        XPathNavigator? xmlNavigator,
        MemberInfo property)
    {
        if (xmlNavigator == null)
        {
            return property.Name;
        }

        var propertyMemberName = XmlCommentsNodeNameHelper.GetMemberNameForFieldOrProperty(property);
        var propertySummaryNode =
            xmlNavigator.SelectSingleNode($"/doc/members/member[@name='{propertyMemberName}']/summary");
        return propertySummaryNode != null
            ? XmlCommentsTextHelper.Humanize(propertySummaryNode.InnerXml)
            : property.Name;
    }
}