import { Component, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { pokemon } from '../TypescriptTypes/Pokemon';
import { PokemonsService } from '../services/pokemons.service';
import { pokemonId } from '../TypescriptTypes/pokemonId';

@Component({
  selector: 'app-pokemons',
  templateUrl: './pokemons.component.html',
  styleUrls: ['./pokemons.component.css']
})
export class PokemonsComponent implements OnInit {
  allPokemons: Array<pokemon> = [];
  nonTeamPokemon: Array<pokemon> =  [];
  TeamPokemon: Array<pokemon> = [];
  
  constructor(public _pokeS: PokemonsService) { }

  ngOnInit(): void {
    this.getAllUserPokemon();
   }

  getAllUserPokemon()
  {
    this._pokeS.getAllUserPokemon().subscribe(
      next => {
        this.allPokemons = next;
        this.setPokemon();
      }
    )
  }

  changePos(i: number, team: boolean)
  {
    let id = 100;
    if(team && this.nonTeamPokemon.length != 12)
    {
      this.TeamPokemon[i].isInTeam = false;
      id = this.TeamPokemon[i].id!;
    }
    else if(!team && this.TeamPokemon.length != 6)
    {
      this.nonTeamPokemon[i].isInTeam = true;
      id = this.nonTeamPokemon[i].id!;
    }
    this.setPokemon();
    let ids : pokemonId = {
      id: id,
      team: team
    }
    this.updateTeam(ids)
  }

  setPokemon()
  {
    this.TeamPokemon.splice(0, this.TeamPokemon.length + 1);
    this.nonTeamPokemon.splice(0, this.TeamPokemon.length + 1);
    for(let i = 0; i < this.allPokemons!.length; i++)
    {
      if(this.allPokemons![i].isInTeam == true && !this.TeamPokemon.includes(this.allPokemons[i]))
      {
        this.TeamPokemon!.push(this.allPokemons![i])
      }
      else if(!this.nonTeamPokemon.includes(this.allPokemons[i]))
      {
        this.nonTeamPokemon!.push(this.allPokemons![i])
      }
    }
  }

  updateTeam(ids: pokemonId)
  {
    this._pokeS.saveUserTeam(ids).subscribe();
  }

}

