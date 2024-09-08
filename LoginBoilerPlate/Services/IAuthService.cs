namespace LoginBoilerPlate.Services;

public interface IAuthService
{
    public Task<User?> GetCurrentUserAsync();
    
}