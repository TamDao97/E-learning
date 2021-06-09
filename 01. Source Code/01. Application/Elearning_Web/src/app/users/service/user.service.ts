import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Configuration } from 'src/app/shared';
import { Observable } from 'rxjs';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

const httpOptionsJson = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchMentor(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'employee/search-mentor', model, httpOptions);
    return tr
  }

  getUserStudentByid(id): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'fontend/userlogins/'+id, httpOptions);
    return tr
  }
  
  searchUser(model): Observable<any> {
    return this.http.post<any>(this.config.ServerWithApiUrl + 'users/search', model, httpOptions);
  }

  search(model): Observable<any> {
    return this.http.post<any>(this.config.ServerWithApiUrl + 'users/search-user', model, httpOptions);
  }

  deleteUser(id): Observable<any> {
    return this.http.delete<any>(this.config.ServerWithApiUrl + 'users/delete/'+id, httpOptions);
  }

  createUser(model): Observable<any> {
    return this.http.post<any>(this.config.ServerWithApiUrl + 'users/create',model, httpOptions);
  }

  updateUser(id, model): Observable<any> {
    return this.http.put<any>(this.config.ServerWithApiUrl + 'users/update/'+id,model, httpOptions);
  }

  getUserById(id): Observable<any> {
    return this.http.get<any>(this.config.ServerWithApiUrl + 'users/get-user-by-id/'+id, httpOptions);
  }

  userAdminLock(id): Observable<any> {
    return this.http.put<any>(this.config.ServerWithApiUrl + 'users/UserAdminLock/'+id, httpOptions);
  }

  userAdminUnLock(id): Observable<any> {
    return this.http.put<any>(this.config.ServerWithApiUrl + 'users/UserAdminUnLock/'+id, httpOptions);
  }

  getGroupPermission(id): Observable<any> {
    return this.http.get<any>(this.config.ServerWithApiUrl + 'users/GetGroupPermission/'+id, httpOptions);
  }

  changePassword(id, model): Observable<any> {
    return this.http.put<any>(this.config.ServerWithApiUrl + 'users/change-password/'+id, model, httpOptions);
  }

  getGroupPermissionById(groupUserId: string): Observable<any> {
    return this.http.get<any>(this.config.ServerWithApiUrl + 'users/GetGroupPermissionById/'+groupUserId, httpOptions);
  }

  searchLearner(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'employee/search-learner', model, httpOptions);
    return tr
  }
}
