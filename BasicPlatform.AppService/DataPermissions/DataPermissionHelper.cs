using System.Reflection;
using System.Xml;
using System.Xml.XPath;
using Athena.Infrastructure.Summaries;
using BasicPlatform.AppService.DataPermissions.Attributes;
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
    /// <returns></returns>
    public static IEnumerable<DataPermission> GetList(string assembly)
    {
        var attributes = GetAttributeList(assembly);
        return attributes.Select(c => new DataPermission(
            c.DisplayName!, c.BaseType.FullName!
        )).ToList();
    }


    /// <summary>
    /// 读取数据权限配置树列表
    /// </summary>
    /// <returns></returns>
    public static IList<DataPermissionGroup> GetTreeList(Assembly assembly, IList<DataPermission>? permissions = null)
    {
        return GetTreeList(assembly.GetName().Name!, permissions);
    }

    /// <summary>
    /// 读取数据权限配置树列表
    /// </summary>
    /// <returns></returns>
    public static IList<DataPermissionGroup> GetTreeList(string assembly, IList<DataPermission>? permissions = null)
    {
        permissions ??= new List<DataPermission>();
        var attributes = GetAttributeList(assembly);
        var list = new List<DataPermissionGroup>();
        foreach (var group in attributes.GroupBy(p => p.Group))
        {
            var displayName = string.IsNullOrEmpty(group.Key) ? "默认分组" : group.Key;
            list.Add(new DataPermissionGroup(
                displayName,
                group.Select(
                    c =>
                    {
                        var resourceKey = c.BaseType.FullName!;
                        var item = permissions.FirstOrDefault(p => p.ResourceKey == resourceKey);
                        return new DataPermission(
                            c.DisplayName!,
                            c.BaseType.FullName!
                        )
                        {
                            Enabled = item is {Enabled: true},
                            DataScope = item?.DataScope ?? RoleDataScope.All,
                            DataScopeCustom = item?.DataScopeCustom,
                            DisableChecked = item?.DisableChecked ?? false
                        };
                    }
                ).ToList()));
        }

        return list;
    }

    #region 私有方法

    /// <summary>
    /// 缓存
    /// </summary>
    private static readonly Dictionary<string, IEnumerable<DataPermissionAttribute>> CacheDictionary = new();

    /// <summary>
    /// 读取数据权限配置树列表
    /// </summary>
    /// <returns></returns>
    private static IEnumerable<DataPermissionAttribute> GetAttributeList(string assembly)
    {
        if (CacheDictionary.ContainsKey(assembly))
        {
            return CacheDictionary[assembly];
        }

        XPathNavigator? xmlNavigator = null;
        var path = AppDomain.CurrentDomain.BaseDirectory;
        // 找出里面的xml
        var fileInfo = new DirectoryInfo(path).GetFiles()
            // 读取文件后缀名为.xml的文件信息
            .Where(p => p.Extension.ToLower() == ".xml")
            .FirstOrDefault(n => assembly.Contains(n.Name.Replace(n.Extension, "")));
        if (fileInfo != null)
        {
            var document = new XmlDocument();
            document.Load(fileInfo.OpenRead());
            xmlNavigator = document.CreateNavigator();
        }

        var asm = Assembly.Load(assembly);
        var types = asm.GetExportedTypes();

        bool IsMyAttribute(IEnumerable<Attribute> o) => o.OfType<DataPermissionAttribute>().Any();
        var typeList = types
            .Where(o => IsMyAttribute(Attribute.GetCustomAttributes(o, false)))
            .ToList();
        var attributes = new List<DataPermissionAttribute>();
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

            attributes.Add(attribute);
        }

        // 添加到缓存
        CacheDictionary.TryAdd(assembly, attributes);
        return attributes;
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

    #endregion
}