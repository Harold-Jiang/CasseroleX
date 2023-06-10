using System.Text;

namespace CasseroleX.Application.Utils;
/// <summary>
/// 日期时间格式化
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// 获取格式化字符串，带时分秒，格式："yyyy-MM-dd HH:mm:ss"
    /// </summary>
    /// <param name="dateTime">日期</param>
    /// <param name="removeSecond">是否移除秒</param>
    public static string ToDateTimeString(this DateTime dateTime, bool removeSecond = false)
    {
        return removeSecond ? dateTime.ToString("yyyy-MM-dd HH:mm") : dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }

    /// <summary>
    /// 获取格式化字符串，带时分秒，格式："yyyy-MM-dd HH:mm:ss"
    /// </summary>
    /// <param name="dateTime">日期</param>
    /// <param name="removeSecond">是否移除秒</param>
    public static string ToDateTimeString(this DateTime? dateTime, bool removeSecond = false)
    {
        return dateTime == null ? string.Empty : ToDateTimeString(dateTime.Value, removeSecond);
    }

    /// <summary>
    /// 获取格式化字符串，不带时分秒，格式："yyyy-MM-dd"
    /// </summary>
    /// <param name="dateTime">日期</param>
    public static string ToDateString(this DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd");
    }

    /// <summary>
    /// 获取格式化字符串，不带时分秒，格式："yyyy-MM-dd"
    /// </summary>
    /// <param name="dateTime">日期</param>
    public static string ToDateString(this DateTime? dateTime)
    {
        return dateTime == null ? string.Empty : ToDateString(dateTime.Value);
    }

    /// <summary>
    /// 获取格式化字符串，不带年月日，格式："HH:mm:ss"
    /// </summary>
    /// <param name="dateTime">日期</param>
    public static string ToTimeString(this DateTime dateTime)
    {
        return dateTime.ToString("HH:mm:ss");
    }

    /// <summary>
    /// 获取格式化字符串，不带年月日，格式："HH:mm:ss"
    /// </summary>
    /// <param name="dateTime">日期</param>
    public static string ToTimeString(this DateTime? dateTime)
    {
        return dateTime == null ? string.Empty : ToTimeString(dateTime.Value);
    }

    /// <summary>
    /// 获取格式化字符串，带毫秒，格式："yyyy-MM-dd HH:mm:ss.fff"
    /// </summary>
    /// <param name="dateTime">日期</param>
    public static string ToMillisecondString(this DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
    }

    /// <summary>
    /// 获取格式化字符串，带毫秒，格式："yyyy-MM-dd HH:mm:ss.fff"
    /// </summary>
    /// <param name="dateTime">日期</param>
    public static string ToMillisecondString(this DateTime? dateTime)
    {
        return dateTime == null ? string.Empty : ToMillisecondString(dateTime.Value);
    }

    /// <summary>
    /// 获取格式化字符串，不带时分秒，格式："yyyy年MM月dd日"
    /// </summary>
    /// <param name="dateTime">日期</param>
    public static string ToChineseDateString(this DateTime dateTime)
    {
        return string.Format("{0}年{1}月{2}日", dateTime.Year, dateTime.Month, dateTime.Day);
    }

    /// <summary>
    /// 获取格式化字符串，不带时分秒，格式："yyyy年MM月dd日"
    /// </summary>
    /// <param name="dateTime">日期</param>
    public static string ToChineseDateString(this DateTime? dateTime)
    {
        return dateTime == null ? string.Empty : ToChineseDateString((DateTime)dateTime);
    }

    /// <summary>
    /// 获取格式化字符串，带时分秒，格式："yyyy年MM月dd日 HH时mm分"
    /// </summary>
    /// <param name="dateTime">日期</param>
    /// <param name="removeSecond">是否移除秒</param>
    public static string ToChineseDateTimeString(this DateTime dateTime, bool removeSecond = false)
    {
        StringBuilder result = new();
        result.AppendFormat("{0}年{1}月{2}日", dateTime.Year, dateTime.Month, dateTime.Day);
        result.AppendFormat(" {0}时{1}分", dateTime.Hour, dateTime.Minute);
        if (removeSecond == false)
            result.AppendFormat("{0}秒", dateTime.Second);
        return result.ToString();
    }

    /// <summary>
    /// 获取格式化字符串，带时分秒，格式："yyyy年MM月dd日 HH时mm分"
    /// </summary>
    /// <param name="dateTime">日期</param>
    /// <param name="removeSecond">是否移除秒</param>
    public static string ToChineseDateTimeString(this DateTime? dateTime, bool removeSecond = false)
    {
        return dateTime == null ? string.Empty : ToChineseDateTimeString(dateTime.Value, removeSecond);
    }

    /// <summary>
    /// 获取描述
    /// </summary>
    /// <param name="span">时间间隔</param>
    public static string Description(this TimeSpan span)
    {
        StringBuilder result = new();
        if (span.Days > 0)
            result.AppendFormat("{0}天", span.Days);
        if (span.Hours > 0)
            result.AppendFormat("{0}小时", span.Hours);
        if (span.Minutes > 0)
            result.AppendFormat("{0}分", span.Minutes);
        if (span.Seconds > 0)
            result.AppendFormat("{0}秒", span.Seconds);
        if (span.Milliseconds > 0)
            result.AppendFormat("{0}毫秒", span.Milliseconds);

        return result.Length > 0 ? result.ToString() : $"{span.TotalSeconds * 1000}毫秒";
    }

    /// <summary>
    /// 格式化日期
    /// </summary>
    public static string ToTimeDifferNow(this DateTime dt)
    {
        TimeSpan span = DateTime.Now - dt;
        if (span.TotalDays >= 365)
        {
            return Math.Floor(span.TotalDays / 365) + "年前";
        }
        else if (span.TotalDays >= 30)
        {
            return Math.Floor(span.TotalDays / 30) + "月前";//估算即可，不用精确
        }
        else if (span.TotalDays >= 21)
        {
            return "3周前";
        }
        else if (span.TotalDays >= 14)
        {
            return "2周前";
        }
        else if (span.TotalDays >= 7)
        {
            return "1周前";
        }
        else if (span.TotalHours >= 24)
        {
            return Math.Floor(span.TotalDays) + "天前";
        }
        else if (span.TotalHours >= 1)
        {
            return Math.Floor(span.TotalHours) + "小时前";
        }
        else
        {
            return span.TotalMinutes >= 1 ? Math.Floor(span.TotalMinutes) + "分钟前" : "刚刚";
        }
    }

    /// <summary>
    /// 本周周一
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="startOfWeek"></param>
    /// <returns></returns>
    public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
    {
        int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
        return dt.AddDays(-1 * diff).Date;
    }

    /// <summary>
    /// 本月第一天
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static DateTime StartOfMonth(this DateTime value)
    {
        return new DateTime(value.Year, value.Month, 1);
    }

    /// <summary>
    /// DateTime转Unix时间戳
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double GetUnixTimeStamp(this DateTime value)
    {
        return value.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
    }
    /// <summary>
    /// Unix时间戳转DateTime
    /// </summary>
    /// <param name="timeStamp"></param>
    /// <returns></returns>
    public static DateTime FromUnixTimeStamp(this string timeStamp)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(timeStamp)).DateTime.ToLocalTime();
    }

    /// <summary>
    /// 字符串转DateTime
    /// </summary>
    public static DateTime ToDateTime(this string? dateString)
    {
        if (!dateString.IsNotNullOrEmpty())
        {
            return DateTime.Now;
        }
        if (dateString.IndexOf("-") > -1)
        {
            if (dateString.IndexOf(":") > -1)
            {
                return DateTime.ParseExact(dateString, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
            }
            if (dateString.Length < 10)
            {
                return DateTime.ParseExact(dateString, "yyyy-M-d",
                System.Globalization.CultureInfo.CurrentCulture);
            }
            return DateTime.ParseExact(dateString, "yyyy-MM-dd",
                System.Globalization.CultureInfo.CurrentCulture);
        }
        else if (dateString.IndexOf("/") > -1)
        {
            if (dateString.IndexOf(":") > -1)
            {
                return DateTime.ParseExact(dateString, "yyyy/MM/dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
            }
            if (dateString.Length < 10)
            {
                return DateTime.ParseExact(dateString, "yyyy/M/d",
                System.Globalization.CultureInfo.CurrentCulture);
            }
            return DateTime.ParseExact(dateString, "yyyy/MM/dd",
                System.Globalization.CultureInfo.CurrentCulture);
        }
        else
        {
            var length = dateString.Length;
            switch (length)
            {
                case 6: //yyyyMM
                    return DateTime.ParseExact(dateString, "yyyyMM", System.Globalization.CultureInfo.CurrentCulture);
                case 8: //yyyyMMdd
                    return DateTime.ParseExact(dateString, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                case 14: //yyyyMMddHHmmss
                    return DateTime.ParseExact(dateString, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
            }
        }
        return DateTime.Now;
    }
}