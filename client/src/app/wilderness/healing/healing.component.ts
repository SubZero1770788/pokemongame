import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { PokemonsService } from '../../services/pokemons.service';
import { pokemon } from '../../TypescriptTypes/Pokemon';
import { usingItem } from 'src/app/TypescriptTypes/usingItem';
import { catchString } from 'src/app/TypescriptTypes/catchString';
import { ColorService } from 'src/app/services/color.service';
import { ShopService } from 'src/app/services/shop.service';
import { UserItems } from 'src/app/TypescriptTypes/UserItems';
import { WildBattlerService } from 'src/app/services/wild-battler.service';

@Component({
  selector: 'app-healing',
  templateUrl: './healing.component.html',
  styleUrls: ['./healing.component.css']
})
export class HealingComponent implements OnInit {
  @Input() userPokemons: Array<pokemon> | null = null;
  @Output() exit = new EventEmitter();
  color: string = 'rgb(155, 220, 155)';
  itemMessage: catchString | null = null;
  currentItem: UserItems | null = null;

  constructor(public _pokeS: PokemonsService, public _colS: ColorService, public _shopS: ShopService, public _battS: WildBattlerService) { }

  ngOnInit(): void {
    this._shopS.itemPasser.subscribe(
      r => {
        this.currentItem = r;
      }
    )
    this._shopS.getItem();
  }

  healPokemon(i : number)
  {
    if(!this.itemMessage)
    {
      let itemData: usingItem = {
        itemId: this.currentItem!.itemId,
        pokemonId: i
      }
      this._pokeS.healPokemon(itemData).subscribe({
        next: r => {
          this.itemMessage = r;
          this.color = this._colS.getMessageColor(this.itemMessage.message, this.color);
        }
      })
    }
  }

  stopHealing()
  {
    this.exit.emit(2);
    this._pokeS.health[1]  = this.itemMessage!.userCurrentHp!;
  }

}
