namespace CasseroleX.Application.Common.Exceptions;
public class PermissionException : Exception
{
    public PermissionException(string? message) : base(message)
    {
    } 
}
