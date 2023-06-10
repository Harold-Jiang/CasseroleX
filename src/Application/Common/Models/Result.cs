namespace CasseroleX.Application.Common.Models;

public class Result
{
    /// <summary>
    ///返回码 失败0 成功1
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    ///返回信息
    /// </summary>
    public string? Msg { get; set; }

    /// <summary>
    /// 返回数据
    /// </summary>
    public object? Data { get; set; }

    /// <summary>
    ///请求地址
    /// </summary>
    //public string Request { get; set; }

    public string? Url { get; set; }

    /// <summary>
    /// 等待时间
    /// </summary>
    public int Wait { get; set; } = 3;
    public DateTime Time { get; set; }


    public Result(int code, string message, object? data, string? url, int wait)
    {
        Code = code;
        Msg = message;
        Data = data;
        Time = DateTime.Now;
        Url = url;
        Wait = wait;
    }
    public static Result Success(object? data = null, string message = "操作成功", string? url = null, int wait = 3)
    {
        return new Result(1, message, data, url, wait);
    }

    public static Result Failure(string message = "操作失败", string? url = null, int wait = 3)
    {
        return new Result(0, message, null, url, wait);
    }

}