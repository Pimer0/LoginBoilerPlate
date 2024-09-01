namespace LoginBoilerPlate.Services;

public class EFcoreUserService : IUserService
{
    private readonly PostgresContext _context;
    public Task<List<User>> GetAll(
         CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<User> Add(UserInput user)
    {
        var entityEntry = new User
        {
            Nom = user.Nom,
            Prenom = user.Prenom,
            Mail = user.Mail,
            MotDePasse = user.MotDePasse,
        };
        _context.Users.Add(entityEntry);
        await _context.SaveChangesAsync();
        return entityEntry;
    }

    public Task<bool> Update(Guid id, User user)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}