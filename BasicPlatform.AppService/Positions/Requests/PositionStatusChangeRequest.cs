namespace BasicPlatform.AppService.Positions.Requests;

/// <summary>
/// 变更职位状态请求
/// </summary>
public class PositionStatusChangeRequest : ITxRequest<string>
{
    /// <summary>
    /// ID
    /// </summary>
    public string Id { get; set; } = null!;
}