import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';
import { Item } from '../TypescriptTypes/Item';
import { AccountService } from './account.service';
import { UserItems } from '../TypescriptTypes/UserItems';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  startUrl = environment.baseUrl;
  currentItem: UserItems | null = null;
  pointPasser = new EventEmitter();
  itemPasser = new EventEmitter();
  itemChosen: boolean = false;

  constructor(private http: HttpClient, public _accS: AccountService) { }

  buyItem(i: Item)
  {
    this.http.post(this.startUrl + "items/" + i.id.toString(), i).subscribe();
  }

  displayItems(currentUserItems : Item)
  {
    return this.http.post<Array<UserItems>>(this.startUrl + "userItems", currentUserItems);
  }

  reducePoints(n:number)
  {
    this.pointPasser.emit(n);
  }

  passItem(it: UserItems)
  {
    this.currentItem = it;
  }

  getItem()
  {
    this.itemPasser.emit(this.currentItem);
    this.currentItem = null;
  }
}
