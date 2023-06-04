import { HttpClient, HttpParams } from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../TypescriptTypes/User';
import { PaginatedResult } from '../TypescriptTypes/Pagination';
import { userData } from '../TypescriptTypes/userData';
import { environment } from 'src/environments/environment';


@Injectable({
  providedIn: 'root'
})
export class AccountService {
  startUrl = environment.baseUrl;
  private currentUsersrc = new BehaviorSubject<User | null>(null);
  currentUsersrc$ = this.currentUsersrc.asObservable();
  points: number = 1;
  userLevel: number = 0;
  pointEmitter = new EventEmitter();
  paginatedResult: PaginatedResult<userData[]> = new PaginatedResult<any[]>;

  constructor(private http:HttpClient) { }

  login(data: any)
  {
    return this.http.post<User>(this.startUrl + 'login', data).pipe(
      map((r: User) => {
        const u = r;
        if(u) {
          this.points = u.points;
          sessionStorage.setItem('Token', JSON.stringify(u.token));
          this.currentUsersrc.next(u);
          this.points = u.points;
          this.userLevel = u.level
          this.pointEmitter.emit(this.points);
        }
      })
    )
  }

  getUsers(page?: number, itemsPerPage?: number)
  {
    let params = new HttpParams();

    if(page && itemsPerPage)
    {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    return this.http.get<userData[]>(this.startUrl + "users", 
    {observe: "response", params}).pipe(
      map(response => {
        if(response.body) {
          this.paginatedResult.result = response.body;
        }
        const pag = response.headers.get("Pagination");
        if(pag)
        {
          this.paginatedResult.pagination = JSON.parse(pag);
        }
        return this.paginatedResult;
      })
    )
  }

  setCurrentUser(u : User){
    this.currentUsersrc.next(u);
  }

  logout() {
    sessionStorage.removeItem('Token');
    this.currentUsersrc.next(null);
  }

  registerUser(userData: any)
  {
    return this.http.post<User>(this.startUrl +'register', userData).pipe(
      map(u => {
        if (u) {
          sessionStorage.setItem('Token', JSON.stringify(u.token));
          this.currentUsersrc.next(u);
          this.points = u.points,
          this.userLevel = u.level,
          this.pointEmitter.emit(this.points);
        }
      })
    )
  }
}
