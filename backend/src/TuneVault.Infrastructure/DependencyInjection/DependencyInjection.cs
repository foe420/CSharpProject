using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TuneVault.Application.Interfaces.Persistence;
using TuneVault.Application.Interfaces.Services;
using TuneVault.Domain.Entities;
using TuneVault.Infrastructure.Auth;
using TuneVault.Infrastructure.Identity;
using TuneVault.Infrastructure.Persistence;
using TuneVault.Infrastructure.Repositories;
using TuneVault.Infrastructure.Services;

namespace TuneVault.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 8;
        })
        .AddUserStore<ApplicationUserStore>();

        services.AddScoped<IMediaRepository, MediaRepository>();
        services.AddScoped<IPlaylistRepository, PlaylistRepository>();
        services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        services.AddScoped<IFavoriteRepository, FavoriteRepository>();
        services.AddScoped<IPlayHistoryRepository, PlayHistoryRepository>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IFileStorageService, LocalFileStorageService>();

        services.AddScoped<IMediaShareRepository, MediaShareRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<INotificationPushService, SignalRNotificationPushService>();
        services.AddScoped<IFollowRepository, FollowRepository>();

        return services;
    }
}
