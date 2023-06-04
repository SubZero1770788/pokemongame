import { Component, OnInit } from '@angular/core';
import { PokemonsService } from '../services/pokemons.service';
import { pokemon } from '../TypescriptTypes/Pokemon';
import { Pagination } from '../TypescriptTypes/Pagination';
import { ColorService } from '../services/color.service';

@Component({
  selector: 'app-pokedex',
  templateUrl: './pokedex.component.html',
  styleUrls: ['./pokedex.component.css']
})
export class PokedexComponent implements OnInit {
  pokemon: any;
  pokemons: pokemon[] = [];
  pagination: Pagination | undefined;
  pageNumber = 1;
  pageSize = 40;
  color1: string = "rgb(240,180,180)";
  color2: string = "rgb(250,180,180)";
  pokedex:boolean = true;
  
  constructor( public _pokeS: PokemonsService, public _colS: ColorService) { }

  ngOnInit(): void {
    this.pokedex = true;
    this.loadPokemons(false);
  }

  loadPokemons(go: boolean)
  {
    if(go)
    {
      this.pageNumber++;
    }
    else if(this.pageNumber != 1)
    {
      this.pageNumber--;
    }
    this._pokeS.getPokemons(this.pageNumber, this.pageSize).subscribe({
      next: r => {
        if(r.result && r.pagination)
        {
          this.pokemons = r.result;
          this.pagination = r.pagination;
        }
      }
    })
  }

  return()
  {
    this.pokedex = true;
  }

  getPokemon(n: number){
    this._pokeS.getPokemon(n + this.pageSize*(this.pageNumber - 1)).subscribe(
      r =>{
        console.log(r);
        this.pokemon = r
        this.pokedex = false;
        this.color1 = this._colS.getBackgroundColor(this.pokemon!.pokemonType1, this.color1);
        this.color2 = this._colS.getBackgroundColor(this.pokemon!.pokemonType2, this.color2);
      }
    )
  }
}