using CRM.Domain.Customers.Events;

namespace CRM.Domain.Customers;

/// <summary>
/// 客户
/// </summary>
[Table("customers")]
public class Customer : FullEntityCore
{
    /// <summary>
    /// 名称
    /// </summary>
    [MaxLength(64)]
    public string Name { get; set; } = null!;

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

    public Customer()
    {
    }

    public Customer(string name, string? phoneNumber, string? telephone, string? industry, string? createdUserId)
    {
        Name = name;
        PhoneNumber = phoneNumber;
        Telephone = telephone;
        Industry = industry;
        CreatedUserId = createdUserId;

        ApplyEvent(new CustomerCreatedEvent(Name, PhoneNumber, Telephone, Industry, CreatedUserId));
    }
}