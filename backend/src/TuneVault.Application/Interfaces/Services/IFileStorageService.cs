using Microsoft.AspNetCore.Http;

namespace TuneVault.Application.Interfaces.Services;

public interface IFileStorageService
{
    Task<string> SaveAsync(
        IFormFile file,
        CancellationToken cancellationToken);
}