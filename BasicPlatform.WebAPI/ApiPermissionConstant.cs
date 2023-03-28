namespace BasicPlatform.WebAPI;

/// <summary>
/// 接口权限常量
/// </summary>
public static class ApiPermissionConstant
{
    #region 角色

    /// <summary>
    /// 角色下拉列表
    /// </summary>
    public const string RoleSelectList = "role:select";

    /// <summary>
    /// 角色下拉列表
    /// </summary>
    public const string RoleDetail = "role:detail";
    /// <summary>
    /// 角色数据权限
    /// </summary>
    public const string RoleDataPermissions = "role:dataPermissions";

    #endregion

    #region 用户

    /// <summary>
    /// 用户详情
    /// </summary>
    public const string UserDetail = "user:detail";

    /// <summary>
    /// 用户资源编码信息
    /// </summary>
    public const string UserResourceCodeInfo = "user:resourceCodeInfo";

    /// <summary>
    /// 用户下拉列表
    /// </summary>
    public const string UserSelectList = "user:select";

    /// <summary>
    /// 用户数据权限
    /// </summary>
    public const string UserDataPermissions = "user:dataPermissions";

    #endregion

    #region 外部页面

    /// <summary>
    /// 外部页面详情
    /// </summary>
    public const string ExternalPageDetail = "externalPage:detail";

    /// <summary>
    /// 外部页面下拉列表
    /// </summary>
    public const string ExternalPageSelectList = "externalPage:select";

    /// <summary>
    /// 外部页面树形列表
    /// </summary>
    public const string ExternalPageTreeList = "externalPage:tree";

    #endregion

    #region 组织架构

    /// <summary>
    /// 组织详情
    /// </summary>
    public const string OrgDetail = "org:detail";

    /// <summary>
    /// 组织架构树形列表
    /// </summary>
    public const string OrgTreeList = "org:tree";

    /// <summary>
    /// 组织架构级联列表
    /// </summary>
    public const string OrgCascaderList = "org:cascader";

    /// <summary>
    /// 组织架构树形下拉列表
    /// </summary>
    public const string OrgTreeSelectList = "org:treeSelect";

    /// <summary>
    /// 组织/部门下拉列表
    /// </summary>
    public const string OrgSelectList = "org:select";

    #endregion

    #region 职位

    /// <summary>
    /// 职位下拉列表
    /// </summary>
    public const string PositionSelectList = "position:select";

    /// <summary>
    /// 职位下拉列表
    /// </summary>
    public const string PositionDetail = "position:detail";

    #endregion
}