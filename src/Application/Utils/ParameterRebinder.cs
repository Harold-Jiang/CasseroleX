using System.Linq.Expressions;

namespace CasseroleX.Application.Utils;
public class ParameterRebinder : ExpressionVisitor
{
    readonly Dictionary<ParameterExpression, ParameterExpression> _map;

    ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
    {
        this._map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
    }

    public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
    {
        return new ParameterRebinder(map).Visit(exp);
    }

    protected override Expression VisitParameter(ParameterExpression p)
    {

        if (_map.TryGetValue(p, out ParameterExpression? replacement))
        {
            p = replacement;
        }

        return base.VisitParameter(p);
    }
}