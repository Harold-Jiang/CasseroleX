namespace CasseroleX.Application.Common.Models;
public class Query
{
    public enum Operators
    {
        ///<summary>
        ///None
        ///</summary>
        None = 0,
        ///<summary>
        ///Equal
        ///</summary>
        Equal = 1,
        ///<summary>
        ///Unequal
        ///</summary>
        NotEqual = 2,
        ///<summary>
        ///Greater than ">"
        ///</summary>
        GreaterThan = 3,
        ///<summary>
        ///Greater than or equal to ">="
        ///</summary>
        GreaterThanOrEqual = 4,
        ///<summary>
        ///Less than
        ///</summary>
        LessThan = 5,
        ///<summary>
        ///Less than or equal to
        ///</summary>
        LessThanOrEqual = 6,
        ///<summary>
        ///Contains' like '% xxx%'
        ///</summary>
        Like = 7,
        ///<summary>
        ///Starting from 'like'% xxx '
        ///</summary>
        StartWith = 8,
        ///<summary>
        ///Ends at 'like' xxx% '
        ///</summary>
        EndWidth = 9,
        ///<summary>
        ///Range 'xxxx xxxx'
        ///</summary>
        Range = 10,
        ///<summary>
        ///Including in (x, x, x)
        ///</summary>
        In = 11,
        ///<summary>
        ///Not included in (x, x, x)
        ///</summary>
        NotIn = 12,
    }
    public enum Condition
    { 
        OrElse = 1, 
        AndAlso = 2
    }
    public string Name { get; set; } = null!;
    public Operators Operator { get; set; }
    public object? Value { get; set; }
    public object? ValueMin { get; set; }
    public object? ValueMax { get; set; }
}

public class QueryFilter
{
    public required string Name { get; set; }
    public required string Value { get; set; }
    public required string OP { get; set; } 
}
