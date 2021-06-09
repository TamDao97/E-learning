import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from '../share/config/configuration';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class HomeServiceService {

  constructor(private http: HttpClient,
    private config: Configuration) { }

    searchSildeBar(): Observable<any> {
      var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'home-services/search', httpOptions);
      return tr;
    }
}
