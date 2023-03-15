using System.Linq.Expressions;

namespace BasicPlatform.AppService.FreeSql;

/// <summary>
/// 表达式树扩展类
/// </summary>
public static class ExpressionExtensions
{
    /// <summary>
    /// 创建linq表达示的body部分
    /// </summary>
    public static Expression<Func<TResponse, bool>> GenerateLambda<TResponse>(
        this ParameterExpression parameterExpression,
        Filter filter
    )
    {
        var expression = parameterExpression.GenerateExpression<TResponse>(filter);
        return Expression.Lambda<Func<TResponse, bool>>(expression, parameterExpression);
    }

    /// <summary>
    /// 生成表达式树
    /// </summary>
    public static Expression GenerateExpression<TResponse>(this ParameterExpression param, Filter filter)
    {
        var property = typeof(TResponse).GetProperty(filter.Key);

        if (property == null)
        {
            return default!;
        }

        // 组装左边
        Expression left = Expression.Property(param, property);
        // 组装右边
        Expression right = Expression.Constant(filter.Value);

        var isCommonRight = filter.Operator is ">" or "<" or "==" or "!=" or ">=" or "<=";

        // 如果是通用查询
        if (isCommonRight)
        {
            if (property.PropertyType == typeof(int))
            {
                right = Expression.Constant(int.Parse(filter.Value));
            }
            else if (property.PropertyType == typeof(int?))
            {
                left = Expression.Property(left, "Value");
                right = Expression.Constant(int.Parse(filter.Value) as int?);
            }
            else if (property.PropertyType == typeof(decimal))
            {
                right = Expression.Constant(decimal.Parse(filter.Value));
            }
            else if (property.PropertyType == typeof(decimal?))
            {
                left = Expression.Property(left, "Value");
                right = Expression.Constant(decimal.Parse(filter.Value) as decimal?);
            }
            else if (property.PropertyType == typeof(double))
            {
                right = Expression.Constant(double.Parse(filter.Value));
            }
            else if (property.PropertyType == typeof(double?))
            {
                left = Expression.Property(left, "Value");
                right = Expression.Constant(double.Parse(filter.Value) as double?);
            }
            else if (property.PropertyType == typeof(DateTime))
            {
                right = Expression.Constant(DateTime.Parse(filter.Value));
            }
            else if (property.PropertyType == typeof(DateTime?))
            {
                left = Expression.Property(left, "Value");
                right = Expression.Constant(DateTime.Parse(filter.Value) as DateTime?);
            }
            else if (property.PropertyType == typeof(string))
            {
                right = Expression.Constant(filter.Value);
            }
            else if (property.PropertyType == typeof(bool))
            {
                right = Expression.Constant(filter.Value.Equals("1"));
            }
            else if (property.PropertyType == typeof(bool?))
            {
                left = Expression.Property(left, "Value");
                right = Expression.Constant(filter.Value.Equals("1") as bool?);
            }
            else if (property.PropertyType == typeof(Guid))
            {
                right = Expression.Constant(Guid.Parse(filter.Value));
            }
            else if (property.PropertyType == typeof(Guid?))
            {
                left = Expression.Property(left, "Value");
                right = Expression.Constant(Guid.Parse(filter.Value) as Guid?);
            }
            else if (property.PropertyType.IsEnum)
            {
                right = Expression.Constant(Enum.Parse(property.PropertyType, filter.Value));
            }
            else
            {
                throw new Exception("暂不能解析该Key的类型");
            }
        }

        Expression expression;
        switch (filter.Operator)
        {
            case "<=":
                expression = Expression.LessThanOrEqual(left, right);
                break;

            case "<":
                expression = Expression.LessThan(left, right);
                break;

            case ">":
                expression = Expression.GreaterThan(left, right);
                break;
            case ">=":
                expression = Expression.GreaterThanOrEqual(left, right);
                break;
            case "!=":
                expression = Expression.NotEqual(left, right);
                break;
            case "contains":
                var method0 = typeof(string).GetMethod("Contains", new[] {typeof(string)});
                expression = Expression.Call(left, method0!, Expression.Constant(filter.Value));
                break;
            case "in":
                if (property.PropertyType.IsEnum)
                {
                    Expression? enumExpression = null;
                    foreach (var val1 in filter.Value.Split(','))
                    {
                        var enumRightValue = Expression.Constant(Enum.Parse(property.PropertyType, val1));
                        var rightExpression = Expression.Equal(left, enumRightValue);
                        enumExpression = enumExpression == null
                            ? rightExpression
                            : enumExpression.OrElse(rightExpression);
                    }

                    expression = enumExpression!;
                    break;
                }

                var instance = Expression.Constant(filter.Value.Split(',').ToList());
                var method1 = typeof(List<string>).GetMethod("Contains", new[] {typeof(string)});
                expression = Expression.Call(instance, method1!, left);
                break;
            case "not in":
                // 数组
                var listExpression = Expression.Constant(filter.Value.Split(',').ToList());
                // Contains语句
                var method2 = typeof(List<string>).GetMethod("Contains", new[] {typeof(string)});
                expression = Expression.Not(Expression.Call(listExpression, method2!, left));
                break;
            //交集，使用交集时左值必须时固定的值
            case "intersect": //交集
                if (property != null)
                {
                    throw new Exception("交集模式下，表达式左边不能为变量，请调整数据规则，如:c=>\"A,B,C\" intersect \"B,D\"");
                }

                var rightValue = filter.Value.Split(',').ToList();
                var leftValue = filter.Key.Split(',').ToList();
                var val = rightValue.Intersect(leftValue);

                expression = Expression.Constant(val.Any());
                break;
            // 介于
            case "between":
                if (property.PropertyType != typeof(DateTime) && property.PropertyType != typeof(DateTime?))
                {
                    throw new Exception("[Between]只能用于日期时间字段");
                }

                var values = filter.Value.Split(',');
                if (values.Length != 2)
                {
                    throw new Exception("[Between]值错误，只能接受两个时间，用逗号分割");
                }

                var startTime = Expression.Constant(DateTime.Parse(values[0]));
                var endTime = Expression.Constant(DateTime.Parse(values[1]).AddDays(1));
                if (property.PropertyType == typeof(DateTime?))
                {
                    var startExpression = Expression.GreaterThanOrEqual(Expression.Property(left, "Value"), startTime);
                    var endExpression = Expression.LessThan(Expression.Property(left, "Value"), endTime);
                    expression = startExpression.AndAlso(endExpression);
                }
                else
                {
                    var startExpression = Expression.GreaterThanOrEqual(left, startTime);
                    var endExpression = Expression.LessThan(left, endTime);
                    expression = startExpression.AndAlso(endExpression);
                }

                break;
            default:
                expression = Expression.Equal(left, right);
                break;
        }

        return expression;
    }

    /// <summary>
    /// OrElse
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static Expression OrElse(this Expression left, Expression right)
    {
        return Expression.OrElse(left, right);
    }

    /// <summary>
    /// AndAlso
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static Expression AndAlso(this Expression left, Expression right)
    {
        return Expression.AndAlso(left, right);
    }
}