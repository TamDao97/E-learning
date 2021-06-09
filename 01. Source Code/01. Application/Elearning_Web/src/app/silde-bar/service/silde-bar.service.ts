import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/app/shared';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};


@Injectable({
  providedIn: 'root'
})
export class SildeBarService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }
  
  searchSildeBar(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'sildebars/search', model, httpOptions);
    return tr;
  }

  createSildeBar(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'sildebars', model, httpOptions);
    return tr;
  }

  updateSildeBar(id: number, model: any): Observable<any> {
    var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'sildebars/' + id, model, httpOptions);
    return tr;
  }

  updateIndex(model: any): Observable<any> {
    var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'sildebars/update-index', model, httpOptions);
    return tr;
  }

  deleteSildeBar(id: number): Observable<any> {
    var tr = this.http.delete<any>(this.config.ServerWithApiUrl + 'sildebars/' + id);
    return tr;
  }

  getByIdSildeBar(id: number): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'sildebars/' + id);
    return tr;
  }

  getListDisplayIndex(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'combobox/search-home-slider', httpOptions);
    return tr;
  }
}
