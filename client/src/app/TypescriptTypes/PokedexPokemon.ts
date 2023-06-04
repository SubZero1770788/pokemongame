export interface pokedexPokemon
{
    id:number,
    name:string,
    level:number,
    Hp:number,
    attack:number,
    defense:number,
    specialAttack:number,
    specialDefense:number,
    speed:number,
    photoUrl:string,
    pokemonType1:string,
    pokemonType2?:string,
    wildPlace:string,
    attacks: Array<string>
}