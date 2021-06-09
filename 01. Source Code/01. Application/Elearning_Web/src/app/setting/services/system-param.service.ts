import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/app/shared';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class SystemParamService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  getListSystemParam(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'system-param', httpOptions);
    return tr;
  }

  updateSystemParam(model: any): Observable<any> {
    var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'system-param', model, httpOptions);
    return tr
  }
}
