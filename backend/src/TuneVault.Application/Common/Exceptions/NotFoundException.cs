using System.Collections.Generic;

namespace TuneVault.Application.Common.Exceptions;

public class NotFoundException : KeyNotFoundException
{
    public NotFoundException()
        : base("Requested resource was not found.")
    {
    }

    public NotFoundException(string message)
        : base(message)
    {
    }
}
