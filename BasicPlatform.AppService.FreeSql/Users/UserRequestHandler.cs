using Athena.Infrastructure;
using BasicPlatform.AppService.Users.Requests;

namespace BasicPlatform.AppService.FreeSql.Users;

/// <summary>
/// 用户请求处理程序
/// </summary>
public class UserRequestHandler : AppServiceBase<User>,
    IRequestHandler<CreateUserRequest, string>,
    IRequestHandler<UpdateUserRequest, string>,
    IRequestHandler<UserStatusChangeRequest, string>,
    IRequestHandler<AssignUserResourcesRequest, string>,
    IRequestHandler<UpdateUserLoginInfoRequest, bool>,
    IRequestHandler<AddUserAccessRecordRequest, long>,
    IRequestHandler<UpdateUserCustomColumnsRequest, long>,
    IRequestHandler<ResetUserPasswordRequest, string>,
    IRequestHandler<AssignUserDataPermissionsRequest, string>
{
    private readonly ISecurityContextAccessor _contextAccessor;

    public UserRequestHandler(UnitOfWorkManager unitOfWorkManager, ISecurityContextAccessor contextAccessor)
        : base(unitOfWorkManager, contextAccessor)
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
            request.Avatar,
            request.RealName,
            request.Gender,
            request.NickName,
            request.PhoneNumber,
            request.Email,
            request.OrganizationId,
            request.PositionId,
            UserId
        );
        await RegisterNewAsync(entity, cancellationToken);

        // 新增关联数据
        if (request.RoleIds.Count > 0)
        {
            var userRoles = request
                .RoleIds
                .Select(roleId => new RoleUser(roleId, entity.Id))
                .ToList();
            await RegisterNewRangeValueObjectAsync(userRoles, cancellationToken);
        }

        return entity.Id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        if (request.Id == UserId)
        {
            throw FriendlyException.Of("不能修改自己的信息");
        }

        // 查询用户名是否重复
        var exists = await Queryable
            .Where(p => p.Id != request.Id)
            .AnyAsync(p => p.UserName == request.UserName, cancellationToken);
        // 已存在
        if (exists)
        {
            throw FriendlyException.Of("用户名已存在");
        }

        // 读取数据
        var entity = await GetForUpdateAsync(request.Id, cancellationToken);
        // 更新
        entity.Update(
            request.UserName,
            request.Avatar,
            request.RealName,
            request.Gender,
            request.NickName,
            request.PhoneNumber,
            request.Email,
            request.OrganizationId,
            request.PositionId,
            UserId
        );

        #region 用户角色

        // 删除旧数据
        await RegisterDeleteValueObjectAsync<RoleUser>(
            p => p.UserId == entity.Id, cancellationToken
        );
        // 新增关联数据
        if (request.RoleIds.Count > 0)
        {
            var userRoles = request
                .RoleIds
                .Select(roleId => new RoleUser(roleId, entity.Id))
                .ToList();
            await RegisterNewRangeValueObjectAsync(userRoles, cancellationToken);
        }

        #endregion

        // 更新
        await RegisterDirtyAsync(entity, cancellationToken);

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
        if (request.Id == UserId)
        {
            throw FriendlyException.Of("不能修改自己的状态");
        }

        var entity = await GetForUpdateAsync(request.Id, cancellationToken);
        entity.StatusChange(UserId);
        await RegisterDirtyAsync(entity, cancellationToken);
        return entity.Id;
    }

    /// <summary>
    /// 分配资源
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<string> Handle(AssignUserResourcesRequest request, CancellationToken cancellationToken)
    {
        if (request.Id == UserId)
        {
            throw FriendlyException.Of("不能给自己分配资源");
        }

        // 删除旧数据
        await RegisterDeleteValueObjectAsync<UserResource>(
            p => p.UserId == request.Id, cancellationToken
        );
        if (request.Resources.Count <= 0)
        {
            return request.Id;
        }

        // 新增新数据
        var userResources = request
            .Resources
            .Select(p => new UserResource(request.Id, p.Key, p.Code, request.ExpireAt))
            .ToList();
        await RegisterNewRangeValueObjectAsync(userResources, cancellationToken);

        return request.Id;
    }


    /// <summary>
    /// 分配数据权限
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> Handle(AssignUserDataPermissionsRequest request, CancellationToken cancellationToken)
    {
        // 删除旧数据
        await RegisterDeleteValueObjectAsync<UserDataPermission>(
            p => p.UserId == request.Id, cancellationToken
        );
        if (request.Permissions.Count <= 0)
        {
            return request.Id;
        }

        // 新增新数据
        var userDataPermissions = request
            .Permissions
            .Select(p => new UserDataPermission
                (request.Id, p.ResourceKey, p.DataScope, p.DataScopeCustom, p.Enabled, request.ExpireAt))
            .ToList();
        await RegisterNewRangeValueObjectAsync(userDataPermissions, cancellationToken);

        return request.Id;
    }

    /// <summary>
    /// 更新用户登录信息
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> Handle(UpdateUserLoginInfoRequest request, CancellationToken cancellationToken)
    {
        var entity = await GetForUpdateAsync(request.Id, cancellationToken);
        entity.UpdateLoginInfo(_contextAccessor.IpAddress);
        await RegisterDirtyAsync(entity, cancellationToken);
        return true;
    }

    /// <summary>
    /// 添加用户访问记录
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<long> Handle(AddUserAccessRecordRequest request, CancellationToken cancellationToken)
    {
        // 未登录
        if (string.IsNullOrEmpty(UserId) ||
            string.IsNullOrEmpty(request.AccessUrl) ||
            string.IsNullOrEmpty(_contextAccessor.IpAddress))
        {
            return -1;
        }

        var entity = new UserAccessRecord(
            UserId,
            _contextAccessor.IpAddress,
            request.AccessUrl,
            _contextAccessor.UserAgent
        );
        await RegisterNewValueObjectAsync(entity, cancellationToken);
        return entity.Id;
    }

    /// <summary>
    /// 更新用户自定义列
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<long> Handle(UpdateUserCustomColumnsRequest request, CancellationToken cancellationToken)
    {
        // 未登录
        if (string.IsNullOrEmpty(UserId))
        {
            return -1;
        }

        // 删除旧数据
        await RegisterDeleteValueObjectAsync<UserCustomColumn>(
            p => p.UserId == UserId && p.ModuleName == request.ModuleName, cancellationToken
        );

        // 新增新数据
        var userCustomColumns = request
            .Columns
            .Select(p => new UserCustomColumn(
                UserId,
                request.ModuleName,
                p.DataIndex,
                p.Width,
                p.Show,
                p.Fixed,
                p.Sort)
            )
            .ToList();

        var rows = await RegisterNewRangeValueObjectAsync(userCustomColumns, cancellationToken);
        return rows.Count;
    }

    /// <summary>
    /// 重置密码
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> Handle(ResetUserPasswordRequest request, CancellationToken cancellationToken)
    {
        if (UserId == request.Id)
        {
            throw FriendlyException.Of("不能重置自己的密码，请使用修改密码功能");
        }

        var entity = await GetForUpdateAsync(request.Id, cancellationToken);
        var newPassword = StringGenerator.Generate(10);
        entity.ResetPassword(newPassword, UserId);
        await RegisterDirtyAsync(entity, cancellationToken);
        return newPassword;
    }
}