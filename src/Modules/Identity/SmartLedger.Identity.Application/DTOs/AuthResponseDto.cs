namespace SmartLedger.Identity.Application.DTOs;

public sealed record AuthResponseDto(
    string Token,
    string Email,
    string FirstName,
    string LastName,
    DateTime ExpiresAt);