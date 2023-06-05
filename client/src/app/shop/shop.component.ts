import { HttpClient } from '@angular/common/http';
import { Component, OnInit} from '@angular/core';
import { ShopService } from '../services/shop.service';
import { Item } from '../TypescriptTypes/Item';
import { PokemonsService } from '../services/pokemons.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.css']
})
export class ShopComponent implements OnInit {
  baseUrl = environment.baseUrl
  items: any;
  shop: any = [{key: 1, values: ''}];
  valid: boolean = false;
  itemToBuy: Item = {id: 0, amount: 0};
 

  constructor(private http: HttpClient, public _shopS: ShopService, public _pokS: PokemonsService) { }

  ngOnInit(): void {
    this.loadItems();
  }

  loadItems()
  {
    this.http.get(this.baseUrl + "items").subscribe({
      next: r => this.items = r,
      error: e => console.log(e),
      complete: () => this.saveItems()
    })
  }

  buyItem(num:number, amount: number)
  {
    this.itemToBuy.id = num;
    this.itemToBuy.amount = amount;

    this.reducePoints(this.shop[num-1].values*this.items[num-1].price);
    
    this.shop[num-1].values = '';
    this._shopS.buyItem(this.itemToBuy);
    this._pokS.points -=amount*this.items[num-1].value;
  }

  saveItems()
  {
    for(let i = 1; i< this.items.length; i++)
    {
      this.shop.push({key: this.items.id, value: undefined});
    }
  }

  reducePoints(n: number)
  {
    this._shopS.reducePoints(n);
  }
}
