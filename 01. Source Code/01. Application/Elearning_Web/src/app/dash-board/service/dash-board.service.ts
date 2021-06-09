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
export class DashBoardService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  getTotalAsync(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'dashboard');
    return tr;
  }

  getRegisterCourse(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'dashboard/register-course', model, httpOptions);
    return tr;
  }

  getRegister(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'dashboard/get-register', model, httpOptions);
    return tr;
  }

  getProvince(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'dashboard/get-province', model, httpOptions);
    return tr;
  }
}
