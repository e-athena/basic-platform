using Athena.Infrastructure.ColumnPermissions.Models;

namespace BasicPlatform.Domain.Models.Users;

/// <summary>
/// 用户列权限
/// </summary>
[Table("authority_user_column_permissions")]
public class UserColumnPermission : ValueObject
{
    /// <summary>
    /// 应用ID
    /// </summary>
    [MaxLength(36)]
    public string? AppId { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    /// <value></value>
    [MaxLength(36)]
    public string UserId { get; set; } = null!;

    /// <summary>
    /// 用户
    /// </summary>
    /// <value></value>
    public virtual User User { get; set; } = null!;

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
    public int MaskLength { get; set; }

    /// <summary>
    /// 掩码位置，1、前面，2、中间，3、后面
    /// </summary>
    public MaskPosition MaskPosition { get; set; } = MaskPosition.Middle;

    /// <summary>
    /// 掩码字符
    /// </summary>
    public string MaskChar { get; set; } = "*";

    /// <summary>
    /// 有效期至
    /// <remarks>为空时永久有效</remarks>
    /// </summary>
    public DateTime? ExpireAt { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public UserColumnPermission()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="appId"></param>
    /// <param name="userId"></param>
    /// <param name="enabled"></param>
    /// <param name="columnType"></param>
    /// <param name="columnKey"></param>
    /// <param name="isEnableDataMask"></param>
    /// <param name="dataMaskType"></param>
    /// <param name="maskLength"></param>
    /// <param name="maskPosition"></param>
    /// <param name="maskChar"></param>
    /// <param name="expireAt"></param>
    public UserColumnPermission(string? appId, string userId, bool enabled, string columnType, string columnKey,
        bool isEnableDataMask, int dataMaskType, int maskLength, MaskPosition maskPosition, string maskChar,
        DateTime? expireAt)
    {
        AppId = appId;
        UserId = userId;
        Enabled = enabled;
        ColumnType = columnType;
        ColumnKey = columnKey;
        IsEnableDataMask = isEnableDataMask;
        DataMaskType = dataMaskType;
        MaskLength = maskLength;
        MaskPosition = maskPosition;
        MaskChar = maskChar;
        ExpireAt = expireAt;
    }
}