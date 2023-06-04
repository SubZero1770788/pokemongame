import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { pokemon } from '../../TypescriptTypes/Pokemon';
import { PokemonsService } from '../../services/pokemons.service';
import { WildBattlerService } from '../../services/wild-battler.service';
import { WildPokemon } from '../../TypescriptTypes/WildPokemon';

@Component({
  selector: 'app-pokemon-team',
  templateUrl: './pokemon-team.component.html',
  styleUrls: ['./pokemon-team.component.css']
})
export class PokemonTeamComponent implements OnInit {
  @Input()userPokemons: Array<pokemon> | null = null; 
  @Output() battleMode = new EventEmitter();
  
  constructor(public _pokS: PokemonsService, public _wildS: WildBattlerService) { }

  ngOnInit(): void {
    this.loadPokemons();
  }

  loadPokemons()
  {
    this._pokS.userPokemons.subscribe(
      a => {
        this.userPokemons = a;
      }
    )

  }

  startBattle(index: number)
  {
    let wildPokemon:WildPokemon = {
      id: this.userPokemons![index].id!,
      isfirst: false
    };
    this._wildS.startBattle(wildPokemon).subscribe(
      response =>{
        this._wildS.pokemonBattle = response;
        this.battleMode.emit(2);
      }
    );
    this._pokS.health[0] = 0;
    this._pokS.health[1] = 0;
    this._pokS.health[2] = this.userPokemons![index].id!;
  }
}


