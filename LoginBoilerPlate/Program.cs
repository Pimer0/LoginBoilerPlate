using Npgsql;

var builder = WebApplication.CreateBuilder(args);
DotNetEnv.Env.Load();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
string? connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
        
if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("Erreur : La chaîne de connexion n'est pas définie.");
    return;
}

try
{
    using (var connection = new NpgsqlConnection(connectionString))
    {
        connection.Open();
        Console.WriteLine("Connexion réussie à la base de données PostgreSQL.");

        // Ici, vous pouvez exécuter vos requêtes SQL
        // Par exemple :
        // using (var cmd = new NpgsqlCommand("SELECT * FROM ma_table", connection))
        // {
        //     using (var reader = cmd.ExecuteReader())
        //     {
        //         while (reader.Read())
        //         {
        //             Console.WriteLine(reader.GetString(0));
        //         }
        //     }
        // }

        connection.Close();
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Erreur lors de la connexion à la base de données : {ex.Message}");
}





app.Run();
