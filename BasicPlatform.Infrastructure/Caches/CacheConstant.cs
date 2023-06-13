namespace BasicPlatform.Infrastructure.Caches;

/// <summary>
/// 缓存相关常量
/// </summary>
public static class CacheConstant
{
    #region 用户相关

    /// <summary>
    /// 用户缓存键
    /// </summary>
    public const string UserCacheKeys = "user:{0}:*";

    /// <summary>
    /// 用户数据范围缓存键
    /// </summary>
    public const string UserDataScopesKey = "user:{0}:UserDataScopes";

    /// <summary>
    /// 用户所在的组织架构缓存键
    /// </summary>
    public const string UserOrganizationKey = "user:{0}:UserOrganization";

    /// <summary>
    /// 用户所有的组织架构及下级组织架构缓存键
    /// </summary>
    public const string UserOrganizationsKey = "user:{0}:UserOrganizations";

    /// <summary>
    /// 用户策略过滤组缓存键
    /// </summary>
    public const string UserPolicyFilterGroupQueryKey = "user:{0}:UserPolicyFilterGroupQuery:{1}";

    /// <summary>
    /// 用户策略过滤组缓存键
    /// </summary>
    public const string UserPolicyFilterGroupQueryPatternKey = "user:{0}:UserPolicyFilterGroupQuery:*";

    /// <summary>
    /// 用户策略查询缓存键
    /// </summary>
    public const string UserPolicyQueryKey = "user:{0}:UserPolicyQuery:{1}";

    /// <summary>
    /// 用户策略查询缓存键
    /// </summary>
    public const string UserPolicyQueryPatternKey = "user:{0}:UserPolicyQuery:*";

    #endregion
}