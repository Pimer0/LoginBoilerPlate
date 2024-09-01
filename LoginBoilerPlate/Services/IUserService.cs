namespace LoginBoilerPlate.Services;

public interface IUserService
{
    Task<List<User>> GetAll( CancellationToken token);
    Task<User?> GetById(Guid id);
    Task<User> Add(UserInput user);
    Task<bool> Update(Guid id, User user);
    Task<bool> Delete(Guid id);
}