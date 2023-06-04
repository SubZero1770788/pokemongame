import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, EventEmitter} from '@angular/core';
import { WildPokemon } from '../TypescriptTypes/WildPokemon';
import { pokemon } from '../TypescriptTypes/Pokemon';
import { Item } from '../TypescriptTypes/Item';
import { catchString } from '../TypescriptTypes/catchString';
import { pokedexPokemon } from '../TypescriptTypes/PokedexPokemon';
import { map } from 'rxjs';
import { PaginatedResult } from '../TypescriptTypes/Pagination';
import { usingItem } from '../TypescriptTypes/usingItem';
import { pokemonId } from '../TypescriptTypes/pokemonId';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PokemonsService {
  baseUrl = environment.baseUrl;
  pokemons: pokemon[] = [];
  paginatedResult: PaginatedResult<pokemon[]> = new PaginatedResult<any[]>;
  currentPokemon: any;
  userPokemons = new EventEmitter();
  currentWildPlace: any;
  points: number = 1;
  pokedexPokemon: any;
  health: [number, number, number] = [0 , 0, 0];
  outputter = new EventEmitter<catchString>();

  constructor(private http: HttpClient) { }

  passForm(cs: catchString)
  {
    this.outputter.emit(cs);
  }

  getPokemons(page?:number, itemsPerPage?: number)
  {
    let params = new HttpParams();

    if(page && itemsPerPage)
    {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    return this.http.get<pokemon[]>(this.baseUrl + "pokemon", {observe: "response", params}).pipe(
    map(response => {
      if(response.body) {
        this.paginatedResult.result = response.body;
      }
      const pag = response.headers.get('Pagination');
      if(pag)
      {
        this.paginatedResult.pagination = JSON.parse(pag);
      }
      return this.paginatedResult;
    })
    )
  }

  passTeam(p: Array<pokemon>)
  {
    this.userPokemons.emit(p);
  }

  getEncounter(pokemon: WildPokemon)
  {
    this.currentWildPlace = pokemon.id;
    this.points--;
    return this.http.post(this.baseUrl + "pokemon/" + pokemon.id, pokemon);
  }
  
  getUserPokemon(pokemon: WildPokemon)
  {
   return this.http.post<Array<pokemon>>(this.baseUrl + "userpokemon", pokemon);
  }

  tryCatching(item: Item)
  {
    return this.http.post<catchString>(this.baseUrl + "pokemon/catch", item);
  }

  getPokemon(n: number)
  {
    return this.http.get<pokedexPokemon>(this.baseUrl + "pokemon/pokedex/" + n);
  }

  healPokemon(ut: usingItem)
  {
    return this.http.post<catchString>(this.baseUrl + "battle/heal/" + ut.pokemonId, ut);
  }

  getAllUserPokemon()
  {
    return this.http.get<Array<pokemon>>(this.baseUrl + "userpokemon/team", );
  }

  saveUserTeam(p: pokemonId)
  {
    return this.http.post(this.baseUrl + "userpokemon/saveteam", p);
  }
}
