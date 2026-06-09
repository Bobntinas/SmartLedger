using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace SmartLedger.Identity.Infrastructure.Endpoints;

public static class MeEndpoint
{
    public static void MapMeEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/me", (ClaimsPrincipal user) =>
        {
            var id = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = user.FindFirstValue(ClaimTypes.Email);
            var firstName = user.FindFirstValue(ClaimTypes.GivenName);
            var lastName = user.FindFirstValue(ClaimTypes.Surname);

            return Results.Ok(new { id, email, firstName, lastName });
        })
        .RequireAuthorization()
        .WithName("GetCurrentUser")
        .WithTags("Identity")
        .WithSummary("Returns the currently authenticated user's profile.");
    }
}