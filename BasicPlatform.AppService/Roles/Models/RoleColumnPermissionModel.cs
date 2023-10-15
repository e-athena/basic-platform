namespace BasicPlatform.AppService.Roles.Models;

/// <summary>
/// 角色列权限模型
/// </summary>
public class RoleColumnPermissionModel
{
    /// <summary>
    /// 应用ID
    /// </summary>
    public string? AppId { get; set; }

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
}