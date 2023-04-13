using System.Reflection;
using System.Xml;
using System.Xml.XPath;
using Athena.Infrastructure.Summaries;
using BasicPlatform.AppService.DataPermissions.Models;

namespace BasicPlatform.AppService.DataPermissions;

/// <summary>
/// 数据权限帮助类
/// </summary>
public static class DataPermissionHelper
{
    /// <summary>
    /// 读取数据权限配置列表
    /// </summary>
    /// <param name="assembly">程序集</param>
    /// <param name="appId">应用ID</param>
    /// <returns></returns>
    public static IEnumerable<DataPermission> GetList(Assembly assembly, string appId)
    {
        return GetList(assembly.GetName().Name!, appId);
    }

    /// <summary>
    /// 读取数据权限配置列表
    /// </summary>
    /// <param name="assembly">程序集</param>
    /// <param name="appId">应用ID</param>
    /// <param name="permissions"></param>
    /// <returns></returns>
    public static IEnumerable<DataPermission> GetList(Assembly assembly, string appId,
        IList<DataPermission>? permissions)
    {
        return GetList(assembly.GetName().Name!, appId, permissions);
    }

    /// <summary>
    /// 读取数据权限配置列表
    /// </summary>
    /// <param name="assemblyName">程序集名称</param>
    /// <param name="appId">应用ID</param>
    /// <returns></returns>
    public static IEnumerable<DataPermission> GetList(string assemblyName, string appId)
    {
        var dict = GetAttributeList(assemblyName);
        return dict.Keys.Select(c => new DataPermission(
            appId,
            c.DisplayName!,
            c.BaseType.Name
        )
        {
            Properties = dict[c]
        }).ToList();
    }

    /// <summary>
    /// 读取数据权限配置列表
    /// </summary>
    /// <param name="assemblyName">程序集名称</param>
    /// <param name="appId">应用ID</param>
    /// <param name="permissions"></param>
    /// <returns></returns>
    public static IEnumerable<DataPermission> GetList(string assemblyName, string appId,
        IList<DataPermission>? permissions)
    {
        var dict = GetAttributeList(assemblyName);
        if (permissions == null || permissions.Count == 0)
        {
            return dict.Keys.Select(c => new DataPermission(
                appId,
                c.DisplayName!,
                c.BaseType.Name
            )
            {
                Properties = dict[c]
            }).ToList();
        }

        return dict.Keys.Select(c =>
        {
            var resourceKey = c.BaseType.Name;
            var item = permissions.FirstOrDefault(p => p.ResourceKey == resourceKey);
            return new DataPermission(
                appId,
                c.DisplayName!,
                resourceKey
            )
            {
                PolicyResourceKey = c.Key,
                Enabled = item is {Enabled: true},
                DataScope = item?.DataScope ?? RoleDataScope.All,
                DataScopeCustom = item?.DataScopeCustom,
                DisableChecked = item?.DisableChecked ?? false,
                Properties = dict[c],
                QueryFilterGroups = item?.QueryFilterGroups ?? new List<QueryFilterGroup>()
            };
        }).ToList();
    }

    /// <summary>
    /// 读取数据权限配置树列表
    /// </summary>
    /// <param name="assembly">程序集</param>
    /// <param name="appId">应用ID</param>
    /// <param name="permissions">拥有的权限</param>
    /// <returns></returns>
    public static IList<DataPermissionGroup> GetGroupList(
        Assembly assembly,
        string appId,
        IList<DataPermission>? permissions = null)
    {
        return GetGroupList(assembly.GetName().Name!, appId, permissions);
    }

    /// <summary>
    /// 读取数据权限配置树列表
    /// </summary>
    /// <param name="assemblyName">程序集名称</param>
    /// <param name="appId">应用ID</param>
    /// <param name="permissions">拥有的权限</param>
    /// <returns></returns>
    public static IList<DataPermissionGroup> GetGroupList(
        string assemblyName,
        string appId,
        IList<DataPermission>? permissions = null
    )
    {
        permissions ??= new List<DataPermission>();
        var dictionary = GetAttributeList(assemblyName);
        var list = new List<DataPermissionGroup>();
        foreach (var group in dictionary.Keys.GroupBy(p => p.Group))
        {
            var displayName = string.IsNullOrEmpty(group.Key) ? "默认分组" : group.Key;
            list.Add(new DataPermissionGroup(
                displayName,
                group
                    .OrderBy(p => p.Sort)
                    .Select(
                        c =>
                        {
                            var resourceKey = c.BaseType.Name;
                            var item = permissions.FirstOrDefault(p => p.ResourceKey == resourceKey);
                            return new DataPermission(
                                appId,
                                c.DisplayName!,
                                resourceKey
                            )
                            {
                                PolicyResourceKey = c.Key,
                                Enabled = item is {Enabled: true},
                                DataScope = item?.DataScope ?? RoleDataScope.All,
                                DataScopeCustom = item?.DataScopeCustom,
                                DisableChecked = item?.DisableChecked ?? false,
                                Properties = dictionary[c],
                                QueryFilterGroups = item?.QueryFilterGroups ?? new List<QueryFilterGroup>()
                            };
                        }
                    )
                    .ToList())
            );
        }

        return list;
    }

    /// <summary>
    /// 获取树形结构列表
    /// </summary>
    /// <param name="assembly">程序集</param>
    /// <param name="appId">应用ID</param>
    /// <returns></returns>
    public static List<DataPermissionTree> GetTreeList(Assembly assembly, string appId)
    {
        return GetTreeList(assembly.GetName().Name!, appId);
    }

    /// <summary>
    /// 获取树形结构列表
    /// </summary>
    /// <param name="assemblyName"></param>
    /// <param name="appId"></param>
    /// <returns></returns>
    public static List<DataPermissionTree> GetTreeList(string assemblyName, string appId)
    {
        var asm = Assembly.Load(assemblyName);
        var types = asm.GetExportedTypes();
        bool IsMyAttribute(IEnumerable<Attribute> o) => o.OfType<DataPermissionAttribute>().Any();
        var typeList = types
            .Where(o => IsMyAttribute(Attribute.GetCustomAttributes(o, false)))
            .ToList();
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

        var summaryList = new List<DataPermissionTree>();
        foreach (var type in typeList)
        {
            var label = type.GetCustomAttribute<DataPermissionAttribute>()
                ?.DisplayName ?? GetTypeSummaryName(xmlNavigator, type);
            var summary = new DataPermissionTree
            {
                AppId = appId,
                Label = label,
                Key = type.Name,
                Value = type.Name,
                PropertyType = "type",
                Children = new List<DataPermissionTree>()
            };
            foreach (var property in type.GetProperties())
            {
                var columnInfo = property.GetCustomAttribute<TableColumnAttribute>();

                string dataType;
                var enumOptions = new List<dynamic>();
                if (
                    property.PropertyType == typeof(int) ||
                    property.PropertyType == typeof(int?) ||
                    property.PropertyType == typeof(long) ||
                    property.PropertyType == typeof(long?) ||
                    property.PropertyType == typeof(double) ||
                    property.PropertyType == typeof(double?)
                )
                {
                    dataType = "number";
                }
                else if (
                    property.PropertyType == typeof(decimal) ||
                    property.PropertyType == typeof(decimal?)
                )
                {
                    dataType = "digit";
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
                else
                {
                    dataType = "string";
                }

                if (string.IsNullOrEmpty(dataType))
                {
                    continue;
                }

                if (columnInfo is {Ignore: true})
                {
                    continue;
                }

                summary.Children.Add(new DataPermissionTree
                {
                    AppId = appId,
                    PropertyType = dataType,
                    Key = property.Name,
                    Value = property.Name,
                    Label = columnInfo?.Title ?? GetPropertySummaryName(xmlNavigator, property),
                    EnumOptions = enumOptions
                });
            }

            summaryList.Add(summary);
        }

        return summaryList;
    }

    #region 私有方法

    /// <summary>
    /// 缓存
    /// </summary>
    private static readonly Dictionary<string, IDictionary<DataPermissionAttribute, List<DataPermissionProperty>>>
        CacheDictionary = new();

    /// <summary>
    /// 读取数据权限配置树列表
    /// </summary>
    /// <returns></returns>
    private static IDictionary<DataPermissionAttribute, List<DataPermissionProperty>> GetAttributeList(
        string assemblyName)
    {
        if (CacheDictionary.TryGetValue(assemblyName, out var list))
        {
            return list;
        }

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

        var asm = Assembly.Load(assemblyName);
        var types = asm.GetExportedTypes();

        bool IsMyAttribute(IEnumerable<Attribute> o) => o.OfType<DataPermissionAttribute>().Any();
        var typeList = types
            .Where(o => IsMyAttribute(Attribute.GetCustomAttributes(o, false)))
            .ToList();
        var attributeDict = new Dictionary<DataPermissionAttribute, List<DataPermissionProperty>>();
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
                attribute.DisplayName = GetTypeSummaryName(xmlNavigator, type);
            }

            var propertyList = new List<dynamic>();
            foreach (var property in type.GetProperties())
            {
                var columnInfo = property.GetCustomAttribute<TableColumnAttribute>();

                #region 获取数据类型

                string dataType;
                var enumOptions = new List<dynamic>();
                if (
                    property.PropertyType == typeof(int) ||
                    property.PropertyType == typeof(int?) ||
                    property.PropertyType == typeof(long) ||
                    property.PropertyType == typeof(long?) ||
                    property.PropertyType == typeof(double) ||
                    property.PropertyType == typeof(double?)
                )
                {
                    dataType = "number";
                }
                else if (
                    property.PropertyType == typeof(decimal) ||
                    property.PropertyType == typeof(decimal?)
                )
                {
                    dataType = "digit";
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
                else
                {
                    dataType = "string";
                }

                #endregion

                if (string.IsNullOrEmpty(dataType))
                {
                    continue;
                }

                switch (columnInfo)
                {
                    case {Ignore: true}:
                    case {HideInSearch: true}:
                        continue;
                    default:
                        propertyList.Add(new
                        {
                            PropertyType = dataType,
                            Key = property.Name,
                            Value = property.Name,
                            Label = columnInfo?.Title ?? GetPropertySummaryName(xmlNavigator, property),
                            EnumOptions = enumOptions,
                            Sort = columnInfo?.Sort ?? 0
                        });
                        break;
                }
            }

            attributeDict.TryAdd(attribute, propertyList
                .OrderBy(p => p.Sort)
                .Select(p => new DataPermissionProperty
                {
                    PropertyType = p.PropertyType,
                    Key = p.Key,
                    Value = p.Value,
                    Label = p.Label,
                    EnumOptions = p.EnumOptions,
                }).ToList());
        }

        // 添加到缓存
        CacheDictionary.TryAdd(assemblyName, attributeDict);
        return attributeDict;
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

    #endregion
}