using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using CasseroleX.Application.Common.Models;
using CasseroleX.Domain.Enums;

namespace CasseroleX.Application.Utils;

public class QuerysCollection : Collection<Query>
{
    public Expression<Func<T, bool>>? AsExpression<T>(Query.Condition? condition = Query.Condition.AndAlso) where T : class
    {
        Type targetType = typeof(T);
        TypeInfo typeInfo = targetType.GetTypeInfo();
        var parameter = Expression.Parameter(targetType, "x");
        Expression? expression = null;
        Expression Append(Expression? exp1, Expression exp2)
        {
            if (exp1 == null)
            {
                return exp2;
            }
            return (condition ?? Query.Condition.OrElse) == Query.Condition.OrElse ? Expression.OrElse(exp1, exp2) : Expression.AndAlso(exp1, exp2);
        }
        foreach (var item in this) //Qurey
        {
            //Obtain the type of the Name property
            var property = typeInfo.GetProperty(item.Name);
            if (property == null ||
                !property.CanRead ||
                (item.Operator != Query.Operators.Range && item.Value == null) ||
                (item.Operator == Query.Operators.Range && item.ValueMin == null && item.ValueMax == null))
            {
                continue;
            }

            //Obtain the real type of itemName
            Type realType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;   
            if (item.Value != null && item.Operator != Query.Operators.In)
            { 
                if (realType.Name == "Status" && Enum.TryParse(item.Value.ToString(), out Status enumValue))
                {
                    item.Value = enumValue;
                }
                else
                {
                    item.Value = Convert.ChangeType(item.Value, realType);
                }
                
            }
             
            //Expression<Func<object>> valueLamba = () => item.Value; 
            MemberExpression expressionProperty = Expression.Property(parameter, item.Name);
            ConstantExpression expressionValue = Expression.Constant(item.Value);
            switch (item.Operator)
            {
                case Query.Operators.Equal:
                    {
                        expression = Append(expression, Expression.Equal(expressionProperty,
                            expressionValue));
                        break;
                    }
                case Query.Operators.NotEqual:
                    {
                        expression = Append(expression, Expression.NotEqual(expressionProperty,
                            expressionValue));
                        break;
                    }
                case Query.Operators.GreaterThan:
                    {
                        expression = Append(expression, Expression.GreaterThan(expressionProperty,
                            expressionValue));
                        break;
                    }
                case Query.Operators.GreaterThanOrEqual:
                    {
                        expression = Append(expression, Expression.GreaterThanOrEqual(expressionProperty,
                            expressionValue));
                        break;
                    }
                case Query.Operators.LessThan:
                    {
                        expression = Append(expression, Expression.LessThan(expressionProperty,
                            expressionValue));
                        break;
                    }
                case Query.Operators.LessThanOrEqual:
                    {
                        expression = Append(expression, Expression.LessThanOrEqual(expressionProperty,
                            expressionValue));
                        break;
                    }
                case Query.Operators.Like:
                    {
                        //Determine whether the attribute value is a String 
                        if (item.Value is string && !string.IsNullOrWhiteSpace(item.Value.ToString()))
                        {
                            MethodCallExpression containsCall = Expression.Call(expressionProperty, "Contains", null,
                           expressionValue);
                            expression = Append(expression, containsCall);
                        }
                        break;
                    }
                case Query.Operators.StartWith:
                    {
                        if (item.Value is string && !string.IsNullOrWhiteSpace(item.Value.ToString()))
                        {
                            MethodCallExpression startsWith = Expression.Call(expressionProperty, "StartsWith", null,
                           expressionValue);
                            expression = Append(expression, startsWith);
                        }
                        break;
                    }
                case Query.Operators.EndWidth:
                    {
                        if (item.Value is string && !string.IsNullOrWhiteSpace(item.Value.ToString()))
                        {
                            var endsWith = Expression.Call(expressionProperty, "EndsWith", null,
                            expressionValue);
                            expression = Append(expression, endsWith);
                        }
                        break;
                    }
                case Query.Operators.Range:
                    {
                        Expression? minExp = null, maxExp = null;
                        if (item.ValueMin != null)
                        {
                            var minValue = Convert.ChangeType(item.ValueMin, realType);
                            // DateTime =>  DateTime? 
                            var nullableProperty = Expression.Convert(expressionProperty, realType);

                            var constant = Expression.Constant(minValue, realType);
                            minExp = Expression.GreaterThanOrEqual(nullableProperty, constant);
                        }
                        if (item.ValueMax != null)
                        {
                            var maxValue = Convert.ChangeType(item.ValueMax, realType);
                            var nullableProperty = Expression.Convert(expressionProperty, realType);
                            var constant = Expression.Constant(maxValue, realType);
                            maxExp = Expression.LessThanOrEqual(nullableProperty, constant);
                        }
                        if (minExp != null && maxExp != null)
                        {
                            expression = Append(expression, Expression.AndAlso(minExp, maxExp));
                        }
                        else if (minExp != null)
                        {
                            expression = Append(expression, minExp);
                        }
                        else if (maxExp != null)
                        {
                            expression = Append(expression, maxExp);
                        }

                        break;
                    }
                case Query.Operators.In:
                    {
                        if (item.Value is not null)
                        {
                            ConstantExpression? list = null;
                            if (realType == typeof(int))
                            {
                                list = Expression.Constant(item.Value.ToString().ToIList<int>());
                            }
                            else if (realType == typeof(string))
                            {
                                list = Expression.Constant(item.Value.ToString().ToIList<string>());
                            }
                            if (list is not null)
                            {
                                var inExp = Expression.Call(list, "Contains", null, expressionProperty);
                                expression = Append(expression, inExp);
                            }
                        }
                        break;
                    }
                case Query.Operators.NotIn:
                    { 
                        if (item.Value is not null)
                        {
                            ConstantExpression? list = null;
                            if (realType == typeof(int))
                            {
                                list = Expression.Constant(item.Value.ToString().ToIList<int>());
                            }
                            else if (realType == typeof(string))
                            {
                                list = Expression.Constant(item.Value.ToString().ToIList<string>());
                            }
                            if (list is not null)
                            {
                                // 定义列表不包含查询条件 
                                var exp = Expression.Call(list, "Contains", null, expressionProperty);
                                var notInExp = Expression.Not(exp);
                                expression = Append(expression, notInExp);
                            }
                        }
                        break;
                    }
            }
        }

        return expression is null ? null : ((Expression<Func<T, bool>>)Expression.Lambda(expression, parameter)); 
    }
}


