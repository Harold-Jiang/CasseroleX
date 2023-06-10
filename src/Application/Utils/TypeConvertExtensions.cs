namespace CasseroleX.Application.Utils;

public static class TypeConvertExtensions
{
    /// <summary>
    /// 转Int
    /// </summary> 

    public static int ToInt(this object? obj)
    {
        if (obj is null) return 0;
        if (int.TryParse(obj.ToString(), out int value))
        {
            return value;
        }
        return value;
    }
    /// <summary>
    /// 转decimal
    /// </summary> 

    public static decimal ToDecimal(this object? obj)
    {
        if (obj is null) return 0;
        if (decimal.TryParse(obj.ToString(), out decimal value))
        {
            return value;
        }
        return value;
    }


    public static bool ToBool(this object obj)
    {
        if (bool.TryParse(obj.ToString(), out bool value))
        {
            return value;
        }
        return value;
    }
}
