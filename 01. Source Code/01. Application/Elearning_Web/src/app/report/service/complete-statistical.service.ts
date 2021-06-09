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
export class CompleteStatisticalService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }
  completeStatistical(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'complete-statistic', model, httpOptions);
    return tr
  }
  export(model: any, type:number): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'complete-statistic/' +type+'/export', model, httpOptions);
    return tr
  }
}
