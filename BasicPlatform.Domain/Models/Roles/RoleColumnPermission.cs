using Athena.Infrastructure.ColumnPermissions.Models;

namespace BasicPlatform.Domain.Models.Roles;

/// <summary>
/// 角色列权限
/// </summary>
[Table("authority_role_column_permissions")]
public class RoleColumnPermission : ValueObject
{
    /// <summary>
    /// 应用ID
    /// </summary>
    [MaxLength(36)]
    public string? AppId { get; set; }

    /// <summary>
    /// 角色ID
    /// </summary>
    /// <value></value>
    [MaxLength(36)]
    public string RoleId { get; set; } = null!;

    /// <summary>
    /// 角色
    /// </summary>
    /// <value></value>
    [Newtonsoft.Json.JsonIgnore]
    [JsonIgnore]
    public virtual Role Role { get; set; } = null!;

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// 列类型
    /// </summary>
    public string ColumnType { get; set; } = null!;

    /// <summary>
    /// 列Key
    /// </summary>
    public string ColumnKey { get; set; } = null!;

    /// <summary>
    /// 启用数据脱敏
    /// </summary>
    public bool IsEnableDataMask { get; set; }

    /// <summary>
    /// 数据脱敏类型
    /// </summary>
    public int DataMaskType { get; set; } = 99;

    /// <summary>
    /// 掩码长度
    /// </summary>
    public int MaskLength { get; set; } = 4;

    /// <summary>
    /// 掩码位置，1、前面，2、中间，3、后面
    /// </summary>
    public MaskPosition MaskPosition { get; set; } = MaskPosition.Middle;

    /// <summary>
    /// 掩码字符
    /// </summary>
    public string MaskChar { get; set; } = "*";

    /// <summary>
    /// 
    /// </summary>
    public RoleColumnPermission()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="appId"></param>
    /// <param name="roleId"></param>
    /// <param name="enabled"></param>
    /// <param name="columnType"></param>
    /// <param name="columnKey"></param>
    /// <param name="isEnableDataMask"></param>
    /// <param name="dataMaskType"></param>
    /// <param name="maskLength"></param>
    /// <param name="maskPosition"></param>
    /// <param name="maskChar"></param>
    public RoleColumnPermission(string? appId, string roleId, bool enabled, string columnType, string columnKey,
        bool isEnableDataMask,
        int dataMaskType, int maskLength, MaskPosition maskPosition, string maskChar)
    {
        AppId = appId;
        RoleId = roleId;
        Enabled = enabled;
        ColumnType = columnType;
        ColumnKey = columnKey;
        IsEnableDataMask = isEnableDataMask;
        DataMaskType = dataMaskType;
        MaskLength = maskLength;
        MaskPosition = maskPosition;
        MaskChar = maskChar;
    }
}