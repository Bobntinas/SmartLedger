using SmartLedger.Identity.Domain.Entities;

namespace SmartLedger.Identity.Application.Abstractions;

public interface IJwtTokenService
{
    string GenerateToken(AppUser user);
}