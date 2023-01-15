using BasicPlatform.AppService.Users.Requests;

namespace BasicPlatform.AppService.FreeSql.Users;

/// <summary>
/// 用户请求处理程序
/// </summary>
public class UserRequestHandler : ServiceBase<Domain.Models.User>,
    IRequestHandler<CreateUserRequest, string>,
    IRequestHandler<UserStatusChangeRequest, string>
{
    private readonly ISecurityContextAccessor _contextAccessor;

    public UserRequestHandler(UnitOfWorkManager unitOfWorkManager, ISecurityContextAccessor contextAccessor)
        : base(unitOfWorkManager)
    {
        _contextAccessor = contextAccessor;
    }

    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var exists = await QueryableNoTracking
            .Where(p => p.UserName == request.UserName)
            .AnyAsync(cancellationToken);
        if (exists)
        {
            throw FriendlyException.Of("用户名已存在");
        }

        var entity = new User(
            request.UserName,
            request.Password,
            request.RealName,
            request.PhoneNumber,
            request.Email,
            Status.Enabled,
            _contextAccessor.UserId
        );
        await RegisterNewAsync(entity, cancellationToken);
        return entity.Id;
    }

    /// <summary>
    /// 状态变更
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> Handle(UserStatusChangeRequest request, CancellationToken cancellationToken)
    {
        var entity = await Queryable
            .Where(p => p.Id == request.Id)
            .FirstAsync(cancellationToken);

        if (entity == null)
        {
            throw FriendlyException.Of("用户不存在");
        }

        entity.StatusChange(_contextAccessor.UserId);
        await RegisterDirtyAsync(entity, cancellationToken);
        return entity.Id;
    }
}