using MediatR;
using SmartLedger.Identity.Application.DTOs;

namespace SmartLedger.Identity.Application.Commands.Login;

public sealed record LoginCommand(
    string Email,
    string Password) : IRequest<AuthResponseDto>;