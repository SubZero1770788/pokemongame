using System.Text.Json;
using API.Entities;
using API.Entities.HelperEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class PokemonSeeder
    {
        public static async Task SeedPokemon(DataContext data)
        {
            if (await data.Pokemons.AnyAsync()) return;

            var pokemonData = await File.ReadAllTextAsync("Data/SeedGenOnePokedex.json");

            var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

            var pokemons = JsonSerializer.Deserialize<List<Pokemon>>(pokemonData);

            foreach(var pokemon in pokemons)
            {
                data.Pokemons.Add(pokemon);
            }

            await data.SaveChangesAsync();
        }

        public static async Task SeedUsers(UserManager<UserData> data)
        {
            if (await data.Users.AnyAsync()) return;

            var usersData = await File.ReadAllTextAsync("Data/SeedUsers.json");

            var opt = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

            var users = JsonSerializer.Deserialize<List<UserData>>(usersData);

            foreach(var us in users)
            {

                us.UserName = us.UserName.ToLower();
                
                await data.CreateAsync(us, "Pa$$w0rd");
            }
        }

        public static async Task SeedItems(DataContext data)
        {
            if (await data.Items.AnyAsync()) return;

            var ItemsData = await File.ReadAllTextAsync("Data/SeedItems.json");

            var opt = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

            var items = JsonSerializer.Deserialize<List<Item>>(ItemsData);

            foreach(var i in items)
            {
                data.Items.Add(i);
            }

            await data.SaveChangesAsync();
        }

        public static async Task SeedPlaces(DataContext data)
        {
            if (await data.WildPlaces.AnyAsync()) return;

            var placesData = await File.ReadAllTextAsync("Data/SeedPlaces.json");

            var opt = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

            var places = JsonSerializer.Deserialize<List<WildPlace>>(placesData);

            foreach(var p in places)
            {
                data.WildPlaces.Add(p);
            }

            await data.SaveChangesAsync();
        }

        public static async Task SeedTypes(DataContext data)
        {
            if (await data.WildPlaces.AnyAsync()) return;

            var typesData = await File.ReadAllTextAsync("Data/SeedType.json");

            var opt = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

            var types = JsonSerializer.Deserialize<List<PokemonType>>(typesData);

            var typesWeak = JsonSerializer.Deserialize<List<WeakTypes>>(typesData);
            var typesResistant = JsonSerializer.Deserialize<List<ResistantTypes>>(typesData);
            var typesIneffective = JsonSerializer.Deserialize<List<IneffectiveTypes>>(typesData);

            foreach(var p in types)
            {
                data.PokemonTypes.Add(p);
            }
            foreach(var p in typesWeak)
            {
                data.WeakTypes.Add(p);
            }
            foreach(var p in typesResistant)
            {
                data.ResistantTypes.Add(p);
            }
            foreach(var p in typesIneffective)
            {
                data.IneffectiveTypes.Add(p);
            }


            await data.SaveChangesAsync();
        }

        public static async Task AttackSeeder(DataContext data)
        {
            if (data.Attacks.Any()) return;

            var attackData = await File.ReadAllTextAsync("Data/SeedAttacks.json");

            var opt = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

            var attacks = JsonSerializer.Deserialize<List<Attack>>(attackData);

            foreach(var a in attacks)
            {
                data.Attacks.Add(a);
            }

            await data.SaveChangesAsync();
        }
        public static async Task SeederPokemonAttackJunction(DataContext data)
        {
            if (data.PokemonAttacks.Any()) return;

            var pokemonattackData = await File.ReadAllTextAsync("Data/SeedPokemonAttacksJunction.json");

            var opt = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

            var pokemonattacks = JsonSerializer.Deserialize<List<PokemonAttacks>>(pokemonattackData);

            foreach(var p in pokemonattacks)
            {        
                data.PokemonAttacks.Add(p);
            }

            await data.SaveChangesAsync();
        }

        public static async Task SeedUserItems(DataContext data)
        {
            if (data.ItemUsers.Any()) return;

            var useritemsData = await File.ReadAllTextAsync("Data/SeedUserItems.json");

            var opt = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

            var useritems = JsonSerializer.Deserialize<List<ItemUser>>(useritemsData);

            foreach(var ui in useritems)
            { 
               data.ItemUsers.Add(ui);
            }
            
            await data.SaveChangesAsync();
        }
        public static async Task SeedPokemonTypesJunction(DataContext data)
        {
            if (data.PokemonPokemonTypes.Any()) return;

            var pokemontypesData = await File.ReadAllTextAsync("Data/SeedPokemonTypesJunction.json");

            var o = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

            var pokemontypes = JsonSerializer.Deserialize<List<PokemonPokemonType>>(pokemontypesData);

            foreach(var ppt in pokemontypes)
            {
                data.PokemonPokemonTypes.Add(ppt);
            }
            
            await data.SaveChangesAsync();
        }

        public static async Task SeedUserPokemons(DataContext data)
        {
            if (data.PokemonUsers.Any()) return;

            var userPokemonsData = await File.ReadAllTextAsync("Data/SeedUserPokemons.json");

            var o = new JsonSerializerOptions{PropertyNameCaseInsensitive=true};

            var userPokemon = JsonSerializer.Deserialize<List<PokemonUser>>(userPokemonsData);

            foreach(var pu in userPokemon)
            {
                data.PokemonUsers.Add(pu);
            }

            await data.SaveChangesAsync();
        }

        public static async Task SeedWeakJunction(DataContext data)
        {
            if (data.Weaks.Any()) return;

            var ineffectiveTypesData = await File.ReadAllTextAsync("Data/SeedWeakJunction.json");

            var o = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

            var ineffectiveTypes = JsonSerializer.Deserialize<List<Weak>>(ineffectiveTypesData);

            foreach (var it in ineffectiveTypes)
            {
                data.Weaks.Add(it);
            }

            await data.SaveChangesAsync();
        }

        public static async Task SeedResistantJunction(DataContext data)
        {
             if (data.Resistants.Any()) return;

            var ineffectiveTypesData = await File.ReadAllTextAsync("Data/SeedResistantJunction.json");

            var o = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

            var ineffectiveTypes = JsonSerializer.Deserialize<List<Resistant>>(ineffectiveTypesData);

            foreach (var it in ineffectiveTypes)
            {
                data.Resistants.Add(it);
            }

            await data.SaveChangesAsync();
        }
        public static async Task SeedIneffectiveJunction(DataContext data)
        {
            if (data.Ineffectives.Any()) return;

            var ineffectiveTypesData = await File.ReadAllTextAsync("Data/SeedIneffectiveJunction.json");

            var o = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

            var ineffectiveTypes = JsonSerializer.Deserialize<List<Ineffective>>(ineffectiveTypesData);

            foreach (var it in ineffectiveTypes)
            {
                data.Ineffectives.Add(it);
            }

            await data.SaveChangesAsync();
        }
    }
}