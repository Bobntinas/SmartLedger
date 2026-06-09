using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartLedger.Identity.Application.Abstractions;
using SmartLedger.Identity.Domain.Repositories;
using SmartLedger.Identity.Infrastructure.Persistence;
using SmartLedger.Identity.Infrastructure.Persistence.Repositories;
using SmartLedger.Identity.Infrastructure.Services;

namespace SmartLedger.Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<IdentityDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }
}