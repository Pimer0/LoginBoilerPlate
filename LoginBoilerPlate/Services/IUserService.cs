namespace LoginBoilerPlate.Services;

public interface IUserService
{
    
    Task<UserOutput> Add(UserInput user);
    Task<User?> Login(string mail, string motDePasse);
}