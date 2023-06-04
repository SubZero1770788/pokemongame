namespace API.Entities.HelperEntities
{
    public class PokemonUser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserData User { get; set; }
        public int PokemonId { get; set; }
        public Pokemon Pokemon { get; set; }
        public string Name { get; set; }
        public int Level { get; set; } 
        public int Experience { get; set; }
        public int HP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int SpecialAttack { get; set; }
        public int SpecialDefense { get; set; }
        public int Speed { get; set; }
        public int Attack1Id { get; set; }
        public int Attack2Id { get; set; }
        public int Attack3Id { get; set; }
        public int Attack4Id { get; set; }
        public bool IsInTeam { get; set; }
    }
}