using MediatR;
using SmartLedger.Identity.Application.Abstractions;
using SmartLedger.Identity.Application.DTOs;
using SmartLedger.Identity.Domain.Repositories;

namespace SmartLedger.Identity.Application.Commands.Login;

public sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IJwtTokenService jwtTokenService) : IRequestHandler<LoginCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        var token = jwtTokenService.GenerateToken(user);

        return new AuthResponseDto(
            token,
            user.Email,
            user.FirstName,
            user.LastName,
            DateTime.UtcNow.AddHours(24));
    }
}