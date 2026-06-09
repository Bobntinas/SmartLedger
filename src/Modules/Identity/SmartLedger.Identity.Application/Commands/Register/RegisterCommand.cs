using MediatR;
using SmartLedger.Identity.Application.DTOs;

namespace SmartLedger.Identity.Application.Commands.Register;

public sealed record RegisterCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName) : IRequest<AuthResponseDto>;