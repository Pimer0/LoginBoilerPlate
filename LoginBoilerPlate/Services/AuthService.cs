using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LoginBoilerPlate;
using LoginBoilerPlate.Services;

public class AuthService : IAuthService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;
    private readonly JwtSecurityTokenHandler _tokenHandler;

    public AuthService(IHttpContextAccessor httpContextAccessor, IUserService userService)
    {
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
        _tokenHandler = new JwtSecurityTokenHandler();
    }

    public async Task<User?> GetCurrentUserAsync()
    {
        var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        
        if (string.IsNullOrEmpty(token))
            return null;

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _userService.GetSecurityKey(),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            var principal = _tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId != null && int.TryParse(userId, out int id))
            {
                return await _userService.GetUserByIdAsync(id);
            }
        }
        catch (SecurityTokenException)
        {
            // Token invalide ou expiré
            return null;
        }
        catch (Exception ex)
        {
            // Log l'exception
            Console.WriteLine($"Une erreur s'est produite lors de la validation du token : {ex.Message}");
            return null;
        }

        return null;
    }
}