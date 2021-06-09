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
export class HomeSettingService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }
  getHomeSetting(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'home-setting/get-home-setting', httpOptions);
    return tr
  }
  createHomeSetting(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'home-setting', model, httpOptions);
    return tr
  }
  getHomeSettingInfo(id: number): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'home-setting/' + id, httpOptions);
    return tr
  }

  updateHomeSetting(id, model): Observable<any> {
    var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'home-setting/' + id, model, httpOptionsJson);
    return tr
  }
}
