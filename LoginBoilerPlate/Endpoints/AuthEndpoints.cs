namespace LoginBoilerPlate.Endpoints;

public class AuthEndpoints
{
    /*// Ajout d'un endpoint pour récupérer l'utilisateur courant
    app.MapGet("/api/current-user", [Authorize] async (HttpContext httpContext, AuthService authService) =>
    {
        var user = await authService.GetCurrentUserAsync();
        if (user == null)
            return Results.Unauthorized();

        return Results.Ok(new { user.IdUser, user.Nom, user.Mail });
    }).RequireAuthorization();*/
}