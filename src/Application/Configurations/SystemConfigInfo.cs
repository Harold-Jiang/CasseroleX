namespace CasseroleX.Application.Configurations;


public partial class SystemConfigInfo : IConfig
{ 
    public string Name { get; set; } = "CasseroleX"; 

    public string? ForbiddeIp { get; set; }

    public string Version { get; set; } = "1.0.0";

    public string PUBLIC { get; set; } = ""; 

    public string CDN { get; set; } = "";

    public string Prefix { get; set; } = "";
      
    public string? LoginBackground { get; set; }

    public string Language { get; set; } = "en";

    public string? Fixedpage { get; set; }

    public string? Timezone { get; set; }

    
}
  
/// <summary>
/// Data types for system configuration
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
/// System Configuration Matching Rule List
/// </summary>
public class SysConfigRegexList
{
    public static string Required => "Required";
    public static string Digits => "Numbers";
    public static string Letters => "Letters";
    public static string Date => "Date";
    public static string Time => "Time";
    public static string Email => "Email";
    public static string Url => "URL";
    public static string QQ => "QQ number";
    public static string IDcard => "ID card";
    public static string Tel => "landline phone";
    public static string Mobile => "Mobile number";
    public static string Zipcode => "Zipcode";
    public static string Chinese => "Chinese";
    public static string Username => "User name";
    public static string Password => "Password";
}
