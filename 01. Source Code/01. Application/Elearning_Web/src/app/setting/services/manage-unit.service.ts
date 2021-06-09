import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
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
export class ManageUnitService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }
  searchManageUnit(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'manage-unit/search', model, httpOptions);
    return tr
  }
  deleteManageUnit(id: string): Observable<any> {
    var tr = this.http.delete<any>(this.config.ServerWithApiUrl + 'manage-unit/' + id, httpOptions);
    return tr
  }

  createManageUnit(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'manage-unit', model, httpOptions);
    return tr
  }
  getManageUnitInfo(id: string): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'manage-unit/' + id, httpOptions);
    return tr
  }

  updateManageUnit(id, model): Observable<any> {
    var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'manage-unit/' + id, model, httpOptionsJson);
    return tr;
  }
}
