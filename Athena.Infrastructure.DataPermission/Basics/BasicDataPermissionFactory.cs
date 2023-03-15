namespace Athena.Infrastructure.DataPermission.Basics;

/// <summary>
/// 基础数据权限工厂类
/// </summary>
public class BasicDataPermissionFactory
{
    private readonly IEnumerable<IBasicDataPermission> _services;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="services"></param>
    public BasicDataPermissionFactory(IEnumerable<IBasicDataPermission> services)
    {
        _services = services;
    }

    /// <summary>
    /// 获取实例列表
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IBasicDataPermission> GetInstanceList()
    {
        return _services.ToList();
    }

    /// <summary>
    /// 获取下拉选择框列表
    /// </summary>
    /// <returns></returns>
    public IList<dynamic> GetSelectList()
    {
        var list = new List<dynamic>();
        list.AddRange(_services
            .Select(p => new
            {
                p.Label,
                p.Value
            })
        );
        return list;
    }
}