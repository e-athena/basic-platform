namespace CRM.Commands.Customers;

/// <summary>
/// 创建客户命令
/// </summary>
public class CreateCustomerCommand : Command
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 手机号
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// 座机号
    /// </summary>
    public string? Telephone { get; set; }

    /// <summary>
    /// 行业
    /// </summary>
    public string? Industry { get; set; }
}