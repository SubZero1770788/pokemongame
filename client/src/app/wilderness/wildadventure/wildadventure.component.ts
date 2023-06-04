import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { pokemon } from 'src/app/TypescriptTypes/Pokemon';
import { WildPokemon } from 'src/app/TypescriptTypes/WildPokemon';
import { catchString } from 'src/app/TypescriptTypes/catchString';
import { AccountService } from 'src/app/services/account.service';
import { ColorService } from 'src/app/services/color.service';
import { PokemonsService } from 'src/app/services/pokemons.service';
import { ShopService } from 'src/app/services/shop.service';
import { WildBattlerService } from 'src/app/services/wild-battler.service';

@Component({
  selector: 'app-wildadventure',
  templateUrl: './wildadventure.component.html',
  styleUrls: ['./wildadventure.component.css']
})
export class WildadventureComponent implements OnInit {
  @Input() currentPokemon: any;
  userPokemons: Array<pokemon> | any;
  currentWildPlace: any;
  @Input() messageFrom: catchString | null = null;
  @Output() returnable = new EventEmitter();
  color: string = 'rgb(155, 220, 155)';
  @Input() type1color: string = 'rgb(155, 220, 155)';
  @Input() type2color: string = 'rgb(155, 220, 155)';
  constructor( public _pokS: PokemonsService, public _accS: AccountService, public _wildS: WildBattlerService, public _shopS: ShopService, public _colS: ColorService) {
    this.currentWildPlace = _pokS.currentWildPlace
   }

  ngOnInit(): void {
    this._pokS.outputter.subscribe(
     cs => {
      this.messageFrom = cs;
      this.color = this._colS.getMessageColor(this.messageFrom.message, this.color);
     })
  }

  continue()
  {
    this.messageFrom = null;
    let WildPokemon: WildPokemon = {id: this.currentWildPlace, isfirst: false}
    this._pokS.getEncounter(WildPokemon).subscribe(
      response => {
        this.currentPokemon = response;
        this.type1color = this._colS.getBackgroundColor(this.currentPokemon.pokemonType1Name, this.type1color);
        this.type2color = this._colS.getBackgroundColor(this.currentPokemon.pokemonType2Name, this.type2color);
      }
    )
    this._pokS.getUserPokemon(WildPokemon).subscribe(
      r => {
        this.userPokemons = r;
        this._pokS.passTeam(this.userPokemons);
      }
    )
    this._shopS.itemChosen = false;
    this.reducePoints();
  }

  return()
  {
    this.returnable.emit(0);
  }

  reducePoints()
  {
    this._shopS.reducePoints(1);
  }
}
