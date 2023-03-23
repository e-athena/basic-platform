namespace BasicPlatform.AppService.Roles.Models;

/// <summary>
/// 角色模型
/// </summary>
public class RoleModel : ModelBase
{
    #region 字段

    private string? _dataScopeCustom;
    private IList<string>? _dataScopeCustomList;

    #endregion

    /// <summary>
    /// 名称
    /// </summary>
    [Required]
    [MaxLength(32)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 数据访问范围
    /// </summary>
    public RoleDataScope DataScope { get; set; }

    /// <summary>
    /// 自定义数据访问范围(组织Id)
    /// <remarks>多个组织使用逗号分割</remarks>
    /// </summary>
    [MaxLength(-1)]
    public string? DataScopeCustom
    {
        get
        {
            if (string.IsNullOrEmpty(_dataScopeCustom) && _dataScopeCustomList != null)
            {
                return string.Join(',', _dataScopeCustomList);
            }

            return _dataScopeCustom;
        }
        set => _dataScopeCustom = value;
    }

    /// <summary>
    /// 自定义数据访问范围
    /// </summary>
    public IList<string>? DataScopeCustomList
    {
        get
        {
            if (_dataScopeCustomList == null && !string.IsNullOrWhiteSpace(_dataScopeCustom))
            {
                return _dataScopeCustom.Split(',').ToList();
            }

            return _dataScopeCustomList;
        }
        set => _dataScopeCustomList = value;
    }

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(1024)]
    public string? Remarks { get; set; }
}