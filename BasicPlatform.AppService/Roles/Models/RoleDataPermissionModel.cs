using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BasicPlatform.AppService.Roles.Models;

/// <summary>
/// 数据数据权限模型
/// </summary>
public class RoleDataPermissionModel
{
    /// <summary>
    /// 应用ID
    /// </summary>
    public string? ApplicationId { get; set; }

    /// <summary>
    /// 资源Key
    /// </summary>
    public string ResourceKey { get; set; } = null!;

    /// <summary>
    /// 数据访问范围
    /// </summary>
    public RoleDataScope DataScope { get; set; }

    /// <summary>
    /// 自定义数据访问范围
    /// </summary>
    public string? DataScopeCustom { get; set; }

    /// <summary>
    /// 自定义数据访问范围
    /// </summary>
    public IList<string> DataScopeCustoms => string.IsNullOrEmpty(DataScopeCustom)
        ? new List<string>()
        : DataScopeCustom.Split(',').ToList();

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// 策略的资源Key
    /// </summary>
    public string PolicyResourceKey { get; set; } = null!;

    /// <summary>
    /// 策略
    /// </summary>
    [JsonIgnore]
    public string? Policy { get; set; }

    private IList<QueryFilterGroup>? _policies;

    /// <summary>
    /// 查询过滤组
    /// </summary>
    public IList<QueryFilterGroup>? QueryFilterGroups
    {
        get
        {
            if (_policies == null && !string.IsNullOrEmpty(Policy))
            {
                return JsonSerializer.Deserialize<IList<QueryFilterGroup>>(Policy);
            }

            return _policies;
        }
        set => _policies = value;
    }
}