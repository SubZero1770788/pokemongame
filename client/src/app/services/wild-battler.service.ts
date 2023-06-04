import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { WildPokemon } from '../TypescriptTypes/WildPokemon';
import { pokemon } from '../TypescriptTypes/Pokemon';
import { Observable } from 'rxjs';
import { chosenAttack } from '../TypescriptTypes/chosenAttack';
import { catchString } from '../TypescriptTypes/catchString';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class WildBattlerService {
  baseUrl = environment.baseUrl;
  pokemonBattle: Array<pokemon> | null = null;
  constructor(private http: HttpClient) { }

  startBattle(p: WildPokemon):Observable<pokemon[]>
  {
    return this.http.post<Array<pokemon>>(this.baseUrl + "battle", p);
  }

  chooseAttack(ca : chosenAttack)
  {
    return this.http.post<catchString>(this.baseUrl + "battle/" + ca.chosenAttackId, ca);
  }

  changeAttack(ca: chosenAttack)
  {
    return this.http.post<catchString>(this.baseUrl + "battle/attackChange", ca);
  }
}
