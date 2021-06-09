import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from '..';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class ComboboxService {

  constructor(private http: HttpClient,
    private config: Configuration) { }

  getListGroupuser(): Observable<any> {
    return this.http.get<any>(this.config.ServerWithApiUrl + 'combobox/GetListGroupuser', httpOptions);
  }

  getCategory(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'combobox/get-category', httpOptions);
    return tr;
  }

  getProgram(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'combobox/get-program', httpOptions);
    return tr;
  }

  getTopic(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'combobox/get-topic', httpOptions);
    return tr;
  }



  getEmployeeAsync(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'combobox/employees', httpOptions);
    return tr
  }

  getHomeSpecialistAsync(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'combobox/home-specialist', httpOptions);
    return tr
  }
}
