using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace LoginBoilerPlate.Services;

public class EFcoreUserService : IUserService
{
    private readonly PostgresContext _context;
    
    private readonly IMapper _mapper;
    
    public EFcoreUserService(PostgresContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    //TODO: Utiliser le token JWT pour la connexion
    public async Task<User?> Login(string mail, string motDePasse)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Mail == mail && u.MotDePasse == motDePasse);
    }
    

    public async Task<UserOutput> Add(UserInput user)
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
        return _mapper.Map<UserOutput>(entityEntry);
    }
    
}