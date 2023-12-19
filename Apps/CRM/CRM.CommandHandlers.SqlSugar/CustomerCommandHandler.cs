namespace CRM.CommandHandlers.SqlSugar;

/// <summary>
/// 客户命令处理器
/// </summary>
public class CustomerCommandHandler : DataPermissionServiceBase<Customer>,
    ICommandHandler<CreateCustomerCommand>
{
    public CustomerCommandHandler(ISqlSugarClient sqlSugarClient, ISecurityContextAccessor accessor) : base(
        sqlSugarClient, accessor)
    {
    }

    /// <summary>
    /// 创建客户
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<string> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = new Customer(
            request.Name,
            request.PhoneNumber,
            request.Telephone,
            request.Industry,
            UserId
        );
        await RegisterNewAsync(entity, cancellationToken);
        return entity.Id;
    }
}