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
export class HomeServiceService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }
  searchHomeService(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'home-service/search', model, httpOptions);
    return tr
  }
  deleteHomeService(id: number): Observable<any> {
    var tr = this.http.delete<any>(this.config.ServerWithApiUrl + 'home-service/' + id, httpOptions);
    return tr
  }

  createHomeService(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'home-service', model, httpOptions);
    return tr
  }
  getHomeServiceInfo(id: number): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'home-service/' + id, httpOptions);
    return tr
  }

  updateHomeService(id, model): Observable<any> {
    var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'home-service/' + id, model, httpOptionsJson);
    return tr;
  }
  getListOrder(): Observable<any> {
    return this.http.get<any>(this.config.ServerWithApiUrl + 'home-service/getListOrder');
  }
  updateStatusHomeService(id: string): Observable<any> {
    var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'home-service/update-status/' + id, httpOptions);
    return tr
  }
  updateIndexHomeService(model): Observable<any> {
    var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'home-service/update-index', model, httpOptions);
    return tr
  }
}
