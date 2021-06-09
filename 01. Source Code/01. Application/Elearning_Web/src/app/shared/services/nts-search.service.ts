import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Configuration } from '../config/configuration';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class NtsSearchService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  getCustomers(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'combobox/get-customers', httpOptions);
    return tr
  }

  getProducts(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'combobox/get-products', httpOptions);
    return tr
  }
}
