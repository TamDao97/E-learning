import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Configuration } from '../../shared';
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
export class NumberSubscribersService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  numberSubscribers(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'number-subscribers/statistical', model, httpOptions);
    return tr
  }
  export(programId:any,type:number): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'number-subscribers/'+type+'/export', programId, httpOptions);
    return tr
  }
}
