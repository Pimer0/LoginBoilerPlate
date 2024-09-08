using Microsoft.IdentityModel.Tokens;

namespace LoginBoilerPlate.Services;

public interface IUserService
{
    
    Task<UserOutput> Add(UserInput user);
    
    Task<User> GetUserByIdAsync(int id);
    
    public SecurityKey GetSecurityKey();
    Task<string?> Login(string mail, string motDePasse);
}