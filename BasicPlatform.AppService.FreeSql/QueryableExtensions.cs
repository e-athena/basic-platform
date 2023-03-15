using System.Linq.Expressions;
using Athena.Infrastructure.Messaging.Requests;

namespace BasicPlatform.AppService.FreeSql;

public static class QueryableExtensions
{
    /// <summary>读取列表</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task<Paging<T>> ToPagingAsync<T>(
        this ISelect<T> query,
        GetPagingRequestBase request,
        bool isCount,
        CancellationToken cancellationToken = default)
        where T : class
    {
        return query.ToPagingAsync(request, cancellationToken);
    }

    /// <summary>
    /// 兼容IQueryable的Select方法
    /// </summary>
    /// <param name="query"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    public static ISelect<TDto> Select<TSource, TDto>(this ISelect<TSource> query,
        Expression<Func<TSource, TDto>> selector)
    {
        return query.WithTempQuery(selector.AutomaticConverter());
    }

    /// <summary>
    /// 缓存表达式树
    /// </summary>
    private static readonly Dictionary<string, object> ExpressionCacheDict = new();

    /// <summary>
    /// 自动转换同名的属性类型相同的属性
    /// </summary>
    /// <param name="expression"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    private static Expression<Func<TSource, TDto>> AutomaticConverter<TSource, TDto>(
        this Expression<Func<TSource, TDto>> expression)
    {
        var parameter = expression.Parameters.First();
        var key = $"Key_{typeof(TSource).FullName}_To_{typeof(TDto).FullName}";
        if (ExpressionCacheDict.ContainsKey(key))
        {
            return (Expression<Func<TSource, TDto>>) ExpressionCacheDict[key];
        }
        var response = Expression.New(typeof(TDto));
        var bindings = typeof(TDto)
            .GetProperties()
            .Select(property =>
            {
                var sourceProperty = typeof(TSource).GetProperty(property.Name);
                if (sourceProperty == null || sourceProperty.PropertyType != property.PropertyType)
                {
                    // 如果找不到同名属性，则在传入的表达式中查找
                    var memberExpression = expression.Body as MemberInitExpression;
                    var member = memberExpression?
                        .Bindings
                        .FirstOrDefault(b => b.Member.Name == property.Name);
                    if (member is MemberAssignment memberAssignment)
                    {
                        return memberAssignment;
                    }
                }

                if (sourceProperty == null || sourceProperty.PropertyType != property.PropertyType) return null;
                {
                    var memberAssignment =
                        Expression.Bind(property, Expression.PropertyOrField(parameter, sourceProperty.Name));
                    return memberAssignment;
                }
            }).OfType<MemberBinding>();
        // 生成Lambda表达式
        var lambda = Expression.Lambda<Func<TSource, TDto>>(Expression.MemberInit(response, bindings), parameter);
        // 添加缓存
        ExpressionCacheDict[key] = lambda;
        return lambda;
    }
}