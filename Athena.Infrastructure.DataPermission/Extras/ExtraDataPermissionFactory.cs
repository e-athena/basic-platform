namespace Athena.Infrastructure.DataPermission.Extras;

/// <summary>
/// 扩展数据权限工厂类
/// </summary>
public class ExtraDataPermissionFactory
{
    private readonly IEnumerable<IExtraDataPermission> _services;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="services"></param>
    public ExtraDataPermissionFactory(IEnumerable<IExtraDataPermission> services)
    {
        _services = services;
    }

    /// <summary>
    /// 获取实例列表
    /// </summary>
    /// <returns></returns>
    public IList<IExtraDataPermission> GetInstances()
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
                p.Key,
                p.Value
            })
        );
        return list;
    }
}