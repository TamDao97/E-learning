import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/app/shared';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

const httpOptionsJson = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class GroupUserService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchGroupUser(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'group-users/search', model, httpOptions);
    return tr
  }

  getGroupUserInfo(id): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'group-users/'+ id);
    return tr
  }

  updateGroupUser(id, model): Observable<any> {
    var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'group-users/'+ id, model);
    return tr
  }

}
