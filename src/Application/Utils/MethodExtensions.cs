using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CasseroleX.Application.Utils;

/// <summary>
/// 属性的扩展方法 
/// </summary>
public static class MethodExtensions
{
    /// <summary>
    /// 是否非空白字符
    /// </summary>
    public static bool IsNotEmpty(this string? @this)
    {
        return @this != "";
    }
    /// <summary>
    /// 是否非NULL或空白字符
    /// </summary>
    public static bool IsNotNullOrEmpty([NotNullWhen(true)] this string? @this)
    {
        return !string.IsNullOrEmpty(@this);
    }
    /// <summary>
    /// 是否非NULL或空白字符
    /// </summary>
    public static bool IsNotNullOrWhiteSpace([NotNullWhen(true)] this string? value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }
    /// <summary>
    /// List是否是空或者无数据
    /// </summary>
    public static bool IsNotNullOrZero<TSource>([NotNullWhen(true)] this IEnumerable<TSource>? source)
    {
        return source != null && source.Any();
    }

    /// <summary>
    /// 是否包含字符串
    /// </summary>
    public static bool Contains(this string @this, string value)
    {
        return @this.IndexOf(value) != -1;
    }
    /// <summary>
    /// 是否包含特性
    /// </summary>
    public static bool Contains(this string @this, string value, StringComparison comparisonType)
    {
        return @this.IndexOf(value, comparisonType) != -1;
    }
    /// <summary>
    /// 是否相似
    /// </summary>
    public static bool IsLike(this string @this, string pattern)
    {
        string regexPattern = "^" + Regex.Escape(pattern) + "$";
        regexPattern = regexPattern.Replace(@"\[!", "[^")
            .Replace(@"\[", "[")
            .Replace(@"\]", "]")
            .Replace(@"\?", ".")
            .Replace(@"\*", ".*")
            .Replace(@"\#", @"\d");
        return Regex.IsMatch(@this, regexPattern);
    }
    /// <summary>
    /// 是否整数
    /// </summary>
    public static bool IsNumeric(this string @this)
    {
        return !Regex.IsMatch(@this, "[^0-9]");
    }
    /// <summary>
    /// 是否日期
    /// </summary>
    public static bool IsValidDateTime(this object @this)
    {
        if (@this == null)
        {
            return true;
        }

        return DateTime.TryParse(@this.ToString(), out _);
    }
    /// <summary>
    /// 是否Decimal
    /// </summary>
    public static bool IsValidDecimal(this object @this)
    {
        if (@this == null)
        {
            return true;
        }

        return decimal.TryParse(@this.ToString(), out _);
    }
    /// <summary>
    /// 是否16位整数
    /// </summary>
    public static bool IsValidInt16(this object @this)
    {
        if (@this == null)
        {
            return true;
        }

        return short.TryParse(@this.ToString(), out _);
    }
    /// <summary>
    /// 是否32位整数
    /// </summary>
    public static bool IsValidInt32(this object @this)
    {
        if (@this == null)
        {
            return true;
        }

        return int.TryParse(@this.ToString(), out _);
    }
    /// <summary>
    /// 是否64位整数
    /// </summary>
    public static bool IsValidInt64(this object @this)
    {
        if (@this == null)
        {
            return true;
        }

        return long.TryParse(@this.ToString(), out _);
    }

    /// <summary>
    /// 是否不是数字
    /// </summary>
    public static Boolean IsNaN(this Double d)
    {
        return Double.IsNaN(d);
    }
    /// <summary>
    /// 转换成金额
    /// </summary>
    public static Decimal ToMoney(this Decimal @this)
    {
        return Math.Round(@this, 2);
    }

    /// <summary>
    /// 返回规范化属性名称
    /// </summary>
    public static string? GetPropertyName<T>(this string? @this)
    {
        if (string.IsNullOrWhiteSpace(@this))
        {
            return null;
        }
        var propertyInfo = typeof(T).GetProperty(@this.Trim(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        // 如果T中找到对应的属性
        if (propertyInfo != null)
        {
            return propertyInfo.Name;
        }
        return null;
    }


    /// <summary>
    /// 返回排序字段名称逗号连接
    /// </summary>
    public static string CreateSort<T>(this string? @this, string? orderby)
    {
        if (string.IsNullOrWhiteSpace(@this))
        {
            return "-Id";
        }
        var sort = string.Empty;
        //逗号来分隔字段字符串
        var fieldsAfterSplit = @this.Split(',');
        foreach (var field in fieldsAfterSplit)
        {
            // 获得属性名称字符串
            var propertyName = field.Trim();
            var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            // 如果T中没找到对应的属性
            if (propertyInfo == null)
            {
                continue;
            }

            sort += orderby == "desc" ? $"-{propertyInfo.Name}," : $"{propertyInfo.Name},";
        }
        return sort.IsNotNullOrEmpty() ? sort.TrimEnd(',') : "-Id";
    }

    /// <summary>
    /// Convert string to enumeration
    /// </summary>
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
    public static List<T> ToIList<T>(this string? @this,string delimiters = ",") 
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
    /// Dynamically setting object properties and values
    /// </summary>
    /// <param name="this">object</param>
    /// <param name="propertyName">name</param>
    /// <param name="propertyValue">value</param>
    /// <returns></returns>
    public static T? SetPropertyValue<T>(this T @this, string propertyName, object? propertyValue)
    {
        var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        if (propertyInfo != null)
        {
            //获取item.Name的真实类型
            Type realType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;  //返回指定可以为 null 的类型的基础类型参数
            if (propertyValue != null)
            {
                propertyValue = Convert.ChangeType(propertyValue, realType);
                propertyInfo.SetValue(@this, propertyValue);
                return @this;
            }
            else
            {
                propertyInfo.SetValue(@this, null);
                return @this;
            }
        }
        return default;

    }

    /// <summary>
    /// Object to Dictionary
    /// </summary>
    /// <param name="obj">Object</param>
    /// <param name="tolower">Is the attribute name lowercase</param>
    /// <returns></returns>
    public static Dictionary<string, object>? ObjectToDictionary(this object? obj,bool tolower = false)
    {
        if (obj is null) return null;
        Dictionary<string, object> dict = new();
        Type type = obj.GetType();
        PropertyInfo[] properties = type.GetProperties();
        foreach (PropertyInfo property in properties)
        {
            var value = property.GetValue(obj, null);
            if (value is not  null )
                dict.Add(tolower ? property.Name.ToLowerInvariant():property.Name, value);
        }
        return dict;
    }

}