import { Component, OnInit } from '@angular/core';
import { AccountService } from '../services/account.service';
import { Pagination } from '../TypescriptTypes/Pagination';
import { userData } from '../TypescriptTypes/userData';

@Component({
  selector: 'app-ranking',
  templateUrl: './ranking.component.html',
  styleUrls: ['./ranking.component.css']
})
export class RankingComponent implements OnInit {
  users: userData[] = [];
  pageNumber = 1;
  pageSize = 8;
  currentPageSize: boolean = false;
  pagination: Pagination | undefined;
  constructor(public _accS: AccountService) { }

  ngOnInit(): void {
    this.loadUsers(false);
  }

  loadUsers(more: boolean)
  {
    if(more)
    {
      this.pageNumber++;
    }
    else if(this.pageNumber != 1)
    {
      this.pageNumber--;
    }
    this._accS.getUsers(this.pageNumber, this.pageSize).subscribe({
      next: r => {
        if(r.result && r.pagination)
        {
          this.users = r.result;
          this.pagination = r.pagination;
          this.currentPageSize = false;
          if(this.pagination.totalPages == this.pageNumber)
          {
            this.currentPageSize = true;
          }
        }
      }
    })
  }

}
