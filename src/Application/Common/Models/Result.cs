using System.ComponentModel.DataAnnotations;

namespace CasseroleX.Application.Common.Models;

public class Result
{
    public int Code { get; set; }

    public string? Msg { get; set; }

    public object? Data { get; set; }

    public string? Url { get; set; }
    
    public int Wait { get; set; } = 3;

    public DateTime Time { get; set; }


    public Result(int code,string message, object? data = null, string? url = null, int wait = 3)
    {
        Code = code;
        Msg = message;  
        Data = data;
        Time = DateTime.Now;
        Url = url;
        Wait = wait;
    }

    public static Result Success(object? data = null, OperationResult operation = OperationResult.SUCCESS, string? url = null, int wait = 3)
    {
        return new Result(1, GetDisplayName(operation), data, url, wait);
    }

    public static Result Failure(OperationResult operation = OperationResult.FAIL, string? url = null, int wait = 3)
    {
        return new Result(0, GetDisplayName(operation), null, url, wait);
    }

    private static string GetDisplayName(OperationResult value)
    {
        var displayAttribute = value.GetType()
            .GetField(value.ToString())?
            .GetCustomAttributes(typeof(DisplayAttribute), false)
            .OfType<DisplayAttribute>()
            .FirstOrDefault();

        return displayAttribute?.Name ?? value.ToString();
    }
}