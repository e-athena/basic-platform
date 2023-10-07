namespace BasicPlatform.Domain.Models.Users;

/// <summary>
/// 用户组
/// </summary>
[Table("authority_user_groups")]
// ReSharper disable once ClassNeverInstantiated.Global
public class UserGroup : FullEntityCore
{
  /// <summary>
  /// 名称
  /// </summary>
  /// <value></value>
  [Required]
  [MaxLength(32)]
  public string? Name { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  /// <value></value>
  [MaxLength(1024)]
  public string? Remarks { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  /// <value></value>
  public Status Status { get; set; } = Status.Enabled;

  /// <summary>
  /// 创建人
  /// </summary>
  public virtual User? CreatedUser { get; set; }

  /// <summary>
  /// 最后更新人
  /// </summary>
  public virtual User? LastUpdatedUser { get; set; }

  /// <summary>
  /// 状态变更
  /// </summary>
  /// <param name="updatedUserId"></param>
  public void StatusChange(string updatedUserId)
  {
    Status = Status == Status.Disabled ? Status.Enabled : Status.Disabled;
    LastUpdatedUserId = updatedUserId;
    UpdatedOn = DateTime.Now;
  }
}