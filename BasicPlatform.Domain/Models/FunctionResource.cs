namespace BasicPlatform.Domain.Models;

/// <summary>
/// 功能资源
/// </summary>
[Table("AuthorityFunctionResources")]
public class FunctionResource : ValueObject
{
    /// <summary>
    /// 功能ID
    /// </summary>
    /// <value></value>
    [MaxLength(36)]
    public string FunctionId { get; set; } = null!;

    /// <summary>
    /// 功能
    /// </summary>
    /// <value></value>
    public virtual Function Function { get; set; } = null!;

    /// <summary>
    /// 资源ID
    /// </summary>
    /// <value></value>
    [MaxLength(36)]
    public string ResourceId { get; set; } = null!;

    /// <summary>
    /// 权限
    /// </summary>
    /// <value></value>
    public virtual Resource Resource { get; set; } = null!;


    public FunctionResource()
    {
    }

    public FunctionResource(string functionId, string resourceId)
    {
        ResourceId = resourceId ?? throw new ArgumentNullException(nameof(resourceId));
        FunctionId = functionId ?? throw new ArgumentNullException(nameof(functionId));
    }
}