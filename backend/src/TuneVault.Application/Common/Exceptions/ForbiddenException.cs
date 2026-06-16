namespace TuneVault.Application.Common.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException()
        : base("Access denied to this resource.")
    {
    }

    public ForbiddenException(string message)
        : base(message)
    {
    }
}
