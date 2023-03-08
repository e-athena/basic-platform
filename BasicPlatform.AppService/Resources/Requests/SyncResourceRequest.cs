namespace BasicPlatform.AppService.Resources.Requests;

/// <summary>
/// 同步资源请求类
/// <remarks>该操作会增量添加资源数据，存在且已分配的数据不会被删除，不存在且分配的的数据将会被删除</remarks>
/// </summary>
public class SyncResourceRequest : ITxRequest<int>
{
    /// <summary>
    /// 资源代码列表
    /// </summary>
    public IList<string> ResourceCodes { get; set; } = null!;
}