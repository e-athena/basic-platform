using System.Reflection;
using System.Xml;
using System.Xml.XPath;
using Athena.Infrastructure.DataPermission.Attributes;
using Athena.Infrastructure.Enums;
using Athena.Infrastructure.Summaries;

namespace Athena.Infrastructure.DataPermission;

public abstract class DataPermissionHelper
{
    /// <summary>
    /// 读取策略树状数据列表
    /// </summary>
    /// <returns></returns>
    public static List<TreeSelectInfo> GetStrategyTreeList(string assembly)
    {
        var asm = Assembly.Load(assembly);
        var types = asm.GetExportedTypes();

        bool IsMyAttribute(IEnumerable<Attribute> o) => o.OfType<DataPermissionAttribute>().Any();
        var typeList = types
            .Where(o => IsMyAttribute(Attribute.GetCustomAttributes(o, false)))
            .ToList();
        var attributes = new List<DataPermissionAttribute>();
        var permissionTypes = new List<Type>();
        foreach (var type in typeList)
        {
            if (type.GetCustomAttributes(typeof(DataPermissionAttribute), false)
                    .FirstOrDefault() is not DataPermissionAttribute attribute)
            {
                continue;
            }

            attribute.Key = type.Name;
            if (string.IsNullOrWhiteSpace(attribute.DisplayName))
            {
                attribute.DisplayName = GetSummaryName(type, assembly);
            }

            attributes.Add(attribute);
            permissionTypes.Add(type);
        }

        var list = GetTreeSelectList(permissionTypes.ToArray(), assembly);
        var result = new List<TreeSelectInfo>();
        foreach (var item in list)
        {
            var dp = attributes.First(p => p.Key == item.Key);
            var parent = new TreeSelectInfo
            {
                Label = dp.DisplayName!,
                Key = item.Key,
                Value = item.Key,
                PropertyType = "class",
                Children = item.Children
            };

            result.Add(parent);
        }

        return result;
    }

    /// <summary>
    /// 获取树形结构列表
    /// </summary>
    /// <returns></returns>
    private static List<TreeSelectInfo> GetTreeSelectList(IEnumerable<Type> types, string assemblyName)
    {
        XPathNavigator? xmlNavigator = null;
        var path = AppDomain.CurrentDomain.BaseDirectory;
        // 找出里面的xml
        var fileInfo = new DirectoryInfo(path).GetFiles()
            // 读取文件后缀名为.xml的文件信息
            .Where(p => p.Extension.ToLower() == ".xml")
            .FirstOrDefault(n => assemblyName.Contains(n.Name.Replace(n.Extension, "")));
        if (fileInfo != null)
        {
            var document = new XmlDocument();
            document.Load(fileInfo.OpenRead());
            xmlNavigator = document.CreateNavigator();
        }

        var summaryList = new List<TreeSelectInfo>();
        foreach (var item in types)
        {
            var summaryName = GetTypeSummaryName(xmlNavigator, item);

            var summary = new TreeSelectInfo
            {
                Label = summaryName,
                Key = item.Name,
                Children = new List<TreeSelectInfo>()
            };
            foreach (var property in item.GetProperties())
            {
                var label = GetPropertySummaryName(xmlNavigator, property);

                var dataType = "";
                var enumOptions = new List<dynamic>();
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
                    dataType = "number";
                }
                else if (
                    property.PropertyType == typeof(DateTime) ||
                    property.PropertyType == typeof(DateTime?)
                )
                {
                    dataType = "dateTime";
                }
                else if (
                    property.PropertyType == typeof(bool) ||
                    property.PropertyType == typeof(bool?)
                )
                {
                    dataType = "boolean";
                }
                else if (property.PropertyType.IsEnum)
                {
                    dataType = "enum";
                    var enums = Enum.GetValues(property.PropertyType);
                    var enumList = enums.OfType<Enum>().ToList();
                    enumOptions.AddRange(enumList.Select(p => new
                    {
                        Label = p.ToDescription(),
                        Value = p.GetHashCode().ToString()
                    }));
                }
                else if (
                    property.PropertyType == typeof(string) ||
                    property.PropertyType == typeof(Guid) ||
                    property.PropertyType == typeof(Guid?))
                {
                    dataType = "string";
                }

                if (string.IsNullOrEmpty(dataType))
                {
                    continue;
                }


                summary.Children.Add(new TreeSelectInfo
                {
                    PropertyType = dataType,
                    Key = property.Name,
                    Value = property.Name,
                    Label = label,
                    EnumOptions = enumOptions
                });
            }

            summaryList.Add(summary);
        }

        return summaryList;
    }

    /// <summary>
    /// 读取注释内容
    /// </summary>
    /// <param name="type"></param>
    /// <param name="assemblyName"></param>
    /// <returns></returns>
    private static string GetSummaryName(Type type, string assemblyName)
    {
        XPathNavigator? xmlNavigator = null;
        var path = AppDomain.CurrentDomain.BaseDirectory;
        // 找出里面的xml
        var fileInfo = new DirectoryInfo(path).GetFiles()
            // 读取文件后缀名为.xml的文件信息
            .Where(p => p.Extension.ToLower() == ".xml")
            .FirstOrDefault(n => assemblyName.Contains(n.Name.Replace(n.Extension, "")));
        if (fileInfo != null)
        {
            var document = new XmlDocument();
            document.Load(fileInfo.OpenRead());
            xmlNavigator = document.CreateNavigator();
        }

        return GetTypeSummaryName(xmlNavigator, type);
    }

    /// <summary>
    /// 读取类注释
    /// </summary>
    /// <param name="xmlNavigator"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    private static string GetTypeSummaryName(
        XPathNavigator? xmlNavigator,
        Type type)
    {
        if (xmlNavigator == null)
        {
            return type.Name;
        }

        var propertyMemberName = XmlCommentsNodeNameHelper.GetMemberNameForType(type);
        var propertySummaryNode =
            xmlNavigator.SelectSingleNode($"/doc/members/member[@name='{propertyMemberName}']/summary");
        return propertySummaryNode != null ? XmlCommentsTextHelper.Humanize(propertySummaryNode.InnerXml) : type.Name;
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

/// <summary>
/// Select Info
/// </summary>
public class TreeSelectInfo
{
    /// <summary>
    /// 显示名称
    /// </summary>
    public string Label { get; set; } = null!;

    /// <summary>
    /// 值
    /// </summary>
    public string Value { get; set; } = null!;

    /// <summary>
    /// Key
    /// </summary>
    public string Key { get; set; } = Guid.NewGuid().ToString("N");

    /// <summary>
    /// 属性类型
    /// </summary>
    public string? PropertyType { get; set; }

    /// <summary>
    /// 子项
    /// </summary>
    public List<TreeSelectInfo>? Children { get; set; }

    /// <summary>
    /// 枚举选项列表
    /// </summary>
    public List<dynamic>? EnumOptions { get; set; }
}