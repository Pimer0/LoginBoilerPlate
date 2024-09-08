using Npgsql;
using System.Security.Claims;
using System.Security.Cryptography;
using FluentValidation;
using LoginBoilerPlate;
using LoginBoilerPlate.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

RSA rsa = RSA.Create();
if (!File.Exists("key.bin"))
{
    var key = rsa.ExportRSAPrivateKey();
    File.WriteAllBytes("key.bin", key);
}
else
{
    rsa.ImportRSAPrivateKey(File.ReadAllBytes("key.bin"), out var _);
}

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        Description = "Token JWT. Saisir \"Bearer {Token}\""
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IUserService, EFCoreUserService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddSingleton<RsaSecurityKey>(new RsaSecurityKey(rsa));

builder.Services.AddAuthorization();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new RsaSecurityKey(rsa),
            ClockSkew = TimeSpan.Zero
        };
        options.Configuration = new OpenIdConnectConfiguration
        {
            SigningKeys = { new RsaSecurityKey(rsa) }
        };
        options.MapInboundClaims = false;
    });

builder.Services.AddHttpContextAccessor();

// Configuration de la base de données
string? connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("La chaîne de connexion n'est pas définie.");
}

builder.Services.AddDbContext<PostgresContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Test de connexion à la base de données
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PostgresContext>();
    try
    {
        dbContext.Database.OpenConnection();
        Console.WriteLine("Connexion réussie à la base de données PostgreSQL.");
        dbContext.Database.CloseConnection();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erreur lors de la connexion à la base de données : {ex.Message}");
    }
}



app.Run();