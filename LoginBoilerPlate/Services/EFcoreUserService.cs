using System.Security.Claims;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace LoginBoilerPlate.Services;

public class EFCoreUserService : IUserService
{
    private readonly RsaSecurityKey _key;
    private readonly JsonWebTokenHandler _jwtHandler;
    private readonly PostgresContext _context;
    private readonly IMapper _mapper;
    
    public EFCoreUserService(PostgresContext context, IMapper mapper, RsaSecurityKey key)
    {
        _context = context;
        _mapper = mapper;
        _key = key;
        _jwtHandler = new JsonWebTokenHandler();
    }


    public SecurityKey GetSecurityKey()
    {
        return _key;
    }

    public async Task<string?> Login(string mail, string motDePasse)
    {
        var user = await _context.Users
            .Where(u => u.Mail == mail)
            .Select(u => new { u.IdUser, u.Nom, u.MotDePasse, u.IdRole })
            .FirstOrDefaultAsync();

        if (user == null || !VerifyPassword(motDePasse, user.MotDePasse))
        {
            return null;
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
            new Claim(ClaimTypes.Name, user.Nom),
            new Claim(ClaimTypes.Email, mail),
            new Claim(ClaimTypes.Role, user.IdRole.ToString())
        };

        var token = _jwtHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.RsaSha256)
        });

        return token;
    }

    private bool VerifyPassword(string inputPassword, string storedHash)
    {
        // Implémentez ici une vérification sécurisée du mot de passe
        // Par exemple, avec BCrypt :
        // return BCrypt.Net.BCrypt.Verify(inputPassword, storedHash);
        
        // Pour l'instant, nous utilisons une comparaison simple (à ne pas utiliser en production)
        return inputPassword == storedHash;
    }

    public async Task<UserOutput> Add(UserInput user)
    {
        var entityEntry = new User
        {
            Nom = user.Nom,
            Prenom = user.Prenom,
            Mail = user.Mail,
            MotDePasse = user.MotDePasse, // Assurez-vous de hacher le mot de passe avant de le stocker
            // IdRole doit être défini ici si nécessaire
        };
        _context.Users.Add(entityEntry);
        await _context.SaveChangesAsync();
        return _mapper.Map<UserOutput>(entityEntry);
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }
}