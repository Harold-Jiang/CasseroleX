using System.ComponentModel;
using System.Reflection;

namespace CasseroleX.Application.Utils;

public static class TypeConvertExtensions
{ 
    public static int ToInt(this object? obj)
    {
        if (obj is null) return 0;
        if (int.TryParse(obj.ToString(), out int value))
        {
            return value;
        }
        return value;
    }
    
    public static decimal ToDecimal(this object? obj)
    {
        if (obj is null) return 0;
        if (decimal.TryParse(obj.ToString(), out decimal value))
        {
            return Math.Round(value, 2);
        }
        return value;
    } 

    public static bool ToBool(this object obj)
    {
        if (bool.TryParse(obj.ToString(), out bool value))
        {
            return value;
        }
        if (obj.ToString() == "1")
        {
            return true;
        }
        return value;
    }

   
    public static T ToEnum<T>(this string @this)
    {
        Type enumType = typeof(T);
        return (T)Enum.Parse(enumType, @this, true);
    }

    /// <summary>
    /// Convert the concatenated string of delimiters into a List
    /// </summary>
    /// <typeparam name="T">String or Int</typeparam>
    /// <param name="this">string</param>
    /// <param name="delimiters">delimiters default ","</param>
    /// <returns></returns>
    public static List<T> ToIList<T>(this string? @this, string delimiters = ",")
    {
        if (!@this.IsNotNullOrWhiteSpace())
        {
            return new List<T>();
        }
        Type targetType = typeof(T);
        var converter = TypeDescriptor.GetConverter(targetType);
        try
        {
            var values = @this!.Split(new[] { delimiters }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => (T)converter.ConvertTo(x.Trim(), targetType)!).ToList();
            return values;
        }
        catch
        {
            return new List<T>();
        }
    }

    /// <summary>
    /// Object to Dictionary
    /// </summary>
    /// <param name="obj">Object</param>
    /// <param name="tolower">Is the attribute name lowercase</param>
    /// <returns></returns>
    public static Dictionary<string, object>? ObjectToDictionary(this object? obj, bool tolower = false)
    {
        if (obj is null) return null;
        Dictionary<string, object> dict = new();
        Type type = obj.GetType();
        PropertyInfo[] properties = type.GetProperties();
        foreach (PropertyInfo property in properties)
        {
            var value = property.GetValue(obj, null);
            if (value is not null)
                dict.Add(tolower ? property.Name.ToLowerInvariant() : property.Name, value);
        }
        return dict;
    }

}
