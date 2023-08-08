using System.Linq.Expressions;

namespace CasseroleX.Application.Utils;
public static class ExpressionExtension
{
    /// <summary>
    /// Use the specified merge function to merge the first and second expressions
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <param name="merge"></param>
    /// <returns></returns>
    public static Expression<T>? Compose<T>(this Expression<T>? first, Expression<T>? second, Func<Expression, Expression, Expression> merge)
    {
        if (first is null && second is null)
            return null;

        if (first is null)
            return second;

        if (second is null)
            return first;

        //Compression parameter (mapping from the second parameter to the first parameter).   
        var map = first.Parameters
            .Select((f, i) => new { f, s = second.Parameters[i] })
            .ToDictionary(p => p.s, p => p.f);

        // Replace the parameter in the second lambda expression with the first parameter  
        var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

        // Create a merged lambda expression using the parameters of the first expression   
        return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
    }
}
