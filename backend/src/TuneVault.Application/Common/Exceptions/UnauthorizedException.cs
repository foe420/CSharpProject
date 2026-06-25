using System;

namespace TuneVault.Application.Common.Exceptions;

public class UnauthorizedException : UnauthorizedAccessException
{
    public UnauthorizedException()
        : base("Unauthorized access to this resource.")
    {
    }

    public UnauthorizedException(string message)
        : base(message)
    {
    }
}
