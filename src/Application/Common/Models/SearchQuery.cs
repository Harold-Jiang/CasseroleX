using System.Linq.Expressions;
using System.Reflection;
using CasseroleX.Application.Common.Json;
using CasseroleX.Application.Utils;
using MediatR;

namespace CasseroleX.Application.Common.Models;
public class SearchQuery<TRequest, TResponse> : IRequest<PaginatedList<TResponse>> where TRequest :class
{
    public string OP { get; set; } = "{}";

    public string Sort { get; set; } = "-Id";

    public string Order { get; set; } = "DESC";

    public string Filter { get; set; } = "{}";

    public int Limit { get; set; } = 10;

    public int Page { get; set; } = 1;

    public string? Search { get; set; }

    /// <summary>
    /// Building query expressions based on parameters
    /// </summary>
    /// <param name="funcWhere">Extra expression</param>
    /// <returns></returns>
    public Expression<Func<TRequest, bool>> GetQueryLamda(Expression<Func<TRequest, bool>>? funcWhere = null)
    {
        QuerysCollection queries = new();
        var filters = this.GetQueryFilterList();
        if (!filters.IsNotNullOrAny())
            return funcWhere ?? (_ => true);

        foreach (var item in filters)
        {
            switch (item.OP)
            {
                case "=":
                    queries.Add(new Query { Name = item.Name, Operator = Query.Operators.Equal, Value = item.Value });
                    break;
                case "<>":
                    queries.Add(new Query { Name = item.Name, Operator = Query.Operators.NotEqual, Value = item.Value });
                    break;
                case "LIKE":
                    queries.Add(new Query { Name = item.Name, Operator = Query.Operators.StartWith, Value = item.Value });
                    break;
                case "NOT LIKE":
                case "LIKE %...%":
                    queries.Add(new Query { Name = item.Name, Operator = Query.Operators.Like, Value = item.Value });
                    break;
                case "NOT LIKE %...%":
                case ">":
                    queries.Add(new Query { Name = item.Name, Operator = Query.Operators.GreaterThan, Value = item.Value });
                    break;
                case ">=":
                    queries.Add(new Query { Name = item.Name, Operator = Query.Operators.GreaterThanOrEqual, Value = item.Value });
                    break;
                case "<":
                    queries.Add(new Query { Name = item.Name, Operator = Query.Operators.LessThan, Value = item.Value });
                    break;
                case "<=":
                    queries.Add(new Query { Name = item.Name, Operator = Query.Operators.LessThanOrEqual, Value = item.Value });
                    break;
                case "FINDIN":
                case "FINDINSET":
                case "FIND_IN_SET":
                case "IN":
                case "IN(...)":
                    queries.Add(new Query { Name = item.Name, Operator = Query.Operators.In, Value = item.Value });
                    break;
                case "NOT IN":
                case "NOT IN(...)":
                    queries.Add(new Query { Name = item.Name, Operator = Query.Operators.NotIn, Value = item.Value });
                    break;
                case "BETWEEN":
                case "NOT BETWEEN":
                case "RANGE":
                    var v = item.Value.Replace("+", " ");
                    var arr = v.Split(" - ");
                    queries.Add(new Query { Name = item.Name, Operator = Query.Operators.Range, ValueMin = arr[0], ValueMax = arr[1] });
                    break;
                case "NOT RANGE":
                case "NULL":
                case "IS NULL":
                case "NOT NULL":
                case "IS NOT NULL":
                default:
                    break;
            }
        }

        var queryWhere = queries.AsExpression<TRequest>();  
        return queryWhere.Compose(funcWhere, Expression.AndAlso) ?? (_ => true);
    }

    private List<QueryFilter> GetQueryFilterList(string? searchFields = null)
    {
        List<QueryFilter> queryFilters = new();

        //quick search
        if (this.Search.IsNotNullOrEmpty() && searchFields.IsNotNullOrEmpty())
        {
            var fields = searchFields.ToIList<string>();
            foreach (var field in fields)
            {
                var propertyName = field.Trim();
                var propertyInfo = typeof(TRequest).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo == null)
                    continue;

                queryFilters.Add(new QueryFilter
                {
                    Name = propertyInfo.Name,
                    Value = this.Search.ReplaceHtmlTag(),
                    OP = "LIKE %...%",
                });
            }
        }
        else //Advanced Search
        {
            var filter = this.Filter.ToObject<Dictionary<string, string>>();
            var op = this.OP.ToObject<Dictionary<string, string>>();

            if (filter != null && op != null)
            {
                foreach (var item in filter)
                {
                    //Detect object attributes in filters
                    if (!item.Key.preg_match(@"^[a-zA-Z0-9_\-\.]+$"))
                        continue;

                    // Obtain the property name string of the query object
                    var propertyInfo = typeof(TRequest).GetProperty(item.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo == null)
                        continue;

                    queryFilters.Add(new QueryFilter
                    {
                        Name = propertyInfo.Name,
                        Value = item.Value,
                        OP = op[item.Key].IsNotNullOrEmpty() ? op[item.Key].ToUpper() : "=",
                    });
                }
            }
        }
        return queryFilters; 
    }
}
