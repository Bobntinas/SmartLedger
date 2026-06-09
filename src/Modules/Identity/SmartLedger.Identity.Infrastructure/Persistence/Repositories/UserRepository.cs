using Microsoft.EntityFrameworkCore;
using SmartLedger.Identity.Domain.Entities;
using SmartLedger.Identity.Domain.Repositories;

namespace SmartLedger.Identity.Infrastructure.Persistence.Repositories;

public sealed class UserRepository(IdentityDbContext context) : IUserRepository
{
    public async Task<AppUser?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default) =>
        await context.Users
            .FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant().Trim(),
                cancellationToken);

    public async Task<bool> ExistsAsync(
        string email,
        CancellationToken cancellationToken = default) =>
        await context.Users
            .AnyAsync(u => u.Email == email.ToLowerInvariant().Trim(),
                cancellationToken);

    public async Task AddAsync(
        AppUser user,
        CancellationToken cancellationToken = default)
    {
        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}