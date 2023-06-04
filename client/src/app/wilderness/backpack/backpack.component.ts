import { Component, EventEmitter, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { UserItems } from '../../TypescriptTypes/UserItems';
import { PokemonsService } from '../../services/pokemons.service';
import { Item } from '../../TypescriptTypes/Item';
import { ShopService } from 'src/app/services/shop.service';

@Component({
  selector: 'app-backpack',
  templateUrl: './backpack.component.html',
  styleUrls: ['./backpack.component.css']
})
export class BackpackComponent implements OnInit {

  userItems: Array<UserItems> | null = null;
  @Output() healingMode = new EventEmitter();
  @Output() itemUsed = new EventEmitter();

  constructor(public _pokeS: PokemonsService, public _shopS: ShopService) { }

  ngOnInit(): void 
  {
    this.loadItems();
  }

  loadItems()
  {
    let searchItems: Item = {id: 0, amount: 0}
    this._shopS.displayItems(searchItems).subscribe(
      response => {
        this.userItems = response;
      } 
    )
  }

  useItem(index: number)
  {
    switch(this.userItems![index].name)
    {
      case 'Potion' :
      this.healingMode.emit(3);
      this._shopS.passItem(this.userItems![index]);
      break;
      case 'Super Potion' :
      this.healingMode.emit(3);
      this._shopS.passItem(this.userItems![index]);
      break;
      case 'Hyper Potion' :
      this.healingMode.emit(3);
      this._shopS.passItem(this.userItems![index]);
      break;
      case 'Pokeball' :
      this.tryToCatch(this.userItems![index].itemId);
      this.ballRemover(index);
      break;
      case 'Greatball' :
      this.tryToCatch(this.userItems![index].itemId);
      this.ballRemover(index);
      break;
      case 'Ultraball' :
      this.tryToCatch(this.userItems![index].itemId);
      this.ballRemover(index);
      break;
      case 'Masterball' :
      this.tryToCatch(this.userItems![index].itemId);
      this.ballRemover(index);
      break;
    }
  }

  tryToCatch(i: number)
  {
    let usedItem: Item = {
      id: i,
      amount: 1,
    };
    this._pokeS.tryCatching(usedItem).subscribe(
        response => {
            this._pokeS.passForm(response);
      }
    )
  }

  ballRemover(i: number)
  {
    this.userItems![i].amount--;
    if(this.userItems![i].amount == 0)
    [
      this.userItems!.splice(i, 1)
    ]
    this._shopS.itemChosen = true;
  }

}
