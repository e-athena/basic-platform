namespace BasicPlatform.AppService.Resources.Requests;

/// <summary>
/// 重新初始化资源
/// <remarks>该操作会重置所有资源数据，所有被分配的资源也将会被清空</remarks>
/// </summary>
public class ReinitializeResourceRequest : ITxRequest<int>
{
    /// <summary>
    /// 资源代码列表
    /// </summary>
    public IList<string> ResourceCodes { get; set; } = null!;
}