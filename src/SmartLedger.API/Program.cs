using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using SmartLedger.Identity.Application;
using SmartLedger.Identity.Application.Commands.Login;
using SmartLedger.Identity.Application.Commands.Register;
using SmartLedger.Identity.Infrastructure;
using SmartLedger.Identity.Infrastructure.Endpoints;
using System.Text;
using MediatR; 


var builder = WebApplication.CreateBuilder(args);
/*
// Serilog
builder.Host.UseSerilog((ctx, config) =>
    config.ReadFrom.Configuration(ctx.Configuration)); */

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "SmartLedger API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new()
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter your JWT token here."
    });
    c.AddSecurityRequirement(new()
    {
        {
            new() { Reference = new() { Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, Id = "Bearer" } },
            []
        }
    });
});


// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!))
        };
    }); 

builder.Services.AddAuthorization();

// Modules
builder.Services.AddIdentityApplication();
builder.Services.AddIdentityInfrastructure(builder.Configuration);

var app = builder.Build();

Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");

// Middleware

app.UseSwagger();
app.UseSwaggerUI();


//app.UseSerilogRequestLogging();
app.UseAuthentication();
app.UseAuthorization();


// Identity endpoints
app.MapPost("/api/auth/register", async (RegisterCommand command, IMediator mediator) =>
{
    var result = await mediator.Send(command);
    return Results.Created("/api/auth/register", result);
})
.WithName("Register")
.WithOpenApi();

app.MapPost("/api/auth/login", async (LoginCommand command, IMediator mediator) =>
{
    var result = await mediator.Send(command);
    return Results.Ok(result);
})
.WithName("Login")
.WithOpenApi();

app.MapMeEndpoint();

/*
// Auto-migrate on startup (development only)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider
        .GetRequiredService<SmartLedger.Identity.Infrastructure.Persistence.IdentityDbContext>();
    await db.Database.MigrateAsync();
} */

app.Run();