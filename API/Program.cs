using System.Data.Common;
using System.Text;
using API.Data;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAppServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

string connString = "";
if (builder.Environment.IsDevelopment()) 
    connString = builder.Configuration.GetConnectionString("DefaultConnection");
else 
{
// Use connection string provided at runtime by FlyIO.
        var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

        // Parse connection URL to connection string for Npgsql
        connUrl = connUrl.Replace("postgres://", string.Empty);
        var pgUserPass = connUrl.Split("@")[0];
        var pgHostPortDb = connUrl.Split("@")[1];
        var pgHostPort = pgHostPortDb.Split("/")[0];
        var pgDb = pgHostPortDb.Split("/")[1];
        var pgUser = pgUserPass.Split(":")[0];
        var pgPass = pgUserPass.Split(":")[1];
        var pgHost = pgHostPort.Split(":")[0];
        var pgPort = pgHostPort.Split(":")[1];
	var updatedHost = pgHost.Replace("flycast", "internal");

        connString = $"Server={updatedHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};";
}
builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseNpgsql(connString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.00000
app.UseCors(builder => builder.AllowAnyHeader()
.AllowAnyMethod().WithOrigins("https://localhost:4200"));

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();
app.MapFallbackToController("Index", "Fallback");

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var data = services.GetRequiredService<DataContext>();
    var manager = services.GetRequiredService<UserManager<UserData>>();
    await data.Database.MigrateAsync();
    await PokemonSeeder.SeedTypes(data);
    await PokemonSeeder.SeedPlaces(data);
    await PokemonSeeder.SeedUsers(manager);
    await PokemonSeeder.SeedItems(data);
    await PokemonSeeder.SeedPokemon(data);
    await PokemonSeeder.AttackSeeder(data);
    await PokemonSeeder.SeedUserItems(data);
    await PokemonSeeder.SeedUserPokemons(data);
    await PokemonSeeder.SeederPokemonAttackJunction(data);
    await PokemonSeeder.SeedPokemonTypesJunction(data);
    await PokemonSeeder.SeedResistantJunction(data);
    await PokemonSeeder.SeedWeakJunction(data);
    await PokemonSeeder.SeedIneffectiveJunction(data);
    //seedAbilities
    //seedStatusAttacks
}
catch (Exception ex)
{
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An error occured");
}

app.Run();
