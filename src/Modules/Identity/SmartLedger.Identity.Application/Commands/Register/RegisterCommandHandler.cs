using MediatR;
using SmartLedger.Identity.Application.Abstractions;
using SmartLedger.Identity.Application.DTOs;
using SmartLedger.Identity.Domain.Entities;
using SmartLedger.Identity.Domain.Repositories;

namespace SmartLedger.Identity.Application.Commands.Register;

public sealed class RegisterCommandHandler(
    IUserRepository userRepository,
    IJwtTokenService jwtTokenService) : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var exists = await userRepository.ExistsAsync(request.Email, cancellationToken);

        if (exists)
            throw new InvalidOperationException($"A user with email '{request.Email}' already exists.");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = AppUser.Create(
            request.Email,
            passwordHash,
            request.FirstName,
            request.LastName);

        await userRepository.AddAsync(user, cancellationToken);

        var token = jwtTokenService.GenerateToken(user);

        return new AuthResponseDto(
            token,
            user.Email,
            user.FirstName,
            user.LastName,
            DateTime.UtcNow.AddHours(24));
    }
}