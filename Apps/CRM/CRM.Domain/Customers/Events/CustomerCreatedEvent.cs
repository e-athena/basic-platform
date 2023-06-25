namespace CRM.Domain.Customers.Events;

/// <summary>
/// 客户创建成功事件
/// </summary>
public class CustomerCreatedEvent : EventBase
{
    /// <summary>
    /// 名称
    /// </summary>
    [MaxLength(64)]
    public string Name { get; set; } 

    /// <summary>
    /// 手机号
    /// </summary>
    [MaxLength(11)]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// 座机号
    /// </summary>
    [MaxLength(16)]
    public string? Telephone { get; set; }

    /// <summary>
    /// 行业
    /// </summary>
    public string? Industry { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public string? CreatedUserId { get; set; }

    public CustomerCreatedEvent(string name, string? phoneNumber, string? telephone, string? industry,
        string? createdUserId)
    {
        Name = name;
        PhoneNumber = phoneNumber;
        Telephone = telephone;
        Industry = industry;
        CreatedUserId = createdUserId;
    }
}