import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';
import { pokemon } from 'src/app/TypescriptTypes/Pokemon';
import { WildPokemon } from 'src/app/TypescriptTypes/WildPokemon';
import { catchString } from 'src/app/TypescriptTypes/catchString';
import { PokemonsService } from 'src/app/services/pokemons.service';
import { ShopService } from 'src/app/services/shop.service';
import { BattleComponent } from '../battle/battle.component';
import { HealingComponent } from '../healing/healing.component';
import { ColorService } from 'src/app/services/color.service';
import { environment } from 'src/environments/environment';


@Component({
  selector: 'app-wild',
  templateUrl: './wild.component.html',
  styleUrls: ['./wild.component.css']
})
export class WildComponent implements OnInit, OnChanges {
  @ViewChild('currentBattle') currentBattle!: BattleComponent;
  @ViewChild('healing') healing!:HealingComponent;
  baseUrl = environment.baseUrl;
  Wildpokemon: any;
  userPokemons: Array<pokemon> | null = null;
  searchItems: any;
  wildPlace : any;
  battleMode: number = 0;
  previousMode: number = 0;
  @Input()messageFrom: catchString | null = null;
  @Output() passMessage = new EventEmitter();
  color1: string = 'rgb(155, 220, 155)';
  color2: string = 'rgb(155, 220, 155)';

  constructor(
    private http:HttpClient, 
    public _pokemonServ: PokemonsService, 
    public _shopS: ShopService,
    public _colS: ColorService) { }

  ngOnChanges(changes: SimpleChanges): void {
   this.passMessage.emit(this.messageFrom);
  }

  ngOnInit(): void {
    this.loadPlaces();
  }

  loadPlaces()
  {
    this.http.get(this.baseUrl + "wildplaces").subscribe({
      next: r => this.wildPlace = r,
      error: e => console.log(e)
    })
  }

  getPokemon(WildPokemon: WildPokemon)
  {
    this._pokemonServ.getUserPokemon(WildPokemon).subscribe(
      response => {
        this.userPokemons = response;
      }
    )
  }

  move(id:number)
  {
    let WildPokemon: WildPokemon = { id: id, isfirst: true};
    this._pokemonServ.getEncounter(WildPokemon).subscribe(
      response =>{
        this.Wildpokemon = response;
        this.color1 = this._colS.getBackgroundColor(this.Wildpokemon.pokemonType1Name, this.color1);
        this.color2 = this._colS.getBackgroundColor(this.Wildpokemon.pokemonType2Name, this.color2);
     }
    )
    this.getPokemon(WildPokemon);
    this.reducePoints();
    this._shopS.itemChosen = false;
    this.battleMode = 1;
  }

  goTo(i: number)
  {
    switch(i)
    {
      case 0:
        this.battleMode = 0;
      break;
      case 1:
        this.battleMode = 1;
      break;
      case 2:
        if(this.previousMode)
        {
          this.battleMode = this.previousMode;
          this.previousMode = 0;
        }
        else
        {
          this.battleMode = 2;
        }
      break;
      case 3:
        this.previousMode = this.battleMode;
        this.battleMode = 3;
      break;
      case 4:
        this.battleMode = 1;
        this.move(this._pokemonServ.currentWildPlace);
      break;
    }
  }

  reducePoints()
  {
    this._shopS.reducePoints(1);
  }
}
