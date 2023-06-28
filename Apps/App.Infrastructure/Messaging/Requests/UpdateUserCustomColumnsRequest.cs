namespace App.Infrastructure.Messaging.Requests;

public class UpdateUserCustomColumnsRequest
{
    /// <summary>
    /// 应用ID
    /// </summary>
    public string? AppId { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    public string UserId { get; set; } = null!;
    
    /// <summary>
    /// 所属模块
    /// </summary>
    public string ModuleName { get; set; } = null!;

    /// <summary>
    /// 表格列列表
    /// </summary>
    public IList<UserCustomColumnModel> Columns { get; set; } = null!;
}