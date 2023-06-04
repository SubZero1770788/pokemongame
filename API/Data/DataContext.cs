using API.Entities;
using API.Entities.HelperEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext: IdentityDbContext<UserData, UserRoles, int,
     IdentityUserClaim<int>, UserDataUserRole, IdentityUserLogin<int>, 
     IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Pokemon> Pokemons { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<WildPlace> WildPlaces { get; set; }       
        public DbSet<PokemonType> PokemonTypes { get; set; }
        public DbSet<Attack> Attacks {get; set;}
        public DbSet<PokemonUser> PokemonUsers {get; set;}
        public DbSet<ItemUser> ItemUsers {get; set;}
        public DbSet<IneffectiveTypes> IneffectiveTypes { get; set; }
        public DbSet<WeakTypes> WeakTypes { get; set; }
        public DbSet<ResistantTypes> ResistantTypes { get; set; }
        public DbSet<Entities.PokemonAttacks> PokemonAttacks {get; set;}
        public DbSet<Resistant> Resistants {get; set;}
        public DbSet<Ineffective> Ineffectives {get; set;}
        public DbSet<Weak> Weaks {get; set;}
        public DbSet<PokemonPokemonType> PokemonPokemonTypes {get; set;}
        public DbSet<CurrentEncounter>  CurrentPokemonEncounter {get; set;}

        protected override void OnModelCreating(ModelBuilder builder){

            base.OnModelCreating(builder);

            builder.Entity<UserData>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Entity<UserRoles>()
                .HasMany(ur => ur.UserRolesCol)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            builder.Entity<ItemUser>()
                .HasKey(iu => new{iu.UserId, iu.ItemId});

            builder.Entity<ItemUser>()
                .HasOne(iu => iu.User)
                .WithMany(u => u.ItemUsers)
                .HasForeignKey(iu => iu.UserId);

            builder.Entity<ItemUser>()
                .HasOne(iu => iu.Item)
                .WithMany(i => i.ItemUsers)
                .HasForeignKey(iu => iu.ItemId);

            builder.Entity<PokemonUser>()
                .HasKey(pu => new { pu.UserId, pu.PokemonId});

            builder.Entity<PokemonUser>()
                .HasOne(pu => pu.Pokemon)
                .WithMany(p => p.PokemonUsers)
                .HasForeignKey(pu => pu.PokemonId);
            
            builder.Entity<PokemonUser>()
                .HasOne(pu => pu.User)
                .WithMany(u => u.Pokemons)
                .HasForeignKey(pu => pu.UserId);

            builder.Entity<PokemonType>()
                .HasMany(pt => pt.Attacks)
                .WithOne(a => a.PokemonType)
                .HasForeignKey(a => a.PokemonTypeId);
                
            builder.Entity<WildPlace>()
                .HasMany(wp => wp.Pokemons)
                .WithOne(p => p.WildPlace)
                .HasForeignKey(p => p.WildPlaceId);

            builder.Entity<Attack>()
                .HasMany(pa => pa.PokemonAttacks)
                .WithOne(pa => pa.Attack)
                .HasPrincipalKey(p => p.Id);

            builder.Entity<Pokemon>()
                .HasMany(p => p.PokemonAttacks)
                .WithOne(pa => pa.Pokemon)
                .HasPrincipalKey(p => p.Id);

            builder.Entity<PokemonAttacks>()
                .HasKey(pa => pa.Id);

            builder.Entity<PokemonType>()
                .HasMany(pt =>  pt.Ineffective)
                .WithOne(w => w.PokemonType)
                .HasPrincipalKey(pt => pt.Id);

            builder.Entity<IneffectiveTypes>()
                .HasMany(it => it.Ineffectives)
                .WithOne(w => w.IneffectiveTypes)
                .HasPrincipalKey(it => it.Id);
                
            builder.Entity<Ineffective>()
                .HasKey(ie => ie.Id);

            builder.Entity<PokemonType>()
                .HasMany(pt =>  pt.Weaks)
                .WithOne(w => w.PokemonType)
                .HasPrincipalKey(pt => pt.Id);

            builder.Entity<WeakTypes>()
                .HasMany(it => it.Weaks)
                .WithOne(w => w.WeakTypes)
                .HasPrincipalKey(it => it.Id);

            builder.Entity<Weak>()
                .HasKey(pa => pa.Id);

            builder.Entity<PokemonType>()
                .HasMany(pt =>  pt.Resistant)
                .WithOne(w => w.PokemonType)
                .HasPrincipalKey(pt => pt.Id);

            builder.Entity<ResistantTypes>()
                .HasMany(it => it.Resistants)
                .WithOne(w => w.ResistantTypes)
                .HasPrincipalKey(it => it.Id);

            builder.Entity<Resistant>()
                .HasKey(re => re.Id);

            builder.Entity<PokemonType>()
                .HasMany(pt =>  pt.Resistant)
                .WithOne(w => w.PokemonType)
                .HasPrincipalKey(pt => pt.Id);

            builder.Entity<ResistantTypes>()
                .HasMany(it => it.Resistants)
                .WithOne(w => w.ResistantTypes)
                .HasPrincipalKey(it => it.Id);
                
            builder.Entity<PokemonPokemonType>()
                .HasKey(ie => ie.Id);

        }
    }
}