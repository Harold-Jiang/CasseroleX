namespace CasseroleX.Application.Configurations;


public partial class SystemConfigInfo : IConfig
{
    /// <summary>
    /// 应用名称
    /// </summary>
    public string AppName { get; set; } = "Jfmall";
    /// <summary>
    /// 备案号
    /// </summary>
    public string? BeiAn { get; set; }


    /// <summary>
    /// 禁止访问的IP
    /// </summary>
    public string? ForbiddeIp { get; set; }

}

/// <summary>
/// 系统配置的分组
/// </summary>
public class SysConfigGroup
{
    public static string Basic => "基础配置";
    public static string Email => "邮件配置";
    public static string Sms => "短信配置";
    public static string Upload => "上传配置";
    public static string Account => "账户配置";
};


/// <summary>
/// 系统配置的数据类型
/// </summary>
public class SysConfigDataType
{
    public static string String => "string";
    public static string Password => "password";
    public static string Text => "text";
    public static string Editor => "editor";
    public static string Number => "number";
    public static string Date => "date";
    public static string Time => "time";
    public static string Datetime => "datetime";
    public static string Datetimerange => "datetimerange";
    public static string Select => "select";
    public static string Selects => "selects";
    public static string Image => "image";
    public static string Images => "images";
    public static string File => "file";
    public static string Files => "files";
    public static string Switch => "switch";
    public static string Checkbox => "checkbox";
    public static string Radio => "radio";
    public static string City => "city";
    public static string Array => "array";
}


/// <summary>
/// 系统配置匹配规则列表
/// </summary>
public class SysConfigRegexList
{
    public static string Required => "必选";
    public static string Digits => "数字";
    public static string Letters => "字母";
    public static string Date => "日期";
    public static string Time => "时间";
    public static string Email => "邮箱";
    public static string Url => "网址";
    public static string Qq => "QQ号";
    public static string IDcard => "身份证";
    public static string Tel => "座机电话";
    public static string Mobile => "手机号";
    public static string Zipcode => "邮编";
    public static string Chinese => "中文";
    public static string Username => "用户名";
    public static string Password => "密码";
}
